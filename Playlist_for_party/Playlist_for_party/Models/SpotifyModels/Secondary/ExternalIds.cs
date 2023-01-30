using System.Text.Json.Serialization;

namespace Playlist_for_party.Models.SpotifyModels.Secondary
{
    public class External_Ids
    {
        [JsonPropertyName("isrc")] public string Isrc { get; set; }
    }
}