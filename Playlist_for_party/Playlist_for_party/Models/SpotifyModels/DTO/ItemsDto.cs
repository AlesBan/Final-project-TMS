using System.Collections;
using System.Collections.Generic;

namespace Playlist_for_party.Models.SpotifyModels.DTO
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