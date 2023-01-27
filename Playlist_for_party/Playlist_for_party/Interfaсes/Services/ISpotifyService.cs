using System.Collections.Generic;
using System.Threading.Tasks;
using Playlist_for_party.Models.Music;
using Playlist_for_party.Models.SpotifyApiConnection;
using Playlist_for_party.Models.SpotifyModels.DTO;

namespace Playlist_for_party.Interfaсes.Services
{
    public interface ISpotifyService
    {
        Task<IEnumerable<Release>> GetNewReleases(string countryCode, int limit, string accessToken);
        Task<IEnumerable<ArtistMy>> GetItems(int limit, string accessToken, string query);
    }
}