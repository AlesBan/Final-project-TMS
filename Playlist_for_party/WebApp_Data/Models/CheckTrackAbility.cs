using System.Text.Json.Serialization;

namespace WebApp_Data.Models
{
    public class CheckTrackAbility
    {
        [JsonPropertyName("exceeding_the_limit")]
        private bool ExceedingTheLimit { get;}

        [JsonPropertyName("track_duplication")]
        private bool TrackDuplication { get;}

        public CheckTrackAbility(bool exceedingTheLimit, bool trackDuplication)
        {
            ExceedingTheLimit = exceedingTheLimit;
            TrackDuplication = trackDuplication;
        }
    }
}