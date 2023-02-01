using System;

namespace Playlist_for_party.Exceptions
{
    public class DeserializationException : Exception
    {
        public DeserializationException()
        {
        }

        public DeserializationException(string message) : base(message)
        {
        }
    }
}