using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using System.Net.Http.Json;


namespace API.Clients
{
    public class LocationApiClient : BaseApiClient
    {
        public LocationApiClient(ITokenProvider tokenProvider) : base(tokenProvider)
        {
        }

        public async Task<LocationDTO> GetAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync($"locations/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var location = await response.Content.ReadFromJsonAsync<LocationDTO>();
                    return location ?? throw new Exception($"La respuesta del servidor para la ubicacion con ID {id} vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo obtener la ubicación con ID {id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener la ubicación con ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener la ubicación con ID {id}: {ex.Message}.", ex);
            }

        }

        public async Task<List<LocationDTO>> GetAllAsync()
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync("locations");
                if (response.IsSuccessStatusCode)
                {
                    var locations = await response.Content.ReadFromJsonAsync<List<LocationDTO>>();
                    return locations ?? throw new Exception("La respuesta del servidor para la lista de ubicaciones vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudieron obtener las ubicaciones. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener las ubicaciones: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener las ubicaciones: {ex.Message}.", ex);
            }
        }

        public async Task AddAsync(LocationDTO location)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PostAsJsonAsync("locations", location);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo crear la ubicación. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al crear la ubicación: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al crear la ubicación: {ex.Message}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.DeleteAsync($"locations/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo eliminar la ubicación con ID {id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al eliminar la ubicación con ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al eliminar la ubicación con ID {id}: {ex.Message}.", ex);
            }
        }

        public async Task UpdateAsync(LocationDTO location)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PutAsJsonAsync("locations", location);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo actualizar la ubicación con ID {location.Id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al actualizar la ubicación con ID {location.Id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al actualizar la ubicación con ID {location.Id}: {ex.Message}.", ex);
            }
        }
    }
}
