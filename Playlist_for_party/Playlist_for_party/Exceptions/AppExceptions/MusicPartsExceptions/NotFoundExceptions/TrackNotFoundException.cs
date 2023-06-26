using System;

namespace Playlist_for_party.Exceptions.AppExceptions.MusicPartsExceptions.NotFoundExceptions
{
    public class TrackNotFoundException : Exception
    {
        public TrackNotFoundException()
        {
        }

        public TrackNotFoundException(string message) : base(message)
        {
        }
    }
}