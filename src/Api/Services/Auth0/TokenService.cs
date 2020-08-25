using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Api.Services.Auth0
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TokenService> _logger;

        public TokenService(HttpClient httpClient, ILogger<TokenService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetClientCredentialsToken(string audience, string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(audience))            
                throw new ArgumentException("audience", nameof(audience));            

            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentException("clientId", nameof(clientId));   

            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new ArgumentException("clientSecret", nameof(clientSecret));            

            var parameters = new Dictionary<string, string> {
                { "grant_type", "client_credentials" },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "audience", audience } };

            var content = new FormUrlEncodedContent(parameters.Select(p => new KeyValuePair<string, string>(p.Key, p.Value?.ToString() ?? "")));

            using var request = new HttpRequestMessage(HttpMethod.Post, "oauth/token")
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);
            var jsonContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var token = JsonConvert.DeserializeObject<Token>(jsonContent);
                return token.AccessToken;
            }

            var error = $"error on get client cred token from Auth0, http-response: {jsonContent}";
            _logger.LogError(error);

            return null;
        }
    }
}
