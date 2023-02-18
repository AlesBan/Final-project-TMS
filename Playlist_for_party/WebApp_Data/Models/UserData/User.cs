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
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Playlist> UserOwnerPlaylists { get; set; }
        public ICollection<UserEditorPlaylist> UserEditorPlaylists { get; set; }
        public User(string userName, string password)
        {
            Id = Guid.NewGuid();
            UserName = userName;
            Password = password;
        }
    }
}