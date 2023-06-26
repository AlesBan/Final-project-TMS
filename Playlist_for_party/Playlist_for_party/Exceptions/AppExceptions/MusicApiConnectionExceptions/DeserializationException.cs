using System;

namespace Playlist_for_party.Exceptions.AppExceptions.MusicApiConnectionExceptions
{
    public class DeserializationOfSpotifyModelException : Exception
    {
        public DeserializationOfSpotifyModelException()
        {
        }

        public DeserializationOfSpotifyModelException(string message) : base(message)
        {
        }
    }
}