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
        public List<string> Roles { get; set; }
        public List<Playlist> PlaylistsAsOwner { get; set; }
        public List<Playlist> PlaylistsAsRedactor { get; set; }

        public User()
        {
            PlaylistsAsOwner = new List<Playlist>();
            PlaylistsAsRedactor = new List<Playlist>();
            Roles = new List<string>();
        }
        
        public void AddPlaylistAsOwner(Playlist playlist)
        {
            PlaylistsAsOwner.Add(playlist);
        }
        
        public void AddPlaylistAsRedactor(Playlist playlist)
        {
            PlaylistsAsRedactor.Add(playlist);
        }


        
        public bool IsOwner(Playlist playlist)
        {
            return playlist.Owner == this;
        }
        
        public bool IsRedactor(Playlist playlist)
        {
            return playlist.Redactors.Contains(this);
        }
    }
}