using System;
using System.Collections.Generic;
using WebApp_Data.Models;
using WebApp_Data.Models.Music;

namespace WebApp_Data.Interfaces
{
    public interface IMusicRepository
    {
        List<Playlist> Playlists { get; set; }
        
        public List<User> Users { get; set; }

    }
}