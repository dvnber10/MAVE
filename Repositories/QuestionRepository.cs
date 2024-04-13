using MAVE.DTO;
using MAVE.Models;
using Microsoft.EntityFrameworkCore;

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
                var catQuestions = await _context.CatQuestions.Where(e => e.Initial == false).ToArrayAsync();
                if(habit.Score == null || id == null) return 1;
                int i = 0;
                DateTime date = DateTime.Now;
                foreach (var h in habit.Score)
                {
                    Question question = new Question
                    {
                        ScoreId = h,
                        Date = DateOnly.FromDateTime(date),
                        QuestionId = catQuestions[i].CatQuestionId,
                        UserId = (int)id
                    };
                    _context.Update(question);
                    _context.SaveChanges();
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
                String a;
                var user = await _context.Users.FindAsync(Id);
                if (user == null)
                {
                    return 1;
                }
                else 
                {
                    user.EvaluationId = result;
                }
                _context.Update(user);
                _context.SaveChanges();
                var catQ = await _context.CatQuestions.Where(e => e.Initial == true).ToArrayAsync();
                int queId = 0;
                foreach (char c in answers)
                {
                    a = c + "";

                    var option = _context.CatOptions.Where(e => e.CatQuestionId == queId
                    && e.Abcd == a).FirstOrDefault();
                    if(option == null || Id == null) return 1;

                    Question question = new Question
                    {
                        CatQuestionId = catQ[queId].CatQuestionId,
                        OptionId = option.OptionId,
                        Date = DateOnly.FromDateTime(date),
                        UserId = Convert.ToInt32(Id)
                    };
                    _context.Update(question);
                    _context.SaveChanges();
                    queId++;
                }
                return 0;

            }
            catch(Exception)
            {
                return 1;
            }
        }
    }
}