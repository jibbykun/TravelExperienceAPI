using TravelExperienceAPI.Interfaces;
using TravelExperienceAPI.Models.Requests;

namespace TravelExperienceAPI.Services
{
    public class ValidationService : IValidationService
    {
        public List<string> Validate(TripRequest request)
        {
            var errors = new List<string>();

            var validations = new (bool Condition, string Message)[]
            {
                (request == null, "Trip request cannot be null."),
                (string.IsNullOrWhiteSpace(request?.Title), "Title is required."),
                (request.UserId == 0, "UserId is required."),
                (request?.StartDate < DateTime.Now, "Start date cannot be in the past."),
                (request?.EndDate < DateTime.Now, "End date cannot be in the past."),
                (request?.Activities == null || !request.Activities.Any(), "At least one activity is required.")
            };

            foreach (var (condition, message) in validations)
            {
                if (condition) errors.Add(message);
            }

            return errors;
        }
    }
}
