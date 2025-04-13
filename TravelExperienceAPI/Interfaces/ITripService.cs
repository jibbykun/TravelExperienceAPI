using TravelExperienceAPI.Models;
using TravelExperienceAPI.Models.Requests;
using TravelExperienceAPI.Models.Responses;

namespace TravelExperienceAPI.Interfaces
{
    public interface ITripService
    {
        Task<TripResponse> CreateTripAsync(TripRequest request);
    }
}
