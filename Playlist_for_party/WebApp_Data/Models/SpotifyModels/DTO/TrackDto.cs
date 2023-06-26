using System;

namespace WebApp_Data.Models.SpotifyModels.DTO
{
    public class TrackDto
    {
        public string Name { get; set; }
        public string ImageRef { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public string ArtistName { get; set; }
        public int Rating { get; set; }

        public double DurationMs { get; set; }
        public string Duration 
        {
            get
            {
                var t = TimeSpan.FromMilliseconds(DurationMs);
                return $"{t.Minutes:D1}.{t.Seconds:D2}";
            }
        }
    }
}