using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace API.Clients
{
    public class ProcedureApiClient : BaseApiClient
    {
        public ProcedureApiClient(ITokenProvider tokenProvider) : base(tokenProvider)
        {
        }

        public async Task<ProcedureDTO> GetAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync($"procedures/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var procedure = await response.Content.ReadFromJsonAsync<ProcedureDTO>();
                    return procedure ?? throw new Exception($"La respuesta del servidor para el procedimiento con ID {id} vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo obtener el procedimiento con ID {id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener el procedimiento con ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener el procedimiento con ID {id}: {ex.Message}.", ex);
            }
        }

        public async Task<List<ProcedureDTO>> GetAllAsync()
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync("procedures");
                if (response.IsSuccessStatusCode)
                {
                    var procedures = await response.Content.ReadFromJsonAsync<List<ProcedureDTO>>();
                    return procedures ?? throw new Exception("La respuesta del servidor para la lista de procedimientos vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudieron obtener los procedimientos. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener los procedimientos: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener los procedimientos: {ex.Message}.", ex);
            }
        }

        public async Task AddAsync(ProcedureDTO procedure)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PostAsJsonAsync("procedures", procedure);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo agregar el procedimiento. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al agregar el procedimiento: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al agregar el procedimiento: {ex.Message}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.DeleteAsync($"procedures/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo eliminar el procedimiento con ID {id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al eliminar el procedimiento con ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al eliminar el procedimiento con ID {id}: {ex.Message}.", ex);
            }
        }

        public async Task UpdateAsync(ProcedureDTO procedure)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PutAsJsonAsync("procedures", procedure);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo actualizar el procedimiento con ID {procedure.Id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al actualizar el procedimiento con ID {procedure.Id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al actualizar el procedimiento con ID {procedure.Id}: {ex.Message}.", ex);
            }
        }
    }
}
