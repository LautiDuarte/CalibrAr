using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using System.Net.Http.Json;

namespace API.Clients
{
    public class InstrumentTypeApiClient : BaseApiClient
    {
        public InstrumentTypeApiClient(ITokenProvider tokenProvider) : base(tokenProvider)
        {
        }

        public async Task<InstrumentTypeDTO> GetAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync($"instrumentTypes/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var instrumentType = await response.Content.ReadFromJsonAsync<InstrumentTypeDTO>();
                    return instrumentType ?? throw new Exception($"La respuesta del servidor para el tipo de instrumento con ID {id} vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo obtener el tipo de instrumento con ID {id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener el tipo de instrumento con ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener el tipo de instrumento con ID {id}: {ex.Message}.", ex);
            }
        }

        public async Task<List<InstrumentTypeDTO>> GetAllAsync()
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync("instrumentTypes");
                if (response.IsSuccessStatusCode)
                {
                    var instrumentTypes = await response.Content.ReadFromJsonAsync<List<InstrumentTypeDTO>>();
                    return instrumentTypes ?? throw new Exception("La respuesta del servidor para la lista de tipos de instrumento vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudieron obtener los tipos de instrumento. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener los tipos de instrumento: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener los tipos de instrumento: {ex.Message}.", ex);
            }
        }

        public async Task AddAsync(InstrumentTypeDTO instrumentType)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PostAsJsonAsync("instrumentTypes", instrumentType);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo agregar el tipo de instrumento. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al agregar el tipo de instrumento: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al agregar el tipo de instrumento: {ex.Message}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.DeleteAsync($"instrumentTypes/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo eliminar el tipo de instrumento con ID {id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al eliminar el tipo de instrumento con ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al eliminar el tipo de instrumento con ID {id}: {ex.Message}.", ex);
            }
        }

        public async Task UpdateAsync(InstrumentTypeDTO instrumentType)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PutAsJsonAsync("instrumentTypes", instrumentType);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo actualizar el tipo de instrumento con ID {instrumentType.Id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al actualizar el tipo de instrumento con ID {instrumentType.Id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al actualizar el tipo de instrumento con ID {instrumentType.Id}: {ex.Message}.", ex);
            }
        }
    }
}
