using MAVE.DTO;
using MAVE.Repositories;
using MAVE.Utilities;
using MAVE.Models;
using dotenv.net;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

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
#pragma warning disable CS8604 // Possible null reference argument.
                var route = _uti.ImageUpload(art.Image);
                DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
                Cloudinary cloudinary = new(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
                cloudinary.Api.Secure = true;
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(@""+route.Result),
                    UseFilename = true,
                    UniqueFilename = false,
                    Overwrite = true
                };
                var uploadResult = cloudinary.Upload(uploadParams);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                string urlImage = Convert.ToString(uploadResult.SecureUrl);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                int res = await _repo.PostArticle(id, urlImage, art);
                File.Delete(route.Result);
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