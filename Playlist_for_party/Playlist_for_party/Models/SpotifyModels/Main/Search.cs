using System.Text.Json.Serialization;
using Playlist_for_party.Models.SpotifyApiConnection;
using Playlist_for_party.Models.SpotifyModels.Secondary;

namespace Playlist_for_party.Models.SpotifyModels.Main
{
    public class Search
    {
        [JsonPropertyName("artists")]
        public Artists Artists { get; set; }
        [JsonPropertyName("tracks")]
        public Tracks Tracks { get; set; }
    }
}