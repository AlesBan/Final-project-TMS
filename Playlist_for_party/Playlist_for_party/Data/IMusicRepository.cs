using System;
using System.Collections.Generic;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Data
{
    public interface IMusicRepository
    {
        List<Playlist> Playlists { get; set; }

        Playlist CreatePlaylist();

        Playlist GetPlaylist(Guid id);
    }
}