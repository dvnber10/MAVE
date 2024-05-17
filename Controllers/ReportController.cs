using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MAVE.Services;
using MAVE.DTO;

namespace MAVE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        private readonly ReportService _serv;
        public ReportController(ReportService serv)
        {
            _serv = serv;
        }

        [HttpGet]
        [Authorize]
        [Route("GetInitialReport")]
        public async Task<IActionResult> GetInitialReport(int? id)
        {
            try
            {
                InitialReportDTO? r = new InitialReportDTO();
                r = await _serv.GetInitialReport(id);
                if (r == null) return NotFound("No se encontró información");
                else return Ok(r);
            }
            catch (Exception ex)
            {
                return BadRequest("Algo salió mal: "+ex.Message);
            }
        }
    }
}