using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace API.Clients
{
    public class CalibrationApiClient : BaseApiClient
    {
        public static async Task<CalibrationDTO> GetAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync($"calibrations/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CalibrationDTO>();
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to get calibration with ID {id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while getting calibration with ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while getting calibration with ID {id}: {ex.Message}.", ex);
            }
        }

        public static async Task<List<CalibrationDTO>> GetAllAsync()
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync("calibrations");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<CalibrationDTO>>();
                }
                else
                {
                    await HandleUnauthorizedResponseAsync(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to get calibrations. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while getting calibrations: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while getting calibrations: {ex.Message}.", ex);
            }
        }

        public static async Task AddAsync(CalibrationDTO calibration)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PostAsJsonAsync("calibrations", calibration);
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to add calibration. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while adding calibration: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while adding calibration: {ex.Message}.", ex);
            }
        }

        public static async Task DeleteAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.DeleteAsync($"calibrations/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to delete calibration with ID {id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while deleting calibration with ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while deleting calibration with ID {id}: {ex.Message}.", ex);
            }
        }

        public static async Task UpdateAsync(CalibrationDTO calibration)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PutAsJsonAsync("calibrations", calibration);
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to update calibration with ID {calibration.Id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while updating calibration with ID {calibration.Id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while updating calibration with ID {calibration.Id}: {ex.Message}.", ex);
            }
        }
    }
}
