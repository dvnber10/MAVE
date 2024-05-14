using MAVE.DTO;
using MAVE.Repositories;

namespace MAVE.Services
{
    public class ReportService
    {
        private readonly ReportRepository _repo;
        public ReportService(ReportRepository repo)
        {
            _repo = repo;
        }
        public async Task<InitialReportDTO?> GetInitialReport(int? id)
        {
            try
            {
                InitialReportDTO? ir = new InitialReportDTO();
                ir = await _repo.GetInitialReport(id);
                if (ir == null) return null;
                else return ir;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}