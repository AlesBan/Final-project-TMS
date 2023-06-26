using System;

namespace Playlist_for_party.Exceptions.UserExceptions
{
    public class InvalidSingUpUserNameException:Exception
    {
        public InvalidSingUpUserNameException()
        {
            
        }
        public InvalidSingUpUserNameException(string message) : base(message)
        {
            
        }
    }
}