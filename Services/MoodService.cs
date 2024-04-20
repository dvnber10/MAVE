using MAVE.DTO;
using MAVE.Repositories;

namespace MAVE.Services
{
    public class MoodService
    {
        private readonly MoodRepository _repo;

        public MoodService(MoodRepository repo)
        {
            _repo = repo;
        }

        public async Task<int> SetMood(MoodDTO mood, int? id)
        {
            if (mood == null)
            {
                return 0;
            }
            else
            {
                if(await _repo.SetMood(mood, id) == 1)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }

        public async Task<int> GetMood(int? id)
        {
            try
            {
                if(await _repo.GetMood(id) == 1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch(Exception)
            {
                return 2;
            }   
        }

        public async Task<MoodGraphicDTO?> GetMoodGraphic(int? id)
        {
            try
            {
                MoodGraphicDTO? mood = await _repo.GetMoodGraphic(id);
                return mood;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}