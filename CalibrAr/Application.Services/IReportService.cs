using DTOs.Reports;

namespace Application.Services
{
    public interface IReportService
    {
        Task<UpcomingCalibrationsReportDTO> GetUpcomingCalibrationsAsync(int days);
        Task<CalibrationResultsReportDTO> GetCalibrationResultsAsync(DateTime from, DateTime to);
    }
}
