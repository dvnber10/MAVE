
using MAVE.DTO;
using MAVE.Models;
using Microsoft.EntityFrameworkCore;

namespace MAVE.Repositories
{
    public class ReportRepository
    {
        private readonly DbAa60a4MavetestContext _context;
        public ReportRepository(DbAa60a4MavetestContext context)
        {
            _context = context;
        }
        
        public async Task<List<string>?> GetQuestions()
        {
            try 
            {
                List<string> r = new List<string>();
                var questions = await _context.CatQuestions.Where(cq => cq.Initial == true).ToListAsync();
                foreach(var question in questions)
                {
                    r.Add(question.Question);
                }
                return r;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<InitialReportDTO?> GetInitialReport(int? id)
        {
            try 
            {
                InitialReportDTO r = new InitialReportDTO();
                List<string> ans = new List<string>();
                List<string> sco = new List<string>();
                var questions = await _context.Questions.Where(q => q.UserId == id && q.OptionId != null).ToListAsync();
                var option = questions.Join(_context.CatOptions, q => q.OptionId, co => co.OptionId, (q, co) => new
                {
                    Answer = co.EvaOption,
                    Score = co.Value                        
                }).ToList(); 
                foreach (var o in option)
                {
                    ans.Add(o.Answer);
                    sco.Add(o.Score);
                }
                r.answer = ans;
                r.score = sco;
                r.question = await GetQuestions();
                return r;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}