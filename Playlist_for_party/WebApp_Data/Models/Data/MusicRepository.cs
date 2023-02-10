using System;
using System.Collections.Generic;
using System.Linq;
using WebApp_Data.Interfaces;
using WebApp_Data.Models.Music;

namespace WebApp_Data.Models.Data
{
    public class MusicRepository : IMusicRepository
    {
        public List<User> Users { get; set; }

        public List<Playlist> Playlists { get; set; } = new List<Playlist>();

    }
}