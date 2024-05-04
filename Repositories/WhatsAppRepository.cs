using MAVE.Models;
using Microsoft.EntityFrameworkCore;

namespace MAVE.Repositories
{     
    public class WhatsAppRepository
    {
        private readonly DbAa60a4MavetestContext _context;
        public WhatsAppRepository(DbAa60a4MavetestContext context)
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