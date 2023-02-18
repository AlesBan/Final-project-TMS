using System;

namespace Playlist_for_party.Exceptions.AppExceptions.DataExceptions
{
    public class DataBaseConnectionException : Exception
    {
        public DataBaseConnectionException()
        {
        }

        public DataBaseConnectionException(string message) : base(message)
        {
        }
    }
}