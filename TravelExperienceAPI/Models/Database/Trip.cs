using MongoDB.Bson.Serialization.Attributes;

namespace TravelExperienceAPI.Models.Database
{
    public class Trip
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string TripId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalCost { get; set; }

    }
}
