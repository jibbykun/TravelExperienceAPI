using TravelExperienceAPI.Models.Requests;

namespace TravelExperienceAPI.Interfaces
{
    public interface IValidationService
    {
        List<string> Validate(TripRequest request);
    }
}
