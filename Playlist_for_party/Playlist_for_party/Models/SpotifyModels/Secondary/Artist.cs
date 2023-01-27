using System.Text.Json.Serialization;
using Playlist_for_party.Models.SpotifyApiConnection;

namespace Playlist_for_party.Models.SpotifyModels.Secondary
{
    public class Artist
    {
        [JsonPropertyName("external_urls")] public External_Urls ExternalUrls { get; set; }
        [JsonPropertyName("href")] public string Href { get; set; }
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("uri")] public string Uri { get; set; }
    }
}