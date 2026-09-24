using MAVE.DTO;
using MAVE.Models;
using MAVE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MAVE.Services
{
    public class MoodService
    {
        private readonly MoodRepository _repo;
        private readonly DbAa60a4MavetestContext _db;

        public MoodService(MoodRepository repo, DbAa60a4MavetestContext db)
        {
            _repo = repo;
            _db = db;
        }

        /// <summary>
        /// Guarda PHQ-4: total en Moods + desglose D/A/T en AUDITORY. 1 por día.
        /// Devuelve 1 ok, 0 datos inválidos, 2 ya registrado hoy.
        /// </summary>
        public async Task<int> SetPhq4(Phq4DTO dto, int? id)
        {
            if (dto == null || id == null || id <= 0) return 0;
            if (dto.D1 < 0 || dto.D1 > 3 || dto.D2 < 0 || dto.D2 > 3 ||
                dto.D3 < 0 || dto.D3 > 3 || dto.D4 < 0 || dto.D4 > 3) return 0;
            try
            {
                var today = DateTime.Now.Date;
                if (await _db.Auditories.AnyAsync(a => a.UserId == id && a.Action == "MOOD_PHQ4" && a.Date.Date == today)) return 2;
                int d = dto.D1 + dto.D2;
                int a = dto.D3 + dto.D4;
                _db.Auditories.Add(new Auditory
                {
                    UserId = id.Value,
                    Action = "MOOD_PHQ4",
                    OldValue = null,
                    NewValue = "D:" + d + ",A:" + a + ",T:" + (d + a),
                    Date = DateTime.Now
                });
                await _db.SaveChangesAsync();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>Historial PHQ-4 (fecha, D, A, total) en orden cronológico.</summary>
        public async Task<List<Phq4PointDTO>> Phq4History(int? id, int take = 30)
        {
            var list = new List<Phq4PointDTO>();
            var rows = await _db.Auditories
                .Where(a => a.UserId == id && a.Action == "MOOD_PHQ4")
                .OrderByDescending(a => a.Date)
                .Take(take)
                .ToListAsync();
            foreach (var r in rows)
            {
                var nv = r.NewValue ?? string.Empty;
                int d = 0, a = 0, t = 0;
                foreach (var part in nv.Split(','))
                {
                    var kv = part.Split(':');
                    if (kv.Length != 2) continue;
                    if (kv[0] == "D") int.TryParse(kv[1], out d);
                    else if (kv[0] == "A") int.TryParse(kv[1], out a);
                    else if (kv[0] == "T") int.TryParse(kv[1], out t);
                }
                list.Add(new Phq4PointDTO { Date = r.Date.ToString("dd/MM"), D = d, A = a, Total = t });
            }
            list.Reverse();
            return list;
        }

        private static int Phq4TotalOf(string? nv)
        {
            foreach (var part in (nv ?? string.Empty).Split(','))
            {
                var kv = part.Split(':');
                if (kv.Length == 2 && kv[0] == "T" && int.TryParse(kv[1], out int t)) return t;
            }
            return -1;
        }

        /// <summary>
        /// Análisis psicológico del seguimiento anímico: excluye filas Moods
        /// contaminadas por PHQ-4 (mismo día + mismo total en AUDITORY),
        /// calcula promedio, distribución, volatilidad, racha, tendencia
        /// e índice compuesto 0-100 (caritas 50% + PHQ-4 30% + adherencia 20%).
        /// En caritas 1 = muy bueno: promedios BAJOS son mejores.
        /// </summary>
        public async Task<MoodAnalysisDTO> Analysis(int? id)
        {
            var dto = new MoodAnalysisDTO();
            var audits = await _db.Auditories
                .Where(a => a.UserId == id && a.Action == "MOOD_PHQ4")
                .ToListAsync();
            var phqKeys = new HashSet<string>(audits.Select(a =>
                a.Date.Date.ToString("yyyyMMdd") + "#" + Phq4TotalOf(a.NewValue)));
            var moods = await _db.Moods.Where(m => m.UserId == id).ToListAsync();
            var faces = moods
                .Where(m => !phqKeys.Contains(m.Date.Date.ToString("yyyyMMdd") + "#" + m.MoodScore))
                .OrderBy(m => m.Date).ToList();

            dto.FacesCount = faces.Count;
            dto.Distribution = new List<int> { 0, 0, 0, 0, 0 };
            foreach (var f in faces)
                if (f.MoodScore >= 1 && f.MoodScore <= 5) dto.Distribution[f.MoodScore - 1]++;

            var last14 = faces.OrderByDescending(m => m.Date).Take(14).Reverse().ToList();
            if (last14.Count > 0)
            {
                dto.FacesAvg = Math.Round(last14.Average(m => (double)m.MoodScore), 2);
                double mean = dto.FacesAvg;
                dto.Volatility = Math.Round(Math.Sqrt(last14.Average(m => Math.Pow(m.MoodScore - mean, 2))), 2);
                dto.FacesTrend = last14
                    .Select(m => new MoodTrendPointDTO { Date = m.Date.ToString("dd/MM"), Score = m.MoodScore })
                    .ToList();
            }
            if (last14.Count >= 4)
            {
                int half = last14.Count / 2;
                double prev = last14.Take(half).Average(m => (double)m.MoodScore);
                double recent = last14.Skip(half).Average(m => (double)m.MoodScore);
                dto.Trend = recent < prev - 0.3 ? "improving" : recent > prev + 0.3 ? "needs-attention" : "stable";
            }

            var activeDays = new HashSet<DateTime>(faces.Select(m => m.Date.Date));
            foreach (var a in audits) activeDays.Add(a.Date.Date);
            dto.DaysActive30 = activeDays.Count(d => d >= DateTime.Now.Date.AddDays(-29));
            int streak = 0;
            var day = DateTime.Now.Date;
            if (!activeDays.Contains(day)) day = day.AddDays(-1);
            while (activeDays.Contains(day)) { streak++; day = day.AddDays(-1); }
            dto.StreakDays = streak;

            var lastPhq = audits.OrderByDescending(a => a.Date).FirstOrDefault();
            int? phqTotal = lastPhq == null ? null : (int?)Phq4TotalOf(lastPhq.NewValue);
            if (lastPhq != null && phqTotal >= 0)
                dto.LastPhq4 = new Phq4LastDTO
                {
                    Date = lastPhq.Date.ToString("dd/MM"), Total = phqTotal.Value,
                    Band = phqTotal <= 2 ? "Mínima" : phqTotal <= 5 ? "Leve" : phqTotal <= 8 ? "Moderada" : "Severa",
                    NeedsFollowUp = false, D = 0, A = 0
                };

            if (faces.Count > 0)
            {
                double faceScore = (5 - dto.FacesAvg) / 4 * 100;
                double adherence = dto.DaysActive30 / 30.0 * 100;
                double index;
                if (phqTotal >= 0)
                    index = 0.5 * faceScore + 0.3 * ((12 - phqTotal.Value) / 12.0 * 100) + 0.2 * adherence;
                else
                    index = 0.7 * faceScore + 0.3 * adherence;
                dto.Index = (int)Math.Round(index);
                dto.IndexBand = dto.Index < 40 ? "Necesita atención" : dto.Index < 70 ? "Estable" : "Saludable";
            }
            return dto;
        }

        public async Task<int> SetMood(MoodDTO mood, int? id)
        {
            if (mood == null)
            {
                return 0;
            }
            else
            {
                if(await _repo.SetMood(mood, id) == 1)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }

        public async Task<int> GetMood(int? id)
        {
            try
            {
                if(await _repo.GetMood(id) == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch(Exception)
            {
                return 2;
            }   
        }

        public async Task<MoodGraphicDTO?> GetMoodGraphic(int? id)
        {
            try
            {
                MoodGraphicDTO? mood = await _repo.GetMoodGraphic(id);
                return mood;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}