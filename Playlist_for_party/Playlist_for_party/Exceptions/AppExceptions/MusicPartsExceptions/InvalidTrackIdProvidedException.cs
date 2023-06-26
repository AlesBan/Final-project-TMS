using System;

namespace Playlist_for_party.Exceptions.AppExceptions.MusicPartsExceptions
{
    public class InvalidTrackIdProvidedException : Exception
    {
        public InvalidTrackIdProvidedException()
        {
            
        }
        public InvalidTrackIdProvidedException(string message) : base(message)
        {
            
        } 
    }
}