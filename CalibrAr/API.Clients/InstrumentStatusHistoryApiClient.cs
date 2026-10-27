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
        public InstrumentStatusHistoryApiClient(ITokenProvider tokenProvider) : base(tokenProvider)
        {
        }

        public async Task<InstrumentStatusHistoryDTO> GetAsync(int id)
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
                    ThrowIfUnauthorized(response);
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

        public async Task<List<InstrumentStatusHistoryDTO>> GetAllAsync()
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
                    ThrowIfUnauthorized(response);
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
    }
}
