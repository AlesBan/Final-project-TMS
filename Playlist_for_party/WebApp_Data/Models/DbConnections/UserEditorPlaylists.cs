using System;
using WebApp_Data.Models.Music;

namespace WebApp_Data.Models.DbConnections
{
    public class UserEditorPlaylists
    {
        public Guid UserEditorId { get; set; }
        public User UserEditor { get; set; }
        public Guid PlaylistId  { get; set; }
        public Playlist Playlist  { get; set; }
        
    }
}