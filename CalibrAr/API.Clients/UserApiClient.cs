using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace API.Clients
{
    public class UserApiClient : BaseApiClient
    {
        public UserApiClient(ITokenProvider tokenProvider) : base(tokenProvider)
        {
        }

        public async Task<UserDTO> GetAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync($"users/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<UserDTO>();
                    return user ?? throw new Exception($"La respuesta del servidor para el usuario con ID {id} vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo obtener el usuario con ID {id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener el usuario con ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener el usuario con ID {id}: {ex.Message}.", ex);
            }
        }

        public async Task<List<UserDTO>> GetAllAsync()
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.GetAsync("users");
                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
                    return users ?? throw new Exception("La respuesta del servidor para la lista de usuarios vino vacia.");
                }
                else
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudieron obtener los usuarios. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al obtener los usuarios: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al obtener los usuarios: {ex.Message}.", ex);
            }
        }

        public async Task AddAsync(UserDTO user)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PostAsJsonAsync("users", user);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo crear el usuario. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al crear el usuario: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al crear el usuario: {ex.Message}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.DeleteAsync($"users/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo eliminar el usuario con ID {id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al eliminar el usuario con ID {id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al eliminar el usuario con ID {id}: {ex.Message}.", ex);
            }
        }

        public async Task UpdateAsync(UserDTO user)
        {
            try
            {
                using var client = await CreateHttpClientAsync();
                HttpResponseMessage response = await client.PutAsJsonAsync("users", user);
                if (!response.IsSuccessStatusCode)
                {
                    ThrowIfUnauthorized(response);
                    string errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"No se pudo actualizar el usuario con ID {user.Id}. Código de estado: {response.StatusCode}, Error: {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Ocurrió un error al actualizar el usuario con ID {user.Id}: {ex.Message}.", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new Exception($"Se agotó el tiempo de espera al actualizar el usuario con ID {user.Id}: {ex.Message}.", ex);
            }
        }
    }
}
