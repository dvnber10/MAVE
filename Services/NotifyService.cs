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
    }
}