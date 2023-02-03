using Playlist_for_party.Models.SpotifyModels.Secondary;
using System.Text.Json.Serialization;
using WebApp_Data.Models.SpotifyModels.Secondary;


namespace Playlist_for_party.Models.SpotifyModels.Secondary
{
    public class Album
    {
        [JsonPropertyName("album_type")] public string AlbumType { get; set; }
        [JsonPropertyName("artists")] public Artist[] Artists { get; set; }

        [JsonPropertyName("available_markets")]
        public string[] AvailableMarkets { get; set; }

        [JsonPropertyName("external_urls")] public External_Urls ExternalUrls { get; set; }
        [JsonPropertyName("href")] public string Href { get; set; }
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("images")] public Image[] Images { get; set; }
        [JsonPropertyName("name")] public string Name { get; set; }
        [JsonPropertyName("release_date")] public string ReleaseDate { get; set; }

        [JsonPropertyName("release_date_precision")]
        public string ReleaseDatePrecision { get; set; }

        [JsonPropertyName("total_tracks")] public int TotalTracks { get; set; }
        [JsonPropertyName("type")] public string Type { get; set; }
        [JsonPropertyName("uri")] public string Uri { get; set; }
    }
}