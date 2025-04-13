using MongoDB.Bson.Serialization.Attributes;

namespace TravelExperienceAPI.Models.Database
{
    public class Activity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string ActivityId { get; set; }
        public string TripId { get; set; }
        public int DestinationId { get; set; }
        public int Duration { get; set; }
        public double Cost { get; set; }
    }
}
