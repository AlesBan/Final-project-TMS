using System;

namespace Playlist_for_party.Exceptions
{
    public class BadRequestToSpotifyApiException : Exception
    {
        public BadRequestToSpotifyApiException()
        {
            
        }

        public BadRequestToSpotifyApiException(string message) : base(message)
        {
            
        }
    }
}