using System;
using System.Collections.Generic;
using WebApp_Data.Models.DbConnections;
using WebApp_Data.Models.Music;

namespace WebApp_Data.Models.UserData
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ImageRef { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<Playlist> UserOwnerPlaylists { get; set; } = new List<Playlist>();
        public ICollection<UserEditorPlaylist> UserEditorPlaylists { get; set; } = new List<UserEditorPlaylist>();
        public User(string userName, string password)
        {
            Id = Guid.NewGuid();
            UserName = userName;
            Password = password;
        }
    }
}