using TravelExperienceAPI.Models.Database;

namespace TravelExperienceAPI.Models.Requests
{
    public class TripRequest
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ActivityRequest> Activities { get; set; }
    }
}
