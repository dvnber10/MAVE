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
                short queId = 14;
                String a;
                var user = await _context.Users.FindAsync(Id);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                user.EvaluationId = result;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                _context.Update(user);
                _context.SaveChanges();
                foreach (char c in answers)
                {
                    a = c + "";
                    var option = _context.CatOptions.Where(e => e.CatQuestionId == queId && e.Abcd == a).FirstOrDefault();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    Question question = new Question
                    {
                        CatQuestionId = queId,
                        OptionId = option.OptionId,
                        Date = DateOnly.FromDateTime(date),
                        UserId = Convert.ToInt32(Id)
                    };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    _context.Update(question);
                    _context.SaveChanges();
                    queId++;
                }
                return 0;
            }catch(System.Exception )
            {
                return 1;
            }
        }
    }
}