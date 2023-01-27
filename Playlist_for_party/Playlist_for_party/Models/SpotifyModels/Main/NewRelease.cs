using System.Text.Json.Serialization;
using Playlist_for_party.Models.SpotifyApiConnection;
using Playlist_for_party.Models.SpotifyModels.Secondary;

namespace Playlist_for_party.Models.SpotifyModels.Main
{
    public class NewRelease
    {
        [JsonPropertyName("albums")]
        public Albums Albums { get; set; }
    }
}