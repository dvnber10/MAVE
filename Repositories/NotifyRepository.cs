using MAVE.Models;
using Microsoft.EntityFrameworkCore;

namespace MAVE.Repositories
{     
    public class NotifyRepository
    {
        private readonly DbAa60a4MavetestContext _context;
        public NotifyRepository(DbAa60a4MavetestContext context)
        {
            _context = context;
        }
        public async Task<List<User>?> MoodReminder()
        {
            try
            {
                DateTime now = DateTime.Now;
                List<User> users = new List<User>();
                var mood = await _context.Moods.Where(m => m.Date.Day != now.Day).ToListAsync();
                foreach(var item in mood)
                {
                    var user = await _context.Users.Where(u => u.UserId == item.UserId).FirstOrDefaultAsync();
                    if(user != null) if(!users.Contains(user)) users.Add(user);
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> HasMoodToday(int id)
        {
            var today = DateTime.Now.Date;
            if (await _context.Moods.AnyAsync(m => m.UserId == id && m.Date.Date == today)) return true;
            return await _context.Auditories.AnyAsync(a => a.UserId == id && a.Action == "MOOD_PHQ4" && a.Date.Date == today);
        }

        public async Task<bool> HasHabitsToday(int id)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            return await _context.Questions.AnyAsync(q => q.UserId == id && q.ScoreId != null && q.Date == today);
        }

        public async Task<List<User>?> HabitReminder()
        {
            try
            {
                DateTime now = DateTime.Now;
                List<User> users = new List<User>();
                var mood = await _context.Questions.Where(q => q.Date.Day != now.Day && q.ScoreId != null).ToListAsync();
                foreach(var item in mood)
                {
                    var user = await _context.Users.Where(u => u.UserId == item.UserId).FirstOrDefaultAsync();
                    if(user != null) if(!users.Contains(user)) users.Add(user);
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}