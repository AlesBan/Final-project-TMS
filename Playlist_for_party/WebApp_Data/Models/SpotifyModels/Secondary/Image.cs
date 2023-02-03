using System.Text.Json.Serialization;

namespace WebApp_Data.Models.SpotifyModels.Secondary
{
    public class Image
    {
        [JsonPropertyName("height")] public int Height { get; set; }
        [JsonPropertyName("url")] public string Url { get; set; }
        [JsonPropertyName("width")] public int Width { get; set; }
    }
}