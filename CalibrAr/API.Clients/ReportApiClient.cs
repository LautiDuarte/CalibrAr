using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DTOs.Reports;

namespace API.Clients
{
    public class ReportApiClient : BaseApiClient
    {
        public ReportApiClient(ITokenProvider tokenProvider) : base(tokenProvider)
        {
        }

        public async Task<UpcomingCalibrationsReportDTO> GetUpcomingCalibrationsAsync(int days)
        {
            var response = await GetAsync($"reports/upcoming-calibrations?days={days}");
            var report = await response.Content.ReadFromJsonAsync<UpcomingCalibrationsReportDTO>();
            return report ?? throw new Exception("La respuesta del servidor para el reporte de calibraciones próximas vino vacia.");
        }

        public async Task<byte[]> GetUpcomingCalibrationsPdfAsync(int days)
        {
            var response = await GetAsync($"reports/upcoming-calibrations/pdf?days={days}");
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<CalibrationResultsReportDTO> GetCalibrationResultsAsync(DateTime from, DateTime to)
        {
            var response = await GetAsync($"reports/calibration-results?{PeriodQuery(from, to)}");
            var report = await response.Content.ReadFromJsonAsync<CalibrationResultsReportDTO>();
            return report ?? throw new Exception("La respuesta del servidor para el reporte de resultados vino vacia.");
        }

        public async Task<byte[]> GetCalibrationResultsPdfAsync(DateTime from, DateTime to)
        {
            var response = await GetAsync($"reports/calibration-results/pdf?{PeriodQuery(from, to)}");
            return await response.Content.ReadAsByteArrayAsync();
        }

        private static string PeriodQuery(DateTime from, DateTime to) =>
            $"from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}";

        private async Task<HttpResponseMessage> GetAsync(string url)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to get report. Status code: {response.StatusCode}, Error: {errorContent}");
                }
                return response;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while getting report: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while getting report: {ex.Message}.", ex);
            }
        }
    }
}
