using System.Text.Json.Serialization;

namespace Playlist_for_party.Models.SpotifyModels.Secondary
{
    public class Image
    {
        [JsonPropertyName("height")] public int Height { get; set; }
        [JsonPropertyName("url")] public string Url { get; set; }
        [JsonPropertyName("width")] public int Width { get; set; }
    }
}