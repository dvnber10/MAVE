using MAVE.Repositories;
using MAVE.Utilities;

namespace MAVE.Services
{
    public class NotifyService
    {
        private readonly NotifyRepository _repo;
        private readonly WhatsAppUtility _uti;
        public NotifyService(NotifyRepository repo, WhatsAppUtility uti)
        {   
            _repo = repo;
            _uti = uti;
        }
        public async Task<int> SendMessages()
        {
            try 
            {
                var mood = await _repo.MoodReminder();
                if (mood != null)
                {
                    foreach(var m in mood)
                    {
                        int res = await _uti.SendMessage("Hola, te escribimos de MAVE tu aplicación para el seguimiento anímico, queremos recordarte que debes poner tu estado de ánimo de hoy :3",
                        m.Phone);
                        if (res == 0) return 2;
                    }
                }
                var habit = await _repo.HabitReminder();
                if(habit != null)
                {
                    foreach(var h in habit)
                    {
                        int res = await _uti.SendMessage("Hola, te escribimos de MAVE tu aplicación para el seguimiento anímico, queremos recordarte que debes hacer tu encuesta de hábitos :3",
                        h.Phone);
                        if(res == 0) return 2;
                    }
                }
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        private static readonly string[] _tips = new string[]
        {
            "Respira 4-7-8 por 2 minutos: inhala 4s, sostén 7s, exhala 8s.",
            "Sal a caminar 10 minutos sin pantallas y nota 3 sonidos.",
            "Escribe 3 cosas por las que agradeces hoy.",
            "Toma un vaso de agua pausado, a sorbos conscientes.",
            "Estira cuello y hombros 2 minutos entre actividades.",
            "Escucha un sonido de lluvia 5 minutos con los ojos cerrados.",
            "Ordena un espacio pequeño: el orden externo calma la mente.",
            "Escribe a alguien que aprecies, solo para saludar.",
            "Duerme hoy 30 minutos más temprano que ayer.",
            "Haz 5 minutos de meditación con un sonido de esta app.",
            "Anota qué emoción domina tu día y qué la disparó.",
            "Come una fruta despacio, notando sabor y textura.",
            "Reduce 15 minutos tu red social más usada hoy.",
            "Repite en voz alta: estoy avanzando un día a la vez."
        };

        /// <summary>
        /// Sugerencia diaria rotativa + pendientes de hoy (ánimo/hábitos).
        /// </summary>
        public async Task<MAVE.DTO.DailySuggestionDTO> DailySuggestion(int? id)
        {
            int uid = id ?? 0;
            bool pendingMood = true;
            bool pendingHabits = true;
            if (uid > 0)
            {
                pendingMood = !await _repo.HasMoodToday(uid);
                pendingHabits = !await _repo.HasHabitsToday(uid);
            }
            string tip = _tips[DateTime.Now.DayOfYear % _tips.Length];
            return new MAVE.DTO.DailySuggestionDTO
            {
                Suggestion = tip,
                PendingMood = pendingMood,
                PendingHabits = pendingHabits
            };
        }
    }
}