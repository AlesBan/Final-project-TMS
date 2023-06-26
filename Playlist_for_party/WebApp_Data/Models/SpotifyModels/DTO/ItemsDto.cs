using System.Collections.Generic;

namespace WebApp_Data.Models.SpotifyModels.DTO
{
    public class ItemsDto
    {
        public List<ArtistDto> ArtistsDto { get; set; }
        public List<TrackDto> TracksDto { get; set; }

        public ItemsDto()
        {
            ArtistsDto = new List<ArtistDto>();
            TracksDto = new List<TrackDto>();
        }
    }
}