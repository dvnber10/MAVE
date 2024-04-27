using MAVE.DTO;
using MAVE.Models;
using Microsoft.EntityFrameworkCore;

namespace MAVE.Repositories
{
    public class MoodRepository
    {
        private readonly DbAa60a4MavetestContext _context;
        private readonly UserRepositories _Urepo;

        public MoodRepository(UserRepositories urepo, DbAa60a4MavetestContext context)
        {
            _Urepo = urepo;
            _context = context;
        }

        public async Task<int> SetMood(MoodDTO mood, int? id)
        {
            try
            {
                var user = await _context.Users.Where(u => u.UserId == id).FirstOrDefaultAsync();
                if (user != null)
                {
                    Mood md = new Mood
                    {
                        MoodScore = mood.Score,
                        Date = DateTime.Now,
                        UserId = user.UserId
                    };
                    _context.Moods.Add(md);
                    await _context.SaveChangesAsync();
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> GetMood(int? id)
        {
            try
            {
                DateTime day = DateTime.Now;
                bool res = true;
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return 1;
                }
                else
                {
                    var mood = await _context.Moods.Where(m => m.UserId == user.UserId).ToListAsync();
                    if (mood == null)
                    {
                        return 0;
                    }
                    foreach(var item in mood)
                    {
                        if (item.Date.Day == day.Day)
                        {
                            res = false;
                        }
                    }
                    if (res) return 0;
                    else return 1;
                }
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public async Task<MoodGraphicDTO?> GetMoodGraphic(int? id)
        {
            try
            {
                MoodGraphicDTO? mood = new MoodGraphicDTO();
                mood.Score1 = await _context.Moods.Where(m => m.MoodScore == 1 && m.UserId == id).CountAsync();
                mood.Score2 = await _context.Moods.Where(m => m.MoodScore == 2 && m.UserId == id).CountAsync();
                mood.Score3 = await _context.Moods.Where(m => m.MoodScore == 3 && m.UserId == id).CountAsync();
                mood.Score4 = await _context.Moods.Where(m => m.MoodScore == 4 && m.UserId == id).CountAsync();
                mood.Score5 = await _context.Moods.Where(m => m.MoodScore == 5 && m.UserId == id).CountAsync();
                return mood;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}