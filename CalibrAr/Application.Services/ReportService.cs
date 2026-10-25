using Domain.Model;
using DTOs.Reports;

namespace Application.Services
{
    public class ReportService : IReportService
    {
        private static readonly int[] AllowedDays = [30, 60, 90];

        private readonly IInstrumentService instrumentService;
        private readonly ICalibrationService calibrationService;

        public ReportService(IInstrumentService instrumentService, ICalibrationService calibrationService)
        {
            this.instrumentService = instrumentService;
            this.calibrationService = calibrationService;
        }

        public async Task<UpcomingCalibrationsReportDTO> GetUpcomingCalibrationsAsync(int days)
        {
            if (!AllowedDays.Contains(days))
                throw new ArgumentException("El período debe ser de 30, 60 o 90 días.");

            var today = DateTime.Now.Date;
            var limit = today.AddDays(days);
            var instruments = await instrumentService.GetAllAsync();

            // Incluye las ya vencidas, no solo las del período
            var rows = instruments
                .Where(i => i.IsActive
                    && i.Status != InstrumentStatus.Decommissioned.ToString()
                    && i.NextCalibrationDate.HasValue
                    && i.NextCalibrationDate.Value.Date <= limit)
                .OrderBy(i => i.NextCalibrationDate)
                .Select(i => new UpcomingCalibrationRowDTO
                {
                    InstrumentCode = i.Code,
                    InstrumentName = i.Name,
                    AreaName = i.AreaName,
                    LastCalibrationDate = i.LastCalibrationDate,
                    NextCalibrationDate = i.NextCalibrationDate!.Value,
                    DaysRemaining = (i.NextCalibrationDate!.Value.Date - today).Days
                })
                .ToList();

            return new UpcomingCalibrationsReportDTO
            {
                GeneratedAt = DateTime.Now,
                Days = days,
                OverdueCount = rows.Count(r => r.DaysRemaining < 0),
                UpcomingCount = rows.Count(r => r.DaysRemaining >= 0),
                Rows = rows
            };
        }

        public async Task<CalibrationResultsReportDTO> GetCalibrationResultsAsync(DateTime from, DateTime to)
        {
            if (from.Date > to.Date)
                throw new ArgumentException("La fecha desde no puede ser posterior a la fecha hasta.");

            var calibrations = (await calibrationService.GetAllAsync())
                .Where(c => c.CalibrationDate.Date >= from.Date && c.CalibrationDate.Date <= to.Date)
                .OrderByDescending(c => c.CalibrationDate)
                .ToList();

            var totals = new List<ResultTotalDTO>
            {
                new() { Label = "Conforme", Count = calibrations.Count(c => c.Result == Result.Conformant.ToString()) },
                new() { Label = "Conforme con restricciones", Count = calibrations.Count(c => c.Result == Result.WithRestrictions.ToString()) },
                new() { Label = "No conforme", Count = calibrations.Count(c => c.Result == Result.NonConformant.ToString()) }
            };

            return new CalibrationResultsReportDTO
            {
                GeneratedAt = DateTime.Now,
                From = from.Date,
                To = to.Date,
                Total = calibrations.Count,
                Totals = totals,
                Rows = calibrations.Select(c => new CalibrationResultRowDTO
                {
                    CalibrationDate = c.CalibrationDate,
                    InstrumentCode = c.InstrumentCode ?? string.Empty,
                    Result = ResultLabel(c.Result),
                    IsExternal = c.IsExternal,
                    CertificateNumber = c.CertificateNumber
                }).ToList()
            };
        }

        private static string ResultLabel(string result) => result switch
        {
            nameof(Result.Conformant) => "Conforme",
            nameof(Result.WithRestrictions) => "Conforme con restricciones",
            nameof(Result.NonConformant) => "No conforme",
            _ => result
        };
    }
}
