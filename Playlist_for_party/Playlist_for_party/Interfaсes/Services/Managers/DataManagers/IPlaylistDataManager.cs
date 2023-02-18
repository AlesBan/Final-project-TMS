using System.Collections.Generic;
using WebApp_Data.Models.Music;
using WebApp_Data.Models.UserData;

namespace Playlist_for_party.Interfaсes.Services.Managers.DataManagers
{
    public interface IPlaylistDataManager
    {
        void AddTrack(User user, Playlist playlist, Track track);
        IEnumerable<Track> GetTracksFromPlaylist(Playlist playlist);
        bool IsOwner(User user, Playlist playlist);
        bool IsRedactor(User user, Playlist playlist);
        int GetNumOfEditors(Playlist playlist);
        void SetRedactorToPlaylist(User user, Playlist playlist);
        string GetResultOfAddingAbility(User user, Playlist playlist, Track track);
    }
}