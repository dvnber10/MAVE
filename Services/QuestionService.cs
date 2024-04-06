using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
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
        [Authorize (Roles="1")]
        public async Task<List<CatQuestion>?> GetInitialQuestion(int? id){
            var user = await _Urepo.GetUserByID(id);
            if (user.EvaluationId!=1)
            {
                return null;
            }else{
                return await _repo.GetInitialQuestion();
            }
        }
    }
}