using MAVE.DTO;
using MAVE.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Engines;

namespace MAVE.Repositories
{
    public class QuestionRepository
    {
        private readonly DbAa60a4MavetestContext _context;
        public QuestionRepository(DbAa60a4MavetestContext context){
            _context = context;
        }
        public async Task<List<CatQuestion>>GetHabitQuestion(){
            bool ini = false;

            var questions = await _context.CatQuestions.Where(e => e.Initial == ini).ToListAsync();
            return questions;
        }
        public async Task<int> SetHabitQuestion(int? id, HabitDTO habit)
        {
            try
            {
                var catQ = await _context.CatQuestions.Where(e => e.Initial == false).ToArrayAsync();
                if(habit.Score == null || id == null) return 1;
                short i = 0;
                DateTime date = DateTime.Now;
                foreach (var h in habit.Score)
                {
                    Question question = new()
                    {
                        ScoreId = h,
                        Date = DateOnly.FromDateTime(date),
                        CatQuestionId = catQ[i].CatQuestionId,
                        UserId = (int)id
                    };
                    _context.Questions.Update(question);
                    await _context.SaveChangesAsync();
                    i++;
                }
                return 0;
            }
            catch(Exception)
            {
                return 1;
            }            
        }
        public async Task<List<CatQuestion>>GetInitialQuestion(){
            bool ini = true;
            var questions = await _context.CatQuestions.Where(e => e.Initial == ini).ToListAsync();
            return questions;
        }

        public async Task<int> SetInitialQuestion(List<char> answers, short result, int? Id)
        {
            try
            {
                DateTime date = DateTime.Now;
                string a;
                var user = await _context.Users.Where(u => u.UserId == Id).FirstOrDefaultAsync();
                if (user == null)
                {
                    return 1;
                }
                else 
                {
                    result++;
                    user.EvaluationId = result;
                }
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                var catQ = await _context.CatQuestions.Where(e => e.Initial == true).ToListAsync();
                short queId = 14;
                foreach (var c in answers)
                {
                    a = c.ToString();
                    var option = await _context.CatOptions.Where(co => co.CatQuestionId == queId
                    && co.Abcd == a).FirstOrDefaultAsync();

                    Question question = new Question
                    {
                        CatQuestionId = queId,
                        #nullable disable
                        OptionId = option.OptionId,
                        #nullable enable
                        Date = DateOnly.FromDateTime(date),
                        UserId = user.UserId
                    };
                    _context.Questions.Update(question);
                    await _context.SaveChangesAsync();
                    queId++;
                }
                return 0;

            }
            catch(Exception)
            {
                return 1;
            }
        }

        public async Task<InitialGraphicDTO?> GetInitialGraphic(int? id)
        {
            try
            {
                InitialGraphicDTO iniVals = new InitialGraphicDTO();
                int d = 0, i = 0, s = 0, c = 0;
                var question = await _context.Questions.Where(q => q.UserId == id && q.OptionId != null).ToListAsync();
                var option = question.Join(
                _context.CatOptions, q => q.OptionId, co => co.OptionId, (q, co) => new
                {
                    Value = co.Value                        
                }).ToList(); 
                if (option == null) return null;
                foreach (var item in option)
                {
                    if (item.Value.Equals("D")) d++;
                    if (item.Value.Equals("I")) i++;
                    if (item.Value.Equals("S")) s++;
                    if (item.Value.Equals("C")) c++;                    
                }
                iniVals.D = d;
                iniVals.I = i;
                iniVals.S = s;
                iniVals.C = c; 
                return iniVals;
            }
            catch(Exception)
            {
                return null;
            }
        }
        public async Task<double> GetPositiveReinforcement(int? id)
        {
            try
            {
                int points = 0, can = 0;
                var one = await _context.Questions.Where(q => q.UserId == id && q.ScoreId !=null).ToListAsync();
                foreach (var q in one)
                {
                    can++;
                    if(q.ScoreId == 1 || q.ScoreId == 7) points += (int)q.ScoreId*1;
                    if(q.ScoreId == 2) points += (int)q.ScoreId*2;
                    if(q.ScoreId == 3) points += (int)q.ScoreId*3;
                    if(q.ScoreId == 4) points += (int)q.ScoreId*4;
                    if(q.ScoreId == 5 || q.ScoreId == 6) points += (int)q.ScoreId*5;
                }
                double fin = (double)points/(double)can;
                return fin;
            }   
            catch(Exception)
            {
                return 0;
            }         
        }
    }
}