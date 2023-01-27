using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Models.Music;
using Playlist_for_party.Models.SpotifyApiConnection;
using Playlist_for_party.Models.SpotifyModels.DTO;
using Playlist_for_party.Models.SpotifyModels.Main;

namespace Playlist_for_party.Services
{
    public class SpotifyService: ISpotifyService
    {
        private readonly HttpClient _httpClient;

        public SpotifyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<IEnumerable<ArtistMy>> GetItems(int limit, string accessToken, string query)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _httpClient.GetAsync($"search?q={query}&type=artist");

            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            var responseObject = await JsonSerializer.DeserializeAsync<SearchArtists>(responseStream);

            return responseObject?.artists.Items.Where(i => i.Images.Length != 0).Select(i => new ArtistMy()
            {
                ArtistName = i.Name,
                ImageRef = i.Images[0].Url,
                Href = i.Href
            });
        }
        
        public async Task<IEnumerable<Release>> GetNewReleases(string countryCode, int limit, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"browse/new-releases?country={countryCode}&limit={limit}");

            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            var responseObject = await JsonSerializer.DeserializeAsync<NewRelease>(responseStream);

            return responseObject?.Albums?.Items.Select(i => new Release
            {
                Name = i.Name,
                Date = i.ReleaseDate,
                ImageUrl = i.Images.FirstOrDefault()?.Url,
                Link = i.ExternalUrls.Spotify,
                Artists = string.Join(",", i.Artists.Select(a => a.Name))
            });
        }
    }
}