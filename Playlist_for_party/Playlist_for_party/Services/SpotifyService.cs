using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Models.Music;
using Playlist_for_party.Models.SpotifyModels.DTO;
using Playlist_for_party.Models.SpotifyModels.Main;
using Playlist_for_party.Models.SpotifyModels.Secondary;

namespace Playlist_for_party.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly HttpClient _httpClient;

        private const string DefImageUrl =
            @"https://media.wired.com/photos/5f9ca518227dbb78ec30dacf/master/w_2560%2Cc_limit/Gear-RIP-Google-Music-1194411695.jpg";

        public SpotifyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Track> GetTrack(string accessToken, string trackId)
        {
            Authorization(accessToken);
            var response = await GetResponse($"tracks/{trackId}");
            var responseObj = await DeserializationAsync<Item>(response);
            var track = new Track()
            {
                TrackId = responseObj.Id,
                Name = responseObj.Name,
                Artist = string.Join(", ", responseObj.Artists.ToList()),
                Album = responseObj.Album.Name,
                Duration = responseObj.DurationMs,
                ImageUrl = responseObj.Album.Images != null ? responseObj.Album.Images[0].Url : DefImageUrl,
                Href = responseObj.Href
            };
            return track;
        }

        public async Task<IEnumerable<ItemDto>> GetItems(int limit, string accessToken, string query)
        {
            Authorization(accessToken);
            var response = await GetResponse($"search?q={query}&type=track%2Cartist&limit={limit}");
            var responseObject = await DeserializationAsync<Search>(response);
            return GetItemDtosLIst(responseObject);
        }

        private IEnumerable<ItemDto> GetItemDtosLIst(Search responseObj)
        {
            var artistDtos = responseObj?.Artists.Items.Select(i => new ArtistDto()
            {
                Name = i.Name,
                ImageRef = i.Images != null ? i.Images[0].Url : DefImageUrl,
                Href = i.Href
            });
            var trackDtos = responseObj?.Tracks.Items.Select(i => new TrackDto()
            {
                Name = i.Name,
                ImageRef = i.Album.Images != null ? i.Album.Images[0].Url : DefImageUrl,
                Href = i.Href,
                Id = i.Id,
                ArtistName = string.Join(", ", i.Artists.Select(a => a.Name))
            });
            var itemDtos = new List<ItemDto>();

            if (artistDtos != null)
            {
                itemDtos.AddRange(artistDtos);
            }

            if (trackDtos != null)
            {
                itemDtos.AddRange(trackDtos);
            }

            return itemDtos;
        }

        public async Task<IEnumerable<ReleaseDto>> GetNewReleases(string countryCode, int limit, string accessToken)
        {
            Authorization(accessToken);

            var response = await GetResponse($"browse/new-releases?country={countryCode}&limit={limit}");

            var responseObject = await DeserializationAsync<NewRelease>(response);

            return responseObject?.Albums?.Items.Select(i => new ReleaseDto
            {
                Name = i.Name,
                Date = i.ReleaseDate,
                ImageUrl = i.Images.FirstOrDefault()?.Url,
                Link = i.ExternalUrls.Spotify,
                Artists = string.Join(",", i.Artists.Select(a => a.Name))
            });
        }

        private void Authorization(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private async Task<HttpResponseMessage> GetResponse(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();
            return response;
        }

        private static async Task<T> DeserializationAsync<T>(HttpResponseMessage response)
        {
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}