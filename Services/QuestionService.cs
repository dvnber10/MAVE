using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using MAVE.Models;
using MAVE.Repositories;
using MAVE.Utilities;

namespace MAVE.Services
{
    public class QuestionService
    {
        private readonly QuestionRepository _repo;
        private readonly UserRepositories _Urepo;
        private readonly UserModel _user;
        private readonly EvaluationUtility _eva;
        public QuestionService(QuestionRepository repository,UserRepositories userRepositories, UserModel user, EvaluationUtility eva){
            _repo = repository;
            _Urepo = userRepositories;
            _user = user;
            _eva = eva;
        }
        [Authorize (Roles="1")]
        public async Task<List<CatQuestion>?> GetInitialQuestion(int id){
            var user = await _Urepo.GetUserByID(id);
            if (user.EvaluationId==1)
            {
                return null;
            }else{
                return await _repo.GetInitialQuestion();
            }
        }

        public async Task<int> SetIntialQuestion(List<char> answer, int? Id)
        {
            _eva.SetAnswers(answer);
            if(await _repo.SetInitialQuestion(answer, _eva.Score(), Id) == 1)
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