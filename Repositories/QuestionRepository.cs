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

        public async Task<int> SetInitialQuestion(List<char> answers, short result, int? Id)
        {
            try
            {
                DateTime date = DateTime.Now;
                short queId = 14;
                String a;
                var user = await _context.Users.FindAsync(Id);
                user.EvaluationId = result;
                _context.Update(user);
                _context.SaveChanges();
                foreach (char c in answers)
                {
                    a = c + "";
                    var option = _context.CatOptions.Where(e => e.CatQuestionId == queId
                    && e.Abcd == a).FirstOrDefault();
                    
                    Question question = new Question
                    {
                        CatQuestionId = queId,
                        OptionId = option.OptionId,
                        Date = DateOnly.FromDateTime(date),
                        UserId = (int)Id
                    };
                    _context.Update(question);
                    _context.SaveChanges();
                    queId++;
                }
                return 0;
            }catch(Exception ex)
            {
                return 1;
            }
        }
    }
}