using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Playlist_for_party.Models.SpotifyApiConnection
{
    public class AuthResult
    {
        [JsonPropertyName("access_token")] public string AccessToken  { get; set; }
        [JsonPropertyName("token_type")] public string TokenType { get; set; }
        [JsonPropertyName("expires_in")] public int ExpiresIn { get; set; }
    }
}
