using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DTOs;


namespace API.Clients
{
    public class InstrumentApiClient : BaseApiClient
    {
        public InstrumentApiClient(ITokenProvider tokenProvider) : base(tokenProvider)
        {
        }

        public async Task<InstrumentDTO> GetAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync($"instruments/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var instrument = await response.Content.ReadFromJsonAsync<InstrumentDTO>();
                    return instrument ?? throw new Exception($"La respuesta del servidor para el instrumento con ID {id} vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo obtener el instrumento con ID {id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener el instrumento con ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener el instrumento con ID {id}: {ex.Message}.", ex);
            }
        }

        public async Task<List<InstrumentDTO>> GetAllAsync()
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync("instruments");
                if (response.IsSuccessStatusCode)
                {
                    var instruments = await response.Content.ReadFromJsonAsync<List<InstrumentDTO>>();
                    return instruments ?? throw new Exception("La respuesta del servidor para la lista de instrumentos vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudieron obtener los instrumentos. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener los instrumentos: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener los instrumentos: {ex.Message}.", ex);
            }
        }

        // El filtrado lo hace el servidor en la base; los parámetros en null no filtran
        public async Task<List<InstrumentDTO>> SearchAsync(string? text = null, string? status = null, int? areaId = null, bool hideDecommissioned = false)
        {
            try
            {
                string query = BuildQuery(
                    ("text", text),
                    ("status", status),
                    ("areaId", areaId?.ToString()),
                    ("hideDecommissioned", hideDecommissioned ? "true" : null));

                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync("instruments/search" + query);
                if (response.IsSuccessStatusCode)
                {
                    var instruments = await response.Content.ReadFromJsonAsync<List<InstrumentDTO>>();
                    return instruments ?? throw new Exception("La respuesta del servidor para la busqueda de instrumentos vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudieron buscar los instrumentos. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al buscar los instrumentos: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al buscar los instrumentos: {ex.Message}.", ex);
            }
        }

        public async Task AddAsync(InstrumentDTO instrument)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PostAsJsonAsync("instruments", instrument);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo agregar el instrumento. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al agregar el instrumento: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al agregar el instrumento: {ex.Message}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.DeleteAsync($"instruments/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo eliminar el instrumento con ID {id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al eliminar el instrumento con ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al eliminar el instrumento con ID {id}: {ex.Message}.", ex);
            }
        }

        public async Task UpdateAsync(InstrumentDTO instrument)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PutAsJsonAsync("instruments", instrument);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo actualizar el instrumento con ID {instrument.Id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al actualizar el instrumento con ID {instrument.Id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al actualizar el instrumento con ID {instrument.Id}: {ex.Message}.", ex);
            }
        }
    }
}
