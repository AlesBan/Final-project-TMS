using System.Text.Json.Serialization;
using Playlist_for_party.Models.SpotifyApiConnection;
using Playlist_for_party.Models.SpotifyModels.Secondary;

namespace Playlist_for_party.Models.SpotifyModels.Main
{
    public class SearchArtists
    {
        [JsonPropertyName("artists")]
        public Artists artists { get; set; }
    }
}