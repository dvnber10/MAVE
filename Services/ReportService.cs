using MAVE.DTO;
using MAVE.Models;
using MAVE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MAVE.Services
{
    public class ReportService
    {
        private readonly ReportRepository _repo;
        private readonly DbAa60a4MavetestContext _db;
        public ReportService(ReportRepository repo, DbAa60a4MavetestContext db)
        {
            _repo = repo;
            _db = db;
        }
        public async Task<InitialReportDTO?> GetInitialReport(int? id)
        {
            try
            {
                InitialReportDTO? ir = new InitialReportDTO();
                ir = await _repo.GetInitialReport(id);
                if (ir == null) return null;
                else return ir;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Tiempo en actividades: meditación (AUDITORY Action=MEDITATION),
        /// conteo de ánimos (Moods) y hábitos completados (Questions con Score).
        /// </summary>
        private static int SecondsOf(string? nv)
        {
            int sec = 0;
            var s = nv ?? string.Empty;
            int p = s.IndexOf('s');
            if (p > 0) int.TryParse(s.Substring(0, p), out sec);
            return sec;
        }

        private static readonly string[] _timedActions = new string[]
            { "MEDITATION", "MINDFULNESS", "YOGA", "BREATHING", "BODYSCAN", "WALK" };

        /// <summary>KPIs de uso de la aplicación para el administrador.</summary>
        public async Task<KpiDTO> GetKpis()
        {
            var dto = new KpiDTO();
            dto.TotalUsers = await _db.Users.CountAsync();
            dto.SuperAdmins = await _db.Users.CountAsync(u => u.RoleId == 1);
            dto.Admins = await _db.Users.CountAsync(u => u.RoleId == 2);
            dto.Psychologists = await _db.Users.CountAsync(u => u.RoleId == 3);
            dto.Patients = await _db.Users.CountAsync(u => u.RoleId == 4);
            var since = DateTime.Now.Date.AddDays(-6);
            var active = new HashSet<int>();
            foreach (var id in await _db.Moods.Where(m => m.Date.Date >= since).Select(m => m.UserId).ToListAsync()) active.Add(id);
            foreach (var id in await _db.Questions.Where(q => q.Date >= DateOnly.FromDateTime(since)).Select(q => q.UserId).ToListAsync()) active.Add(id);
            foreach (var id in await _db.Auditories.Where(a => a.Date.Date >= since).Select(a => a.UserId).ToListAsync()) active.Add(id);
            foreach (var id in await _db.ChatMessages.Where(m => m.Date.Date >= since).Select(m => m.SenderId).ToListAsync()) active.Add(id);
            dto.ActiveUsers7d = active.Count;
            int sec = 0;
            var timed = await _db.Auditories.Where(a => _timedActions.Contains(a.Action)).ToListAsync();
            foreach (var l in timed) sec += SecondsOf(l.NewValue);
            dto.ActivityMinutes = sec / 60;
            dto.ChatMessages = await _db.ChatMessages.CountAsync();
            var phq = await _db.Auditories.Where(a => a.Action == "MOOD_PHQ4" && a.Date.Date >= DateTime.Now.Date.AddDays(-29)).ToListAsync();
            var totals = new List<int>();
            foreach (var r in phq)
                foreach (var part in (r.NewValue ?? string.Empty).Split(','))
                {
                    var kv = part.Split(':');
                    if (kv.Length == 2 && kv[0] == "T" && int.TryParse(kv[1], out int t)) { totals.Add(t); break; }
                }
            dto.Phq4Count30d = totals.Count;
            dto.Phq4Avg30d = totals.Count > 0 ? Math.Round(totals.Average(), 1) : 0;
            return dto;
        }

        /// <summary>Última actividad registrada (auditoría) para monitoreo del admin.</summary>
        public async Task<List<AdminActivityLogDTO>> GetActivityLog(int take = 50)
        {
            if (take < 1) take = 1;
            if (take > 200) take = 200;
            var rows = await _db.Auditories.OrderByDescending(a => a.Date).Take(take).ToListAsync();
            var uids = rows.Select(r => r.UserId).Distinct().ToList();
            var names = await _db.Users.Where(u => uids.Contains(u.UserId)).ToDictionaryAsync(u => u.UserId, u => u.UserName);
            return rows.Select(r => new AdminActivityLogDTO
            {
                Date = r.Date,
                UserName = names.TryGetValue(r.UserId, out var n) ? n : ("#" + r.UserId),
                Action = r.Action ?? string.Empty,
                Detail = (r.NewValue ?? string.Empty).Length > 60 ? (r.NewValue ?? string.Empty).Substring(0, 60) : (r.NewValue ?? string.Empty)
            }).ToList();
        }

        private static string Band(int total)
        {
            if (total <= 2) return "Mínima";
            if (total <= 5) return "Leve";
            if (total <= 8) return "Moderada";
            return "Severa";
        }

        private async Task<List<Phq4PointDTO>> Phq4HistoryPoints(int? id, int take)
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

        /// <summary>
        /// Reporte clínico general para psicólogo: paciente, actividad por tipo,
        /// PHQ-4 (último + banda + historial) y adherencia de 30 días.
        /// </summary>
        public async Task<ClinicalReportDTO?> GetClinicalReport(int? id)
        {
            var user = await _db.Users
                .Where(u => u.UserId == id)
                .Select(u => new { u.UserName, u.Email })
                .FirstOrDefaultAsync();
            if (user == null) return null;
            var dto = new ClinicalReportDTO { PatientName = user.UserName, Email = user.Email };
            dto.Activity = await GetActivityTime(id);
            var hist = await Phq4HistoryPoints(id, 14);
            dto.Phq4History = hist;
            var last = hist.LastOrDefault();
            if (last != null)
                dto.LastPhq4 = new Phq4LastDTO
                {
                    Date = last.Date, D = last.D, A = last.A, Total = last.Total,
                    Band = Band(last.Total), NeedsFollowUp = last.D >= 3 || last.A >= 3
                };
            var since = DateTime.Now.Date.AddDays(-29);
            var moodDays = await _db.Moods
                .Where(m => m.UserId == id && m.Date.Date >= since)
                .Select(m => m.Date.Date).ToListAsync();
            var phqDays = await _db.Auditories
                .Where(a => a.UserId == id && a.Action == "MOOD_PHQ4" && a.Date.Date >= since)
                .Select(a => a.Date.Date).ToListAsync();
            dto.DaysMood30 = moodDays.Union(phqDays).Count();
            var sinceQ = DateOnly.FromDateTime(since);
            dto.DaysHabits30 = await _db.Questions
                .Where(q => q.UserId == id && q.ScoreId != null && q.Date >= sinceQ)
                .Select(q => q.Date).Distinct().CountAsync();
            dto.RecentHabits = await _db.Questions
                .Where(q => q.UserId == id && q.ScoreId != null)
                .OrderByDescending(q => q.Date)
                .Take(10)
                .Join(_db.CatQuestions, q => q.CatQuestionId, c => c.CatQuestionId,
                    (q, c) => new { q, c })
                .GroupJoin(_db.CatOptions, x => x.q.OptionId, o => o.OptionId,
                    (x, os) => new { x.q, x.c, o = os.FirstOrDefault() })
                .Select(x => new HabitAnswerDTO
                {
                    Date = x.q.Date.ToString("dd/MM"),
                    Question = x.c.Question,
                    Answer = x.o != null ? x.o.EvaOption : "-"
                })
                .ToListAsync();
            return dto;
        }

        public async Task<ActivityTimeDTO> GetActivityTime(int? id)
        {
            int uid = id ?? 0;
            var dto = new ActivityTimeDTO();
            var logs = await _db.Auditories
                .Where(a => a.UserId == uid && a.Action == "MEDITATION")
                .ToListAsync();
            int total = 0;
            var perDay = new Dictionary<DateTime, int>();
            foreach (var l in logs)
            {
                int sec = 0;
                var nv = l.NewValue ?? string.Empty;
                int sPos = nv.IndexOf('s');
                if (sPos > 0) int.TryParse(nv.Substring(0, sPos), out sec);
                if (sec <= 0) continue;
                total += sec;
                var day = l.Date.Date;
                perDay[day] = perDay.TryGetValue(day, out int acc) ? acc + sec : sec;
            }
            dto.MeditationSeconds = total;
            dto.MeditationSessions = logs.Count;
            var allLogs = await _db.Auditories.Where(a => a.UserId == uid).ToListAsync();
            var byType = new Dictionary<string, (int Sec, int Count)>();
            foreach (var l in allLogs)
            {
                int sec = 0;
                var nv2 = l.NewValue ?? string.Empty;
                int p = nv2.IndexOf('s');
                if (p > 0) int.TryParse(nv2.Substring(0, p), out sec);
                if (sec <= 0) continue;
                string act = l.Action ?? "OTRO";
                byType[act] = byType.TryGetValue(act, out var cur) ? (cur.Sec + sec, cur.Count + 1) : (sec, 1);
            }
            foreach (var kv in byType)
                dto.ByType.Add(new TypeTimeDTO { Action = kv.Key, Seconds = kv.Value.Sec, Sessions = kv.Value.Count });
            dto.MoodsCount = await _db.Moods.CountAsync(m => m.UserId == uid)
                + await _db.Auditories.CountAsync(a => a.UserId == uid && a.Action == "MOOD_PHQ4");
            dto.HabitsCount = await _db.Questions.CountAsync(q => q.UserId == uid && q.ScoreId != null);
            for (int i = 6; i >= 0; i--)
            {
                var day = DateTime.Now.Date.AddDays(-i);
                dto.Last7Days.Add(new DayTimeDTO
                {
                    Date = day.ToString("dd/MM"),
                    Seconds = perDay.TryGetValue(day, out int s) ? s : 0
                });
            }
            return dto;
        }
    }
}