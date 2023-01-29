using System.Collections.Generic;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Data
{
    public class MusicRepository
    {
        public Playlist Playlist { get; set; }

        public MusicRepository()
        {
            Playlist = new Playlist();
        }
    }
}