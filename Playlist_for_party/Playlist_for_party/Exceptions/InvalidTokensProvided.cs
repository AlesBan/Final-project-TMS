using System;

namespace Playlist_for_party.Exceptions
{
    public class InvalidTokensProvided : Exception
    {
        public InvalidTokensProvided()
        {
        }

        public InvalidTokensProvided(string message) : base(message)
        {
        }
    }
}