using System;
using WebApp_Data.Models.Music;
using WebApp_Data.Models.UserData;

namespace WebApp_Data.Models.DbConnections
{
    public class UserEditorPlaylist
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid PlaylistId  { get; set; }
        public Playlist Playlist  { get; set; }
    }
}