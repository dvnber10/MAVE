using MAVE.DTO;
using MAVE.Repositories;
using MAVE.Utilities;
using MAVE.Models;

namespace MAVE.Services
{
    public class ArticleService
    {
        private readonly ArticleRepository _repo;
        private readonly ImageUtility _uti;

        public ArticleService(ArticleRepository repo, ImageUtility uti)
        {
            _repo = repo;
            _uti = uti;
        }
        public async Task<List<Article>?> GetArticles(int? id)
        {
            try
            {
                List<Article>? art = new List<Article>();
                art = await _repo.GetArticles(id);
                return art;
            }
            catch (Exception)
            {
                return null;
            }   
        }

        public async Task<int> PostArticle(int? id, IFormFile img, ArticleWhitImageDTO art)
        {
            try
            {
                string? imge = await _uti.ImageRoute(img);
                int res = await _repo.PostArticle(id, imge, art);
                if(res == 1) return 3;
                if(res == 2) return 4;
                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public async Task<int> PutArticle(int? id, IFormFile img, ArticleWhitImageDTO art)
        {
            try
            {
                string? imge = null;
                if(img != null)  imge = await _uti.ImageRoute(img);
                #nullable disable
                int res = await _repo.PutArticle(id, imge, art);
                #nullable enable
                if (res == 1) return 2;
                if (res == 2) return 3;
                if (res == 3) return 4;
                return 0;
            }
            catch(Exception)
            {
                return 1;
            }
        }

        public async Task<ArticleDTO?> GetArticle(int? id, string name)
        {
            try
            {
                ArticleDTO? art = await _repo.GetArticle(id, name);
                if(art == null) return null;
                else return art;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
} 