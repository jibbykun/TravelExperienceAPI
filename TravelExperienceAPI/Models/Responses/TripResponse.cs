using TravelExperienceAPI.Models.Database;

namespace TravelExperienceAPI.Models.Responses
{
    public class TripResponse
    {
        public Trip Trip { get; set; }
        public List<Activity> Activities { get; set; }
    }
}
