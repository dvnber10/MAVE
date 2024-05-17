using MAVE.DTO;
using MAVE.Models;
using MAVE.Services;
using Microsoft.EntityFrameworkCore;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;

namespace MAVE.Repositories
{
    public class ArticleRepository
    {

        private readonly DbAa60a4MavetestContext _context;
        private readonly UserService _Userv;
        //public static readonly Account account = new Account(
        //    "dvhg4fegu",
        //    "142365139439512",
        //    "UpOh_jG7d6rMpt1__kJD0x-O0io");


        public ArticleRepository(DbAa60a4MavetestContext context, UserService userv)
        {
            _context = context;
            _Userv = userv;
        }

        public async Task<List<Article>?> GetArticles(int? id)
        {
            try
            {
                var user = await _Userv.GetUserById(id);
                if (user == null)
                {
                    return null;
                }
                //Role 2 => Admin, Role 3 => Psycho, Role 4 => User
                if (user.RoleId == 3)
                {
                    var article = await _context.Articles.Where(a => a.UserId == id).ToListAsync();
                    return article;
                }
                else
                {
                    var article = await _context.Articles.ToListAsync();
                    return article;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<int> PostArticle(int? id, string image, ArticleWhitImageDTO art)
        {
            try
            {
                var user = await _context.Users.Where(u => u.UserId == id).FirstOrDefaultAsync();
                if (user == null) return 2;
                if (user.RoleId != 3) return 3;
                Console.WriteLine(art.Image);
                DateTime date = new DateTime(art.Year, art.Month, art.Day);
                if (art.ArticleName == null || art.Link == null || art.Resume == null || id == null) return 2;
                Article arti = new Article
                {
                    UserId = (int)id,
                    ArticleName = art.ArticleName,
                    Link = art.Link,
                    Picture = image,
                    Resume = art.Resume,
                    Date = DateOnly.FromDateTime(date),
                    TypeId = art.Type
                };
                await _context.Articles.AddAsync(arti);
                await _context.SaveChangesAsync();

                return 0;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public async Task<int> PutArticle(int? id, string img, ArticleWhitImageDTO art)
        {
            try
            {
                var user = await _context.Users.Where(u => u.UserId == id).FirstOrDefaultAsync();
                if (user == null) return 1;
                if (user.RoleId != 3) return 3;
                DateTime date = new DateTime(art.Year, art.Month, art.Day);
                if (art.ArticleName == null || art.Link == null || art.Resume == null || id == null) return 2;
                var article = await _context.Articles.Where(a => a.ArticleName == art.ArticleName
                && a.UserId == id).FirstOrDefaultAsync();
                if (article == null) return 1;
                if (img != null)
                {
                    article.UserId = (int)id;
                    article.ArticleName = art.ArticleName;
                    article.Link = art.Link;
                    article.Picture = img;
                    article.Resume = art.Resume;
                    article.Date = DateOnly.FromDateTime(date);
                    article.TypeId = art.Type;
                    _context.Articles.Update(article);
                    await _context.SaveChangesAsync();
                    return 0;
                }
                else
                {
                    article.UserId = (int)id;
                    article.ArticleName = art.ArticleName;
                    article.Link = art.Link;
                    article.Resume = art.Resume;
                    article.Date = DateOnly.FromDateTime(date);
                    article.TypeId = art.Type;
                    _context.Articles.Update(article);
                    await _context.SaveChangesAsync();
                    return 0;
                }
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public async Task<ArticleDTO?> GetArticle(int? id, string name)
        {
            try
            {
                ArticleDTO art = new ArticleDTO();
                var arti = await _context.Articles.Where(a => a.ArticleName == name && a.UserId == id).FirstOrDefaultAsync();
                User user = await _Userv.GetUserById(id);
                if (arti != null)
                {
                    var type = await _context.CatArticleTypes.Where(cat => cat.TypeId == arti.TypeId).FirstOrDefaultAsync();
                    art.ArticleName = arti.ArticleName;
                    art.HealthProf = user.UserName;
                    art.Resume = arti.Resume;
                    art.Picture = arti.Picture;
                    art.Link = arti.Link;
                    if (type != null) art.Type = type.ArticleType;
                    art.Date = arti.Date;
                    return art;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}