using System.Net.Http.Headers;
using System.Text.Json;
using MAVE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace MAVE.Services
{
    public sealed record FreesoundTrack(int Id, string Name, string Username, double Duration);

    /// <summary>
    /// Sonidos aleatorios de meditación vía Freesound API v2.
    /// Término + página + resultado aleatorios, solo licencias CC0
    /// (libres de derechos). Caché temporal para no saturar la API externa.
    /// </summary>
    public class MeditationService
    {
        private readonly IHttpClientFactory _http;
        private readonly IMemoryCache _cache;
        private readonly DbAa60a4MavetestContext _db;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private static readonly Random _rnd = new Random();

        private static readonly string[] _terms = new string[]
        {
            "meditation ambient", "rain forest", "ocean waves", "tibetan singing bowl",
            "night crickets", "fireplace crackling", "wind chimes", "river stream",
            "distant thunderstorm", "morning birds", "zen garden", "deep drone calm"
        };

        private const string _filter = "license:\"Creative Commons 0\" duration:[300 TO 1800]";

        public MeditationService(IHttpClientFactory http, IMemoryCache cache, IConfiguration config, DbAa60a4MavetestContext db)
        {
            _http = http;
            _cache = cache;
            _db = db;
            _apiKey = config["Freesound:ApiKey"] ?? string.Empty;
            _baseUrl = (config["Freesound:BaseUrl"] ?? "https://freesound.org/apiv2/").TrimEnd('/') + "/";
        }

        public bool IsConfigured()
        {
            return !string.IsNullOrWhiteSpace(_apiKey) && !_apiKey.StartsWith("PEGA");
        }

        /// <summary>
        /// Lista N pistas distintas (término + página + resultado aleatorios).
        /// GET /api/meditation/sounds?count=N
        /// </summary>
        /// <summary>
        /// Registra segundos de cualquier actividad del catálogo en AUDITORY.
        /// </summary>
        public async Task LogActivityAsync(int userId, string action, int seconds)
        {
            if (userId <= 0 || seconds <= 0) return;
            _db.Auditories.Add(new Auditory
            {
                UserId = userId,
                Action = action,
                OldValue = null,
                NewValue = seconds + "s",
                Date = DateTime.Now
            });
            await _db.SaveChangesAsync();
        }

        public async Task<List<FreesoundTrack>> GetSoundsAsync(int count)
        {
            if (count < 1) count = 1;
            if (count > 20) count = 20;
            var picked = new List<FreesoundTrack>();
            var seen = new HashSet<int>();
            for (int attempt = 0; attempt < 40 && picked.Count < count; attempt++)
            {
                string term = _terms[_rnd.Next(_terms.Length)];
                int page = _rnd.Next(1, 9);
                var tracks = await SearchCachedAsync(term, page);
                if (tracks.Count == 0 && page != 1) tracks = await SearchCachedAsync(term, 1);
                foreach (var t in tracks)
                {
                    if (t.Duration < 300) continue; // garantía local: mínimo 5 minutos
                    if (seen.Add(t.Id)) picked.Add(t);
                    if (picked.Count >= count) break;
                }
            }
            return picked;
        }

        /// <summary>
        /// Registra segundos escuchados en AUDITORY (Action=MEDITATION, NewValue="120s#817231").
        /// </summary>
        public async Task LogTimeAsync(int userId, int soundId, int seconds)
        {
            if (userId <= 0 || seconds <= 0) return;
            _db.Auditories.Add(new Auditory
            {
                UserId = userId,
                Action = "MEDITATION",
                OldValue = null,
                NewValue = seconds + "s#" + soundId,
                Date = DateTime.Now
            });
            await _db.SaveChangesAsync();
        }

        public async Task<FreesoundTrack?> GetRandomSoundAsync()
        {
            string term = _terms[_rnd.Next(_terms.Length)];
            int page = _rnd.Next(1, 9);

            var tracks = await SearchCachedAsync(term, page);
            if ((tracks == null || tracks.Count == 0) && page != 1)
            {
                tracks = await SearchCachedAsync(term, 1);
            }
            if (tracks == null || tracks.Count == 0) return null;
            return tracks[_rnd.Next(tracks.Count)];
        }

        /// <summary>
        /// Transmite el preview en streaming (sin cargarlo en memoria) y
        /// reenvía el header Range para que el &lt;audio&gt; pueda adelantar/retroceder.
        /// </summary>
        public async Task StreamPreviewAsync(int id, HttpResponse response, string? rangeHeader, CancellationToken ct)
        {
            string previewUrl = await GetPreviewUrlAsync(id);
            var client = _http.CreateClient("freesound");
            using var req = new HttpRequestMessage(HttpMethod.Get, previewUrl);
            req.Headers.Authorization = new AuthenticationHeaderValue("Token", _apiKey);
            if (!string.IsNullOrEmpty(rangeHeader))
                req.Headers.TryAddWithoutValidation("Range", rangeHeader);
            using var res = await client.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            if (!res.IsSuccessStatusCode && res.StatusCode != System.Net.HttpStatusCode.PartialContent)
                res.EnsureSuccessStatusCode();

            response.StatusCode = (int)res.StatusCode;
            var ch = res.Content.Headers;
            if (ch.ContentType != null) response.ContentType = ch.ContentType.ToString();
            if (ch.ContentLength.HasValue) response.ContentLength = ch.ContentLength;
            if (ch.ContentRange != null) response.Headers["Content-Range"] = ch.ContentRange.ToString();
            response.Headers["Accept-Ranges"] = "bytes";
            response.Headers["Cache-Control"] = "public, max-age=1800";

            using var stream = await res.Content.ReadAsStreamAsync(ct);
            await stream.CopyToAsync(response.Body, ct);
        }

        private async Task<List<FreesoundTrack>> SearchCachedAsync(string term, int page)
        {
            string key = "fs:search:v2:" + term + ":" + page;
            if (_cache.TryGetValue(key, out List<FreesoundTrack>? cached) && cached != null)
                return cached;

            var tracks = await SearchFreesoundAsync(term, page);
            _cache.Set(key, tracks, TimeSpan.FromMinutes(5));
            return tracks;
        }

        private async Task<List<FreesoundTrack>> SearchFreesoundAsync(string term, int page)
        {
            var list = new List<FreesoundTrack>();
            var client = _http.CreateClient("freesound");
            string url = _baseUrl + "search/text/?query=" + Uri.EscapeDataString(term)
                + "&page_size=15&page=" + page
                + "&filter=" + Uri.EscapeDataString(_filter)
                + "&fields=id,name,username,duration,license";
            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.Authorization = new AuthenticationHeaderValue("Token", _apiKey);
            using var res = await client.SendAsync(req);
            if (res.StatusCode == System.Net.HttpStatusCode.NotFound)
                return list; // página fuera de rango: se reintenta con otra
            res.EnsureSuccessStatusCode();
            using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
            if (doc.RootElement.TryGetProperty("results", out JsonElement results))
            {
                foreach (var r in results.EnumerateArray())
                {
                    int id = r.TryGetProperty("id", out JsonElement eId) ? eId.GetInt32() : 0;
                    string name = r.TryGetProperty("name", out JsonElement eName) ? (eName.GetString() ?? "Sin título") : "Sin título";
                    string user = r.TryGetProperty("username", out JsonElement eUser) ? (eUser.GetString() ?? "Freesound") : "Freesound";
                    double dur = r.TryGetProperty("duration", out JsonElement eDur) ? eDur.GetDouble() : 0;
                    if (id > 0) list.Add(new FreesoundTrack(id, name, user, dur));
                }
            }
            return list;
        }

        private async Task<string> GetPreviewUrlAsync(int id)
        {
            string key = "fs:preview:" + id;
            if (_cache.TryGetValue(key, out string? cached) && !string.IsNullOrEmpty(cached))
                return cached;

            var client = _http.CreateClient("freesound");
            string url = _baseUrl + "sounds/" + id + "/?fields=id,name,previews";
            using var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.Authorization = new AuthenticationHeaderValue("Token", _apiKey);
            using var res = await client.SendAsync(req);
            res.EnsureSuccessStatusCode();
            using var doc = JsonDocument.Parse(await res.Content.ReadAsStringAsync());
            string preview = string.Empty;
            if (doc.RootElement.TryGetProperty("previews", out JsonElement pv))
            {
                if (pv.TryGetProperty("preview-hq-mp3", out JsonElement hq)) preview = hq.GetString() ?? string.Empty;
                if (string.IsNullOrEmpty(preview) && pv.TryGetProperty("preview-lq-mp3", out JsonElement lq)) preview = lq.GetString() ?? string.Empty;
            }
            if (string.IsNullOrEmpty(preview)) throw new InvalidOperationException("Freesound no devolvió preview para el sonido " + id);
            _cache.Set(key, preview, TimeSpan.FromMinutes(30));
            return preview;
        }
    }
}
