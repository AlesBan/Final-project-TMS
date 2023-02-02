namespace Playlist_for_party.Models.SpotifyApiConnection
{
    public class AuthResult
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}
