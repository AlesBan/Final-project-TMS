using System.Text.Json.Serialization;
using Playlist_for_party.Models.SpotifyApiConnection;

namespace Playlist_for_party.Models.SpotifyModels.Secondary
{
    public class Item
    {
        [JsonPropertyName("followers")] public Followers Followers { get; set; }
        [JsonPropertyName("tracks")] public Tracks Tracks { get; set; }
        [JsonPropertyName("album")] public Album Album { get; set; }
        [JsonPropertyName("albums")] public Albums Albums { get; set; }
        [JsonPropertyName("album_type")] public string AlbumType { get; set; }
        [JsonPropertyName("artists")] public Artist[] Artists { get; set; }

        [JsonPropertyName("available_markets")]
        public object[] AvailableMarkets { get; set; }

        [JsonPropertyName("disc_number")] public int DiscNumber { get; set; }
        [JsonPropertyName("duration_ms")] public int DurationMs { get; set; }
        [JsonPropertyName("explicit")] public bool Explicity { get; set; }
        [JsonPropertyName("external_ids")] public External_Ids ExternalIds { get; set; }
        [JsonPropertyName("external_urls")] public External_Urls ExternalUrls { get; set; }
        [JsonPropertyName("href")] public string Href { get; set; }
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("is_local")] public bool IsLocal { get; set; }
        [JsonPropertyName("popularity")] public int Popularity { get; set; }
        [JsonPropertyName("preview_url")] public string PreviewUrl { get; set; }
        [JsonPropertyName("track_number")] public int TrackNumber { get; set; }
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