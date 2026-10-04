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
                    throw new Exception($"Failed to get reference standard with ID {id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while getting reference standard with ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while getting reference standard with ID {id}: {ex.Message}.", ex);
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
                    throw new Exception($"Failed to get reference standards. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while getting reference standards: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while getting reference standards: {ex.Message}.", ex);
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
                    throw new Exception($"Failed to add reference standard. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while adding reference standard: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while adding reference standard: {ex.Message}.", ex);
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
                    throw new Exception($"Failed to delete reference standard with ID {id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while deleting reference standard with ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while deleting reference standard with ID {id}: {ex.Message}.", ex);
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
                    throw new Exception($"Failed to update reference standard with ID {referenceStandard.Id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while updating reference standard with ID {referenceStandard.Id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while updating reference standard with ID {referenceStandard.Id}: {ex.Message}.", ex);
            }
        }
    }
}
