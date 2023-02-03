using System;
using System.Collections.Generic;
using WebApp_Data.Models.Music;

namespace WebApp_Data.Models
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
    }
}