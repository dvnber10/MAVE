using System.ComponentModel;
using MAVE.Models;
using MAVE.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace MAVE.Repositories
{
    public class QuestionRepository
    {
        private readonly DbAa60a4MavetestContext _context;
        public QuestionRepository(DbAa60a4MavetestContext context){
            _context = context;
        }
        public async Task<List<CatQuestion>>GetInitialQuestion(){
            bool ini = true;
            var questions = await _context.CatQuestions.Where(e => e.Initial == ini).ToListAsync();
            return questions;
        }

        public async Task<int> SetInitialQuestion(List<char> answers, short result, int? Id)
        {
            short queId = 14;
            var user = await _context.Users.FindAsync(Id);
            user.EvaluationId = result;
            _context.Update(user);
            foreach(char c in answers)
            {
                QuestionModel question = new QuestionModel
                {
                    CatQuestionId = queId,
                    OptionId = c,
                    Date = DateTime.Now,
                    UserId = Id
                };
                _context.Update(question);
                queId++;
            }
            return 0;
        }
    }
}