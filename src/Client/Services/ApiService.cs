using Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class ApiService
    {
        public HttpClient _httpClient;

        public ApiService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<List<UserModel>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("/users");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<UserModel>>();
        }
    }
}
