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
                var user = await _context.Users.FindAsync(id);
                if(user != null){
                    Mood md = new Mood
                    {
                        MoodScore = mood.Score,
                        Date = DateTime.Now,
                        UserId = user.UserId
                    };
                    _context.Moods.Add(md);
                    _context.SaveChanges();
                    return 1;
                }
                else
                {
                    return 0;
                }
            }catch (Exception)
            {
                return 0;
            }
        }
        
        public async Task<int> GetMood(int? id){
            try
            {
                DateTime day = DateTime.Now;
                var user =await _context.Users.FindAsync(id);
                if(user == null)
                {
                    return 1;
                }
                else 
                {
                    var mood = _context.Moods.Where(m => m.UserId == user.UserId).FirstOrDefault();
                    if(mood == null)
                    {
                        return 1;
                    }
                    else 
                    {
                        if(mood.Date.Day == day.Day)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            catch(Exception)
            {
                return 1;
            }
        }

        public async Task<MoodGraphicDTO?> GetScores(int? id)
        {
            try
            {
                MoodGraphicDTO mood = new MoodGraphicDTO(); 
                var score1 = await _context.Moods.Where(m => m.MoodScore == 1 && m.UserId == id).CountAsync(); 
                var score2 = await _context.Moods.Where(m => m.MoodScore == 2 && m.UserId == id).CountAsync();
                var score3 = await _context.Moods.Where(m => m.MoodScore == 3 && m.UserId == id).CountAsync();
                var score4 = await _context.Moods.Where(m => m.MoodScore == 4 && m.UserId == id).CountAsync();
                var score5 = await _context.Moods.Where(m => m.MoodScore == 5 && m.UserId == id).CountAsync();
                mood.Score1 = score1;
                mood.Score2 = score2;
                mood.Score3 = score3;
                mood.Score4 = score4;
                mood.Score5 = score5;
                return mood;
            }
            catch (Exception)
            {
                return null;
            }            
        }
    }
}