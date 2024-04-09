using MAVE.Models;
using MAVE.Repositories;

namespace MAVE.Services
{
    public class QuestionService
    {
        private readonly QuestionRepository _repo;
        private readonly UserRepositories _Urepo;
        public QuestionService(QuestionRepository repository,UserRepositories userRepositories){
            _repo = repository;
            _Urepo = userRepositories;
        }
        public async Task<List<CatQuestion>?> GetHabitQuestion(int? id){
            return await _repo.GetHabitQuestion();
        }
    }
}