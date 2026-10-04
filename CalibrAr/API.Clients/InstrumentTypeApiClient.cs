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
                    throw new Exception($"Failed to get instrument type with ID {id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while getting instrument type with ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while getting instrument type with ID {id}: {ex.Message}.", ex);
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
                    throw new Exception($"Failed to get instrument types. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while getting instrument types: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while getting instrument types: {ex.Message}.", ex);
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
                    throw new Exception($"Failed to add instrument type. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while adding instrument type: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while adding instrument type: {ex.Message}.", ex);
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
                    throw new Exception($"Failed to delete instrument type with ID {id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while deleting instrument type with ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while deleting instrument type with ID {id}: {ex.Message}.", ex);
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
                    throw new Exception($"Failed to update instrument type with ID {instrumentType.Id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while updating instrument type with ID {instrumentType.Id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while updating instrument type with ID {instrumentType.Id}: {ex.Message}.", ex);
            }
        }
    }
}
