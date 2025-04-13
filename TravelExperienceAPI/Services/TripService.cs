using System.ComponentModel.DataAnnotations;
using MongoDB.Driver;
using TravelExperienceAPI.Interfaces;
using TravelExperienceAPI.Models.Database;
using TravelExperienceAPI.Models.Requests;
using TravelExperienceAPI.Models.Responses;

namespace TravelExperienceAPI.Services
{
    public class TripService : ITripService
    {
        private readonly IMongoCollection<Trip> _tripCollection;
        private readonly IMongoCollection<Activity> _activitiesCollection;
        private readonly IValidationService _validationService;

        public TripService(IMongoDatabase database, IValidationService validationService)
        {
            _tripCollection = database.GetCollection<Trip>("Trips");
            _activitiesCollection = database.GetCollection<Activity>("Activities");
            _validationService = validationService;
        }

        public async Task<TripResponse> CreateTripAsync(TripRequest request)
        {
            // Basic validation
            var errors = _validationService.Validate(request);
            if (errors.Any())
                throw new ArgumentException(string.Join(" | ", errors));

            // Calculate total cost
            var totalCost = request.Activities.Sum(a => a.Cost);

            TripResponse tripResponse = new TripResponse()
            {
                Activities = new List<Activity>()
            };

            var trip = new Trip
            {
                TripId = Guid.NewGuid().ToString(),
                UserId = request.UserId,
                Title = request.Title,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TotalCost = totalCost
            };

            await _tripCollection.InsertOneAsync(trip);

            tripResponse.Trip = trip;

            foreach (var activity in request.Activities)
            {
                var dbActivity = new Activity
                {
                    ActivityId = Guid.NewGuid().ToString(),
                    TripId = trip.TripId,
                    DestinationId = activity.DestinationId,
                    Duration = activity.Duration,
                    Cost = activity.Cost
                };

                tripResponse.Activities.Add(dbActivity);

                await _activitiesCollection.InsertOneAsync(dbActivity);
            }

            return tripResponse;
        }
    }
}
