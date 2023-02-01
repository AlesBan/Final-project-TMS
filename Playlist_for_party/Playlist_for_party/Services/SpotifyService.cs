using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Playlist_for_party.Exceptions;
using Playlist_for_party.Interfaсes.Services;
using Playlist_for_party.Models.Music;
using Playlist_for_party.Models.SpotifyModels.DTO;
using Playlist_for_party.Models.SpotifyModels.Main;
using Playlist_for_party.Models.SpotifyModels.Secondary;

namespace Playlist_for_party.Services
{
    public class SpotifyService : IMusicService
    {
        private readonly HttpClient _httpClient;
        private readonly ISpotifyAccountService _spotifyAccountService;
        private const int Limit = 6;

        private const string DefImageUrl =
            @"https://media.wired.com/photos/5f9ca518227dbb78ec30dacf/master/w_2560%2Cc_limit/Gear-RIP-Google-Music-1194411695.jpg";

        public SpotifyService(HttpClient httpClient, ISpotifyAccountService spotifyAccountService)
        {
            _httpClient = httpClient;
            _spotifyAccountService = spotifyAccountService;
        }

        public async Task<Track> GetTrack(string trackId)
        {
            var accessToken = await _spotifyAccountService.GetAccessToken();
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

        public async Task<ItemsDto> GetItems(string query)
        {
            var accessToken = await _spotifyAccountService.GetAccessToken();
            Authorization(accessToken);
            var response = await GetResponse(CreateRequest(query, true, true));
            var responseObject = await DeserializationAsync<Search>(response);
            return GetItemDtosLIst(responseObject);
        }

        private string CreateRequest(string query, bool needArtists, bool needTracks)
        {
            var request = $"search?q={query}&type=";
            if (needTracks && needArtists)
            {
                request += "track%2Cartist";
            }
            else
            {
                request += needArtists ? "artist" : "track";
            }
            request += $"&limit={Limit}";
            return request;
        }
        
        private static ItemsDto GetItemDtosLIst(Search responseObj)
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
            var itemsDtos = new ItemsDto();
            if (artistDtos != null)
            {
                itemsDtos.ArtistDtos.AddRange(artistDtos);
            }

            if (trackDtos != null)
            {
                itemsDtos.TrackDtos.AddRange(trackDtos);
            }

            return itemsDtos;
        }

        private static void CheckResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new BadRequestToSpotifyApiException();
            }

            if (response.ReasonPhrase == "Unauthorized")
            {
                throw new UnauthorizedException();
            }
        }

        private async Task<HttpResponseMessage> GetResponse(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            CheckResponse(response);
            return response;
        }

        private static async Task<T> DeserializationAsync<T>(HttpResponseMessage response)
        {
            T responseObj;
            try
            {
                responseObj = await response.Content.ReadFromJsonAsync<T>();
            }
            catch
            {
                throw new DeserializationOfSpotifyModelException();
            }

            return responseObj;
        }
        
        private void Authorization(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }
}