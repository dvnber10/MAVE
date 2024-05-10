using Microsoft.AspNetCore.Mvc;
using MAVE.Services;
using Microsoft.AspNetCore.Authorization;
using MAVE.DTO;
using MAVE.Models;

namespace MAVE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ArticleService _serv;

        public ArticleController (ArticleService serv)
        {
            _serv = serv;
        }

        [HttpGet]
        [Authorize]
        [Route("GetArticles/{id}")]
        public async Task<IActionResult> GetArticles(int? id)
        {
            try
            {
                List<Article>? art = new List<Article>();
                art = await _serv.GetArticles(id);
                if(art == null) return NotFound("No se encontraron artículos");
                return Ok(art);
            }
            catch (Exception ex)
            {
                return BadRequest("Algo salió mal: "+ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("PostArticle/{id}")]
        public async Task<IActionResult> PostArticle(int? id, [FromBody] ArticleWhitImageDTO art)
        {
            try
            {
                if(art.Image == null) return BadRequest("No se envió imagen");
                int res = await _serv.PostArticle(id, art.Image, art);
                if(res == 0) return Ok("Se han guardado los datos");
                else if(res == 1) return BadRequest("Algo salió mal con el servicio");
                else if(res == 2) return BadRequest("No hay imagen");
                else if(res == 3) return BadRequest("Algo salió mal con el repositorio");
                else return BadRequest("Faltan datos");
            }
            catch (Exception ex)
            {
                return BadRequest("Algo salió mal: "+ex.Message);
            }
        }

        [HttpPut]
        [Authorize]
        [Route("PutArticle/{id}")]
        public async Task<IActionResult> PutArticle(int? id, [FromBody] ArticleWhitImageDTO art)
        {
            try
            {
                #nullable disable
                int res = await _serv.PutArticle(id, art.Image, art);
                #nullable enable
                if(res == 0) return Ok("Se ha acatualizado articulo");
                if(res == 1) return BadRequest("Algo salió mal con el servicio");
                if(res == 2) return BadRequest("Algo salió mal en el repositorio");
                if(res == 3) return BadRequest("Falta Información para guardar");
                else return BadRequest("No tienes los permisos necesarios");
            }
            catch (Exception ex)
            {
                return BadRequest("Algo salió mal: "+ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("GetArticle/{id}")]
        public async Task<IActionResult> GetArticle(int? id, ArticleNameDTO name)
        {
            try
            {
                if(name.Name == null) return BadRequest("No se ha enviado ningún nombre para buscar");
                ArticleDTO? art = await _serv.GetArticle(id, name.Name);
                if(art == null) return NotFound("No se encontraron datos");
                else return Ok(art);
            }
            catch (Exception ex)
            {
                return BadRequest("Algo salió mal: "+ex);
            }
        }
    }
}