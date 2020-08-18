using Api.Services.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace Api.Services.Auth0
{
    internal class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<UserService> _logger;
        private readonly TokenService _tokenService;
        private readonly IMapper _mapper;

        public UserService(HttpClient httpClient, IConfiguration config, ILogger<UserService> logger, TokenService tokenService,
            IMapper mapper)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
            _tokenService = tokenService;
            _mapper = mapper;

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Task<IEnumerable<UserModel>> GetUsersAsync(Dictionary<string, string> filter)
        {
            return GetUsersAsync(filter.GetSearchQuery());
        }

        private async Task<IEnumerable<UserModel>> GetUsersAsync(string searchQuery)
        {
            var audience = string.Format("{0}/api/v2/", _config["Auth0:Authority"]);
            var accessToken = await _tokenService.GetClientCredentialsToken(audience, _config["Auth0:ClientId"], _config["Auth0:ClientSecret"]);

            if (string.IsNullOrWhiteSpace(accessToken))
                _logger.LogError("Error requesting access token");

            var requestUri = string.IsNullOrWhiteSpace(searchQuery) 
                ? "api/v2/users" : 
                QueryHelpers.AddQueryString("api/v2/users", "q", searchQuery);

            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            var jsonContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var users = JsonConvert.DeserializeObject<IEnumerable<User>>(jsonContent);
                return _mapper.Map<List<UserModel>>(users);
            }

            var error = $"error on get users from Auth0, http-response: {jsonContent}";
            _logger.LogError(error);

            return null;
        }
    }
}
