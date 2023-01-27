using System.Threading.Tasks;

namespace Playlist_for_party.Interfaсes.Services
{
    public interface ISpotifyAccountService
    {
        Task<string> GetAccessToken();
    }
}