using System;

namespace Playlist_for_party.Exceptions.AppExceptions
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