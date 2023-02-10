using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Playlist_for_party.Exceptions.AppExceptions;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Models.SpotifyApiConnection;

namespace Playlist_for_party.Services
{
    public class SpotifyAccountService : ISpotifyAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SpotifyAccountService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetAccessToken()
        {
            try
            {
                return await GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);
            }
            catch
            {
                throw new InvalidTokensProvided();
            }
        }
        
        private static HttpRequestMessage CreateRequest(string clientId, string clientSecret)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "token");
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" }
            });
            return request;
        }

        private async Task<HttpResponseMessage> GetResponse(string clientId, string clientSecret)
        {
            var response = await _httpClient.SendAsync(CreateRequest(clientId, clientSecret));
            response.EnsureSuccessStatusCode();
            return response;
        }

        private async Task<string> GetToken(string clientId, string clientSecret)
        {
            var authResult = await GetResponse(clientId, clientSecret).Result.Content.ReadFromJsonAsync<AuthResult>();
            return authResult?.AccessToken;
        }
    }
}