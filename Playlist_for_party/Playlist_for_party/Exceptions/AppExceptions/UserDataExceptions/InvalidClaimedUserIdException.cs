using System;

namespace Playlist_for_party.Exceptions.AppExceptions
{
    public class InvalidClaimedUserIdException : Exception
    {
        public InvalidClaimedUserIdException()
        {
        }

        public InvalidClaimedUserIdException(string message) : base(message)
        {
        }
    }
}