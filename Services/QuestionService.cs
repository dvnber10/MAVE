using MAVE.DTO;
using MAVE.Models;
using MAVE.Repositories;
using MAVE.Utilities;

namespace MAVE.Services
{
    public class QuestionService
    {
        private readonly QuestionRepository _repo;
        private readonly UserRepositories _Urepo;
        private readonly EvaluationUtility _eva;
        private readonly UserService _Userv;
        public QuestionService(QuestionRepository repository,UserRepositories userRepositories, EvaluationUtility eva, UserService user){
            _repo = repository;
            _Urepo = userRepositories;
            _eva = eva;
            _Userv = user;
        }
        public async Task<List<CatQuestion>?> GetHabitQuestion(int? id){
            return await _repo.GetHabitQuestion();
        }
        public async Task<int> SetHabitQuestion(int? id, HabitDTO habit)
        {
            try
            {
                if(await _repo.SetHabitQuestion(id,habit) == 0)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch(Exception)
            {
                return 2;
            }
        }
        public async Task<List<CatQuestion>?> GetInitialQuestion(int id){
            try{
                var user = await _Urepo.GetUserByID(id);
                if (user.EvaluationId == 1)
                {
                    return await _repo.GetInitialQuestion();
                }
                else
                {
                    return null;
                }

            }catch(Exception){

                return null;
            }
        }

        public async Task<int> SetIntialQuestion(List<char> answer, int? Id)
        {
            try
            {
                _eva.SetAnswers(answer);
                short score = _eva.Score();
                if(score != 0)
                {
                    if (await _repo.SetInitialQuestion(answer, score, Id) == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 2;
                }
            }
            catch (Exception)
            {
                return 2;
            }
        }

        public async Task<InitialGraphicDTO?> GetInitialGraphic(int? id)
        {
            try
            {
                InitialGraphicDTO? iniVals = new InitialGraphicDTO();
                iniVals = await _repo.GetInitialGraphic(id);
                return iniVals;
            }
            catch(Exception)
            {
                return null;
            }
        }
        public async Task<string> GetPositiveReinforcement(int? id)
        {
            try
            {
                Random r = new Random();
                string[] rein = ["¡Excelente trabajo! Veo que has puesto mucho esfuerzo en esta tarea.",
                "Me siento muy orgulloso/a de ti por la manera en que has manejado este desafío.",
                "Buen trabajo al mantenerte concentrado/a. Esa dedicación dará sus frutos.",
                "Estoy impresionado/a por tu persistencia en resolver ese problema complicado.",
                "Me alegra ver que has aprendido de tus errores anteriores. Estás en el camino correcto."];
                var user = await _Userv.GetUserById(id);
                double res = await _repo.GetPositiveReinforcement(id);
                if(res == 0)
                {
                    return "2";
                }
                else if(res > 3)
                {
                    int index = r.Next(0,3);
                    return rein[index];
                }
                else
                {
                    int index = r.Next(3, 5);
                    return rein[4];
                }
            }
            catch (Exception)
            {
                return "1";
            }
        }
    }
}