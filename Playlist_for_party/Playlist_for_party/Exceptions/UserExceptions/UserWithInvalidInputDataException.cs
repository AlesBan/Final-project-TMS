using System;

namespace Playlist_for_party.Exceptions.UserExceptions
{
    public class UserWithInvalidInputDataException : Exception
    {
        public UserWithInvalidInputDataException()
        {
        }

        public UserWithInvalidInputDataException(string message) : base(message)
        {
        }
    }
}