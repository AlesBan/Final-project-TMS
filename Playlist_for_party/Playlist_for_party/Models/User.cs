using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Playlist_for_party.Data;
using Playlist_for_party.Models.DbConnections;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ImageRef { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public ICollection<Playlist> CreatedPlaylists { get; set; }
        public ICollection<UserEditorPlaylists> UserEditorPlaylists { get; set; }
    }
}