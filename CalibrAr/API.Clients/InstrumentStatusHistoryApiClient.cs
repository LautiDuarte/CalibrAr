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
                    throw new Exception($"No se pudo obtener el historial de estados con ID {id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener el historial de estados con ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener el historial de estados con ID {id}: {ex.Message}.", ex);
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
                    throw new Exception($"No se pudieron obtener los historiales de estados. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener los historiales de estados: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener los historiales de estados: {ex.Message}.", ex);
            }
        }
    }
}
