using System.Collections.Generic;
using System.Threading.Tasks;
using Playlist_for_party.Models.Music;
using Playlist_for_party.Models.SpotifyModels.DTO;

namespace Playlist_for_party.Interfaсes.Services
{
    public interface ISpotifyService
    {
        Task<ItemsDto> GetItems(string query);
        Task<Track> GetTrack (string trackId);
        
    }
}