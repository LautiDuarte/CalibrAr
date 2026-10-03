using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace API.Clients
{
    public class InstrumentStatusHistoryApiClient : BaseApiClient
    {
        public static async Task<InstrumentStatusHistoryDTO> GetAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync($"instrumentStatusHistories/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var instrumentStatusHistory = await response.Content.ReadFromJsonAsync<InstrumentStatusHistoryDTO>();
                    return instrumentStatusHistory ?? throw new Exception($"La respuesta del servidor para el historial de estado con ID {id} vino vacia.");
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to get instrument status history with ID {id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while getting instrument status history with ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while getting instrument status history with ID {id}: {ex.Message}.", ex);
            }
        }

        public static async Task<List<InstrumentStatusHistoryDTO>> GetAllAsync()
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync("instrumentStatusHistories");
                if (response.IsSuccessStatusCode)
                {
                    var instrumentStatusHistories = await response.Content.ReadFromJsonAsync<List<InstrumentStatusHistoryDTO>>();
                    return instrumentStatusHistories ?? throw new Exception("La respuesta del servidor para la lista de historiales de estado vino vacia.");
                }
                else
                {
                    await HandleUnauthorizedResponseAsync(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to get instrument status histories. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while getting instrument status histories: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while getting instrument status histories: {ex.Message}.", ex);
            }
        }

        public static async Task AddAsync(InstrumentStatusHistoryDTO instrumentStatusHistory)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PostAsJsonAsync("instrumentStatusHistories", instrumentStatusHistory);
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to add instrument status history. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while adding instrument status history: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while adding instrument status history: {ex.Message}.", ex);
            }
        }

        public static async Task DeleteAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.DeleteAsync($"instrumentStatusHistories/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to delete instrument status history with ID {id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while deleting instrument status history with ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while deleting instrument status history with ID {id}: {ex.Message}.", ex);
            }
        }

        public static async Task UpdateAsync(InstrumentStatusHistoryDTO instrumentStatusHistory)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PutAsJsonAsync("instrumentStatusHistories", instrumentStatusHistory);
                if (!response.IsSuccessStatusCode)
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to update instrument status history with ID {instrumentStatusHistory.Id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while updating instrument status history with ID {instrumentStatusHistory.Id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while updating instrument status history with ID {instrumentStatusHistory.Id}: {ex.Message}.", ex);
            }
        }
    }
}
