using System.Text.Json.Serialization;

namespace WebApp_Data.Models.TrackAbilities
{
    public class CheckTrackAbility
    {
        [JsonPropertyName("exceeding_the_limit")]
        public bool ExceedingTheLimit { get; set; }

        [JsonPropertyName("track_duplication")]
        public bool TrackDuplication { get;set;}
    }
}