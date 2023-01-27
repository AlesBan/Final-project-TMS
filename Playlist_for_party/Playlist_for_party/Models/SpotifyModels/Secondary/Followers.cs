using System.Text.Json.Serialization;

namespace Playlist_for_party.Models.SpotifyModels.Secondary
{
    public class Followers
    {
        [JsonPropertyName("href")] public object Href { get; set; }
        [JsonPropertyName("total")] public int Total { get; set; }
    }
}