using Application.Services;
using DTOs.Reports;

namespace WebAPI
{
    public static class ReportEndpoints
    {
        public static void MapReportEndpoints(this WebApplication app)
        {
            app.MapGet("/reports/upcoming-calibrations", async (int? days, IReportService reportService) =>
            {
                try
                {
                    UpcomingCalibrationsReportDTO report = await reportService.GetUpcomingCalibrationsAsync(days ?? 30);

                    return Results.Ok(report);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetUpcomingCalibrationsReport")
            .Produces<UpcomingCalibrationsReportDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("ReportsRead");

            app.MapGet("/reports/upcoming-calibrations/pdf", async (int? days, IReportService reportService, ReportPdfGenerator pdfGenerator) =>
            {
                try
                {
                    UpcomingCalibrationsReportDTO report = await reportService.GetUpcomingCalibrationsAsync(days ?? 30);
                    byte[] pdf = pdfGenerator.GenerateUpcomingCalibrations(report);

                    return Results.File(pdf, "application/pdf", $"calibraciones-proximas-{DateTime.Now:yyyyMMdd}.pdf");
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetUpcomingCalibrationsReportPdf")
            .Produces(StatusCodes.Status200OK, contentType: "application/pdf")
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("ReportsRead");

            app.MapGet("/reports/calibration-results", async (DateTime? from, DateTime? to, IReportService reportService) =>
            {
                try
                {
                    var (start, end) = ResolvePeriod(from, to);
                    CalibrationResultsReportDTO report = await reportService.GetCalibrationResultsAsync(start, end);

                    return Results.Ok(report);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetCalibrationResultsReport")
            .Produces<CalibrationResultsReportDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("ReportsRead");

            app.MapGet("/reports/calibration-results/pdf", async (DateTime? from, DateTime? to, IReportService reportService, ReportPdfGenerator pdfGenerator) =>
            {
                try
                {
                    var (start, end) = ResolvePeriod(from, to);
                    CalibrationResultsReportDTO report = await reportService.GetCalibrationResultsAsync(start, end);
                    byte[] pdf = pdfGenerator.GenerateCalibrationResults(report);

                    return Results.File(pdf, "application/pdf", $"resultados-calibraciones-{DateTime.Now:yyyyMMdd}.pdf");
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("GetCalibrationResultsReportPdf")
            .Produces(StatusCodes.Status200OK, contentType: "application/pdf")
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("ReportsRead");
        }

        // Sin fechas, el período por defecto son los últimos 12 meses
        private static (DateTime From, DateTime To) ResolvePeriod(DateTime? from, DateTime? to)
        {
            DateTime end = to ?? DateTime.Now;
            DateTime start = from ?? end.AddMonths(-12);

            return (start, end);
        }
    }
}
