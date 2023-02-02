using System;
using Playlist_for_party.Models.Music;

namespace Playlist_for_party.Models.DbConnections
{
    public class UserEditorPlaylists
    {
        public Guid UserEditorId { get; set; }
        public User UserEditor { get; set; }
        public Guid PlaylistId  { get; set; }
        public Playlist Playlist  { get; set; }
        
    }
}