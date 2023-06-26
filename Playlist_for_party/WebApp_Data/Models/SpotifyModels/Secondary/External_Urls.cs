using System.Text.Json.Serialization;

namespace WebApp_Data.Models.SpotifyModels.Secondary
{
    public class External_Urls
    {
        [JsonPropertyName("spotify")] public string Spotify { get; set; }
    }
}