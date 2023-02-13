using System;

namespace Playlist_for_party.Exceptions.AppExceptions.MusicPartsExceptions.NotFoundExceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message) : base(message)
        {
        }
    }
}