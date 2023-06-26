using System;

namespace Playlist_for_party.Exceptions.UserExceptions
{
    public class InvalidPasswordLengthException:Exception
    {
        public InvalidPasswordLengthException()
        {
            
        }
        public InvalidPasswordLengthException(string message) : base(message)
        {
            
        }
    }
}