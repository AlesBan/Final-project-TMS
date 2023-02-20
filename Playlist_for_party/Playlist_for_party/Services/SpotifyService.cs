using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Playlist_for_party.Data;
using Playlist_for_party.Exceptions.AppExceptions.MusicApiConnectionExceptions;
using Playlist_for_party.Exceptions.UserExceptions;
using Playlist_for_party.Interfaсes.Services;
using WebApp_Data.Models.Music;
using WebApp_Data.Models.SpotifyModels.DTO;
using WebApp_Data.Models.SpotifyModels.Main;
using WebApp_Data.Models.SpotifyModels.Secondary;

namespace Playlist_for_party.Services
{
    public class SpotifyService : IMusicService
    {
        private readonly HttpClient _httpClient;
        private readonly ISpotifyAccountService _spotifyAccountService;
        private const int Limit = 10;

        private const string DefImageUrl =
            @"https://media.wired.com/photos/5f9ca518227dbb78ec30dacf/master/w_2560%2Cc_limit/Gear-RIP-Google-Music-1194411695.jpg";

        public SpotifyService(HttpClient httpClient, ISpotifyAccountService spotifyAccountService)
        {
            _httpClient = httpClient;
            _spotifyAccountService = spotifyAccountService;
        }

        public async Task<Track> GetTrackFromSpotifyApi(string trackId)
        {
            var accessToken = await _spotifyAccountService.GetAccessToken();
            Authorization(accessToken);

            var response = await GetResponse($"tracks/{trackId}");
            var responseObj = await DeserializationAsync<Item>(response);
            var firstImage = responseObj.Album.Images?[0];

            var track = new Track
            {
                Id = responseObj.Id,
                Name = responseObj.Name,
                ArtistName = string.Join(", ", responseObj.Artists.Select(a => a.Name)),
                Album = responseObj.Album.Name,
                DurationMs = responseObj.DurationMs,
                ImageUrl = firstImage?.Url ?? DefImageUrl,
                Href = responseObj.Href
            };

            return track;
        }

        public async Task<ItemsDto> GetItemsFromSpotifyApi(MusicContext musicContext, string query)
        {
            var accessToken = await _spotifyAccountService.GetAccessToken();
            Authorization(accessToken);

            var requestMessage = CreateRequest(query, true, true);
            var response = await GetResponse(requestMessage);
            var responseObject = await DeserializationAsync<Search>(response);

            return GetItemsDtoList(musicContext, responseObject);
        }

        private static ItemsDto GetItemsDtoList(MusicContext musicContext, Search responseObj)
        {
            var artistsDto = responseObj?.Artists.Items.Where(i => i.Images?.Length > 0)
                .Select(i => new ArtistDto()
                {
                    Name = i.Name,
                    ImageRef = i.Images[0].Url,
                    Href = i.Href
                });

            var tracksDto = responseObj?.Tracks.Items.Where(i => i.Album.Images != null)
                .Select(i => new TrackDto()
                {
                    Name = i.Name,
                    ImageRef = i.Album.Images[0].Url,
                    Href = i.Href,
                    Id = i.Id,
                    ArtistName = string.Join(", ", i.Artists.Select(a => a.Name)),
                    Rating = GetTrackRating(musicContext, i.Id),
                    DurationMs = i.DurationMs
                });

            var itemsDto = new ItemsDto()
            {
                ArtistsDto = artistsDto?.ToList() ?? new List<ArtistDto>(),
                TracksDto = tracksDto?.ToList() ?? new List<TrackDto>()
            };

            return itemsDto;
        }

        private static int GetTrackRating(MusicContext musicContext, string trackId)
        {
            try
            {
                return musicContext.Tracks
                    .SingleOrDefault(t => t.Id == trackId)!.Rating;
            }
            catch
            {
                return 0;
            }
        }

        private void Authorization(string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private async Task<HttpResponseMessage> GetResponse(string requestMessage)
        {
            var response = await _httpClient.GetAsync(requestMessage);
            CheckResponse(response);

            return response;
        }

        private static string CreateRequest(string query, bool needArtists, bool needTracks)
        {
            var decodedQuery = HttpUtility.UrlEncode(query);
            var requestType = needTracks && needArtists ? "track" + "%2C" + "artist" : needArtists ? "artist" : "track";
            var requestUri = $"search?q={decodedQuery}&type={requestType}&market=BY&limit={Limit}";

            return requestUri;
        }

        private static void CheckResponse(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new BadRequestToSpotifyApiException();
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedException();
            }
        }

        private static async Task<T> DeserializationAsync<T>(HttpResponseMessage response)
        {
            T responseObj;

            try
            {
                responseObj = await response.Content.ReadFromJsonAsync<T>();
            }
            catch (JsonException ex)
            {
                throw new DeserializationOfSpotifyModelException(ex.Message);
            }

            return responseObj;
        }
    }
}