using Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Client.Services
{
    public class UserService
    {
        public HttpClient _httpClient;

        public UserService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<List<UserModel>> GetUsersAsync(string accessToken)
        {
            var response = await _httpClient.GetAsync("/users");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<List<UserModel>>();
        }
    }
}
