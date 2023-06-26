using System;

namespace Playlist_for_party.Exceptions.UserExceptions
{
    public class InvalidUserNameLengthException:Exception
    {
        public InvalidUserNameLengthException()
        {
            
        }
        public InvalidUserNameLengthException(string message) : base(message)
        {
            
        }
    }
}