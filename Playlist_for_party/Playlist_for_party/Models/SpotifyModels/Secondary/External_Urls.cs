using System.Text.Json.Serialization;

namespace Playlist_for_party.Models.SpotifyModels.Secondary
{
    public class ExternalUrls
    {
        [JsonPropertyName("spotify")] public string Spotify { get; set; }
    }
}