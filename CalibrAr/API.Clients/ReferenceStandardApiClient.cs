using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace API.Clients
{
    public class ReferenceStandardApiClient : BaseApiClient
    {
        public ReferenceStandardApiClient(ITokenProvider tokenProvider) : base(tokenProvider)
        {
        }

        public async Task<ReferenceStandardDTO> GetAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync($"referenceStandards/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var referenceStandard = await response.Content.ReadFromJsonAsync<ReferenceStandardDTO>();
                    return referenceStandard ?? throw new Exception($"La respuesta del servidor para el patron de referencia con ID {id} vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo obtener el patrón de referencia con ID {id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener el patrón de referencia con ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener el patrón de referencia con ID {id}: {ex.Message}.", ex);
            }
        }

        public async Task<List<ReferenceStandardDTO>> GetAllAsync()
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync("referenceStandards");
                if (response.IsSuccessStatusCode)
                {
                    var referenceStandards = await response.Content.ReadFromJsonAsync<List<ReferenceStandardDTO>>();
                    return referenceStandards ?? throw new Exception("La respuesta del servidor para la lista de patrones de referencia vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudieron obtener los patrones de referencia. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener los patrones de referencia: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener los patrones de referencia: {ex.Message}.", ex);
            }
        }

        public async Task AddAsync(ReferenceStandardDTO referenceStandard)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PostAsJsonAsync("referenceStandards", referenceStandard);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo agregar el patrón de referencia. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al agregar el patrón de referencia: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al agregar el patrón de referencia: {ex.Message}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.DeleteAsync($"referenceStandards/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo eliminar el patrón de referencia con ID {id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al eliminar el patrón de referencia con ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al eliminar el patrón de referencia con ID {id}: {ex.Message}.", ex);
            }
        }

        public async Task UpdateAsync(ReferenceStandardDTO referenceStandard)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PutAsJsonAsync("referenceStandards", referenceStandard);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo actualizar el patrón de referencia con ID {referenceStandard.Id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al actualizar el patrón de referencia con ID {referenceStandard.Id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al actualizar el patrón de referencia con ID {referenceStandard.Id}: {ex.Message}.", ex);
            }
        }
    }
}
