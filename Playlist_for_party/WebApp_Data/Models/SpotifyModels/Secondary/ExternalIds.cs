using System.Text.Json.Serialization;

namespace WebApp_Data.Models.SpotifyModels.Secondary
{
    public class External_Ids
    {
        [JsonPropertyName("isrc")] public string Isrc { get; set; }
    }
}