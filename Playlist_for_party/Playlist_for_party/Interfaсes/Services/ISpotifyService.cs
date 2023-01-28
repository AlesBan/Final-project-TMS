using System.Collections.Generic;
using System.Threading.Tasks;
using Playlist_for_party.Models.Music;
using Playlist_for_party.Models.SpotifyApiConnection;
using Playlist_for_party.Models.SpotifyModels.DTO;

namespace Playlist_for_party.Interfaсes.Services
{
    public interface ISpotifyService
    {
        Task<IEnumerable<ReleaseDto>> GetNewReleases(string countryCode, int limit, string accessToken);
        Task<IEnumerable<ItemDto>> GetItems(int limit, string accessToken, string query);
        Task<Track> GetTrack(string accessToken, string trackId);
        Playlist Playlist { get; set; }
    }
}