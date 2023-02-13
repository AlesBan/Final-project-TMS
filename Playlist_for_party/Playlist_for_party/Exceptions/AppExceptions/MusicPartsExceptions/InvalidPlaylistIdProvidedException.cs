using System;

namespace Playlist_for_party.Exceptions.AppExceptions.MusicPartsExceptions
{
    public class InvalidPlaylistIdProvidedException:Exception
    {
        public InvalidPlaylistIdProvidedException()
        {
            
        }
        public InvalidPlaylistIdProvidedException(string message) : base(message)
        {
            
        } 
    }
}