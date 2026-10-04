using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace API.Clients
{
    public class NonConformityApiClient : BaseApiClient
    {
        public NonConformityApiClient(ITokenProvider tokenProvider) : base(tokenProvider)
        {
        }

        public async Task<NonConformityDTO> GetAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync($"nonConformities/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var nonConformity = await response.Content.ReadFromJsonAsync<NonConformityDTO>();
                    return nonConformity ?? throw new Exception($"La respuesta del servidor para la no conformidad con ID {id} vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to get non conformity with ID {id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while getting non conformity with ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while getting non conformity with ID {id}: {ex.Message}.", ex);
            }
        }

        public async Task<List<NonConformityDTO>> GetAllAsync()
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync("nonConformities");
                if (response.IsSuccessStatusCode)
                {
                    var nonConformities = await response.Content.ReadFromJsonAsync<List<NonConformityDTO>>();
                    return nonConformities ?? throw new Exception("La respuesta del servidor para la lista de no conformidades vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to get non conformities. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while getting non conformities: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while getting non conformities: {ex.Message}.", ex);
            }
        }

        public async Task AddAsync(NonConformityDTO nonConformity)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PostAsJsonAsync("nonConformities", nonConformity);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to add non conformity. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while adding non conformity: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while adding non conformity: {ex.Message}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.DeleteAsync($"nonConformities/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to delete non conformity with ID {id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while deleting non conformity with ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while deleting non conformity with ID {id}: {ex.Message}.", ex);
            }
        }

        public async Task UpdateAsync(NonConformityDTO nonConformity)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PutAsJsonAsync("nonConformities", nonConformity);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to update non conformity with ID {nonConformity.Id}. Status code: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"An error occurred while updating non conformity with ID {nonConformity.Id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Request timed out while updating non conformity with ID {nonConformity.Id}: {ex.Message}.", ex);
            }
        }
    }
}
