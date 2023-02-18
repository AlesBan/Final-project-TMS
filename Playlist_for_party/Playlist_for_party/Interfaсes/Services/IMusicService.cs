using System.Threading.Tasks;
using WebApp_Data.Models.Music;
using WebApp_Data.Models.SpotifyModels.DTO;

namespace Playlist_for_party.Interfaсes.Services
{
    public interface IMusicService
    {
        Task<ItemsDto> GetItemsFromSpotifyApi(string query);
        Task<Track> GetTrackFromSpotifyApi (string trackId);
        
    }
}