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
        [Route("GetInitialReport/{id}")]
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

        /// <summary>
        /// Tiempo en actividades: meditación (AUDITORY) + conteos de ánimo y hábitos.
        /// GET /api/report/activity-time/{id}
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("activity-time/{id}")]
        public async Task<IActionResult> ActivityTime(int? id)
        {
            try
            {
                ActivityTimeDTO r = await _serv.GetActivityTime(id);
                return Ok(r);
            }
            catch (Exception ex)
            {
                return BadRequest("Algo salió mal: "+ex.Message);
            }
        }

        /// <summary>
        /// Reporte clínico general para psicólogo: paciente, actividad por tipo,
        /// PHQ-4 (último + historial + banda) y adherencia 30 días.
        /// GET /api/report/clinical/{id}
        /// </summary>
        /// <summary>KPIs de uso para el administrador.</summary>
        [HttpGet]
        [Authorize]
        [Route("kpis")]
        public async Task<IActionResult> Kpis()
        {
            try { return Ok(await _serv.GetKpis()); }
            catch (Exception ex) { return BadRequest("Algo salió mal: " + ex.Message); }
        }

        /// <summary>Última actividad (auditoría) para monitoreo del admin.</summary>
        [HttpGet]
        [Authorize]
        [Route("activity-log")]
        public async Task<IActionResult> ActivityLog([FromQuery] int take = 50)
        {
            try { return Ok(await _serv.GetActivityLog(take)); }
            catch (Exception ex) { return BadRequest("Algo salió mal: " + ex.Message); }
        }

        [HttpGet]
        [Authorize]
        [Route("clinical/{id}")]
        public async Task<IActionResult> Clinical(int? id)
        {
            try
            {
                ClinicalReportDTO? r = await _serv.GetClinicalReport(id);
                if (r == null) return NotFound("No se encontró información del paciente");
                return Ok(r);
            }
            catch (Exception ex)
            {
                return BadRequest("Algo salió mal: " + ex.Message);
            }
        }
    }
}