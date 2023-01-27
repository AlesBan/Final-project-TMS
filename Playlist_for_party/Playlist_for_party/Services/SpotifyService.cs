using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Models.Music;
using Playlist_for_party.Models.SpotifyApiConnection;

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

            return responseObject.artists.items.Where(i => i.images.Length != 0).Select(i => new ArtistMy()
            {
                ArtistName = i.name,
                ImageRef = i.images[0].url,
                Href = i.href
            });
        }
        
        public async Task<IEnumerable<Release>> GetNewReleases(string countryCode, int limit, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync($"browse/new-releases?country={countryCode}&limit={limit}");

            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            var responseObject = await JsonSerializer.DeserializeAsync<NewRelease>(responseStream);

            return responseObject?.albums?.items.Select(i => new Release
            {
                Name = i.name,
                Date = i.release_date,
                ImageUrl = i.images.FirstOrDefault().url,
                Link = i.external_urls.spotify,
                Artists = string.Join(",", i.artists.Select(i => i.name))
            });
        }
    }
}