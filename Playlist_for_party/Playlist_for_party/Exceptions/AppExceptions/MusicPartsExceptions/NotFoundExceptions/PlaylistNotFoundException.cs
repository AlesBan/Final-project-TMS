using System;

namespace Playlist_for_party.Exceptions.AppExceptions.MusicPartsExceptions.NotFoundExceptions
{
    public class PlaylistNotFoundException : Exception
    {
        public PlaylistNotFoundException()
        {
        }

        public PlaylistNotFoundException(string message) : base(message)
        {
        }
    }
}