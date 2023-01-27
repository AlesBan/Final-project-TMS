using System.Text.Json.Serialization;
using Playlist_for_party.Models.SpotifyApiConnection;

namespace Playlist_for_party.Models.SpotifyModels.Secondary
{
    public class Albums
    {
        [JsonPropertyName("href")] public string Href { get; set; }
        [JsonPropertyName("items")] public Item[] Items { get; set; }
        [JsonPropertyName("limit")] public int Limit { get; set; }
        [JsonPropertyName("next")] public string Next { get; set; }
        [JsonPropertyName("offset")] public int Offset { get; set; }
        [JsonPropertyName("previous")] public object Previous { get; set; }
        [JsonPropertyName("total")] public int Total { get; set; }
    }
}