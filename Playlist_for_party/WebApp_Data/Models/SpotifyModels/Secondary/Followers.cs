using System.Text.Json.Serialization;

namespace WebApp_Data.Models.SpotifyModels.Secondary
{
    public class Followers
    {
        [JsonPropertyName("href")] public object Href { get; set; }
        [JsonPropertyName("total")] public int Total { get; set; }
    }
}