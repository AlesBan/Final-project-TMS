using System.Text.Json.Serialization;
using Playlist_for_party.Models.SpotifyModels.Secondary;
using WebApp_Data.Models.SpotifyModels.Secondary;

namespace WebApp_Data.Models.SpotifyModels.Main
{
    public class Search
    {
        [JsonPropertyName("artists")]
        public Artists Artists { get; set; }
        [JsonPropertyName("tracks")]
        public Tracks Tracks { get; set; }
    }
}