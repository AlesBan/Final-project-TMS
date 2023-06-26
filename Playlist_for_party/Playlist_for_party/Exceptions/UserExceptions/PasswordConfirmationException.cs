using System;

namespace Playlist_for_party.Exceptions.UserExceptions
{
    public class PasswordConfirmationException : Exception
    {
        public PasswordConfirmationException()
        {
        }

        public PasswordConfirmationException(string message) : base(message)
        {
        }
    }
}