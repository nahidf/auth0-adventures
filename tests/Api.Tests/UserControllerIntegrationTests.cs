using Microsoft.AspNetCore.Mvc.Testing;
using Models;
using Newtonsoft.Json;
using Should;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace Api.Tests
{
    public partial class UserControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient httpClient;

        public UserControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetUsers_NoBearer_ShouldReturnUnAuthorized()
        {
            // Act
            var response = await httpClient.GetAsync("users");

            response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetUsers_InvalidBearer_ShouldReturnUnAuthorized()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "users");

            var response = await httpClient.GetAsync("users");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "This is an invalid token");

            response.StatusCode.ShouldEqual(HttpStatusCode.Unauthorized);
        }

        [Fact]

        public async Task GetUsers_ValidBearer_ShouldReturnAllUsers()
        {
            var bearer = await FixtureHelpers.GetFixtureContentsAsync("token.txt");//Todo: this can be replaced with getting token from auth0
            var request = new HttpRequestMessage(HttpMethod.Get, "users");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            var response = await httpClient.SendAsync(request);

            var jsonContent = await response.Content.ReadAsStringAsync();
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
            var users = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(jsonContent);

            users.ShouldNotBeEmpty();
            users.Count().ShouldBeGreaterThan(6);
        }

        [Fact]
        public async Task GetUsers_ValidBearer_InvalidFilter_ShouldReturnAllUsers()
        {
            var bearer = await FixtureHelpers.GetFixtureContentsAsync("token.txt");//Todo: this can be replaced with getting token from auth0
            var request = new HttpRequestMessage(HttpMethod.Get, "users?filter=test");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            var response = await httpClient.SendAsync(request);

            var jsonContent = await response.Content.ReadAsStringAsync();
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
            var users = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(jsonContent);

            users.ShouldNotBeEmpty();
            users.Count().ShouldBeGreaterThan(6);
        }

        [Fact]

        public async Task GetUsers_ValidBearer_SearchByName_ShouldReturnOne()
        {
            var bearer = await FixtureHelpers.GetFixtureContentsAsync("token.txt");//Todo: this can be replaced with getting token from auth0
            var request = new HttpRequestMessage(HttpMethod.Get, "users?name=nahid");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            var response = await httpClient.SendAsync(request);

            var jsonContent = await response.Content.ReadAsStringAsync();
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
            var users = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(jsonContent);

            users.ShouldNotBeEmpty();
            users.Count().ShouldEqual(1);
            users.First().Name.ShouldEqual("Nahid");
        }

        [Fact]
        public async Task GetUsers_ValidBearer_SearchByEmail_ShouldReturnOne()
        {
            var bearer = await FixtureHelpers.GetFixtureContentsAsync("token.txt");//Todo: this can be replaced with getting token from auth0
            var request = new HttpRequestMessage(HttpMethod.Get, "users?email=nahid@test.com");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            var response = await httpClient.SendAsync(request);

            var jsonContent = await response.Content.ReadAsStringAsync();
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
            var users = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(jsonContent);

            users.ShouldNotBeEmpty();
            users.Count().ShouldEqual(1);
            users.First().Name.ShouldEqual("Nahid");
        }

        [Fact]
        public async Task GetUsers_ValidBearer_SearchByEmailAndNickName_ShouldReturnTwo()
        {
            var bearer = await FixtureHelpers.GetFixtureContentsAsync("token.txt");//Todo: this can be replaced with getting token from auth0
            var request = new HttpRequestMessage(HttpMethod.Get, "users?email=nahid@test.com&nickname=rainbow");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            var response = await httpClient.SendAsync(request);

            var jsonContent = await response.Content.ReadAsStringAsync();
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
            var users = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(jsonContent);

            users.ShouldNotBeEmpty();
            users.Count().ShouldEqual(2);
        }

        [Fact]
        public async Task GetUsers_ValidBearer_SearchByNickName_ShouldReturnOne()
        {
            var bearer = await FixtureHelpers.GetFixtureContentsAsync("token.txt");//Todo: this can be replaced with getting token from auth0
            var request = new HttpRequestMessage(HttpMethod.Get, "users?nickname=nahid");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearer);

            var response = await httpClient.SendAsync(request);

            var jsonContent = await response.Content.ReadAsStringAsync();
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);
            var users = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(jsonContent);

            users.ShouldNotBeEmpty();
            users.Count().ShouldEqual(1);
            users.First().NickName.ShouldEqual("nahid");
        }
    }
}
