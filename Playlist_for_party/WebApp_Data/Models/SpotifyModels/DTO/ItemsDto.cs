using System.Collections.Generic;

namespace WebApp_Data.Models.SpotifyModels.DTO
{
    public class ItemsDto
    {
        public List<ArtistDto> ArtistDtos { get; set; }
        public List<TrackDto> TrackDtos { get; set; }

        public ItemsDto()
        {
            ArtistDtos = new List<ArtistDto>();
            TrackDtos = new List<TrackDto>();
        }
    }
}