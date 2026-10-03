using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace API.Clients
{
    public class CalibrationMeasurementApiClient : BaseApiClient
    {
        public static async Task<CalibrationMeasurementDTO> GetAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync($"calibrationMeasurements/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var calibrationMeasurement = await response.Content.ReadFromJsonAsync<CalibrationMeasurementDTO>();
                    return calibrationMeasurement ?? throw new Exception($"La respuesta del servidor para la medicion de calibracion con ID {id} vino vacia.");
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to get calibration measurement with ID {id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while getting calibration measurement with ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while getting calibration measurement with ID {id}: {ex.Message}.", ex);
            }
        }

        public static async Task<List<CalibrationMeasurementDTO>> GetAllAsync()
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync("calibrationMeasurements");
                if (response.IsSuccessStatusCode)
                {
                    var calibrationMeasurements = await response.Content.ReadFromJsonAsync<List<CalibrationMeasurementDTO>>();
                    return calibrationMeasurements ?? throw new Exception("La respuesta del servidor para la lista de mediciones de calibracion vino vacia.");
                }
                else
                {
                    await HandleUnauthorizedResponseAsync(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to get calibration measurements. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while getting calibration measurements: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while getting calibration measurements: {ex.Message}.", ex);
            }
        }

        public static async Task AddAsync(CalibrationMeasurementDTO calibrationMeasurement)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PostAsJsonAsync("calibrationMeasurements", calibrationMeasurement);
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to add calibration measurement. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while adding calibration measurement: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while adding calibration measurement: {ex.Message}.", ex);
            }
        }

        public static async Task DeleteAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.DeleteAsync($"calibrationMeasurements/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to delete calibration measurement with ID {id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while deleting calibration measurement with ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while deleting calibration measurement with ID {id}: {ex.Message}.", ex);
            }
        }

        public static async Task UpdateAsync(CalibrationMeasurementDTO calibrationMeasurement)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PutAsJsonAsync("calibrationMeasurements", calibrationMeasurement);
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to update calibration measurement with ID {calibrationMeasurement.Id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while updating calibration measurement with ID {calibrationMeasurement.Id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while updating calibration measurement with ID {calibrationMeasurement.Id}: {ex.Message}.", ex);
            }
        }
    }
}
