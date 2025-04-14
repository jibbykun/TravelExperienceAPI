using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using TravelExperienceAPI.Interfaces;
using TravelExperienceAPI.Services;
using TravelExperienceAPI.Models.Requests;
using TravelExperienceAPI.Models.Database;
using TravelExperienceAPI.Models;

namespace TravelExperienceAPI.Tests.Services
{
    public class TripServiceTests
    {
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly Mock<IMongoCollection<Trip>> _mockTripCollection;
        private readonly Mock<IMongoCollection<Activity>> _mockActivitiesCollection;
        private readonly TripService _tripService;

        public TripServiceTests()
        {
            _mockTripCollection = new Mock<IMongoCollection<Trip>>();
            _mockActivitiesCollection = new Mock<IMongoCollection<Activity>>();

            _mockTripCollection.Setup(x => x.InsertOneAsync(It.IsAny<Trip>(), null, default))
                .Returns(Task.CompletedTask);

            _mockActivitiesCollection.Setup(x => x.InsertOneAsync(It.IsAny<Activity>(), null, default))
                .Returns(Task.CompletedTask);

            _mockDatabase = new Mock<IMongoDatabase>();
            _mockDatabase.Setup(x => x.GetCollection<Trip>("Trips", null))
                .Returns(_mockTripCollection.Object);

            _mockDatabase.Setup(x => x.GetCollection<Activity>("Activities", null))
                .Returns(_mockActivitiesCollection.Object);

            _tripService = new TripService(_mockDatabase.Object, new ValidationService());
        }

        [Fact]
        public async Task CreateTripAsync_ValidRequest_CreatesTripWithCorrectTotalCost()
        {
            // Arrange
            var request = new TripRequest
            {
                UserId = 123,
                Title = "My Europe Trip",
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(7),
                Activities = new List<ActivityRequest>
                {
                    new ActivityRequest { DestinationId = 1, Duration = 2, Cost = 100 },
                    new ActivityRequest { DestinationId = 2, Duration = 1, Cost = 50 }
                }
            };

            // Act
            var result = await _tripService.CreateTripAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(123, result.Trip.UserId);
            Assert.Equal(150, result.Trip.TotalCost);
            Assert.Equal(2, result.Activities.Count);
            _mockTripCollection.Verify(x => x.InsertOneAsync(It.IsAny<Trip>(), null, default), Times.Once);
            _mockActivitiesCollection.Verify(x => x.InsertOneAsync(It.IsAny<Activity>(), null, default), Times.Exactly(2)); // Ensure activities are inserted
        }

        [Fact]
        public async Task CreateTripAsync_NullRequest_ThrowsNullReferenceException()
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<NullReferenceException>(() => _tripService.CreateTripAsync(null));
            Assert.Contains("Object reference not set to an instance", exception.Message); 
        }

        [Fact]
        public async Task CreateTripAsync_MissingTitle_ThrowsArgumentException()
        {
            // Arrange
            var request = new TripRequest
            {
                UserId = 1,
                Title = "",
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2),
                Activities = new List<ActivityRequest>
                {
                    new ActivityRequest { DestinationId = 1, Duration = 1, Cost = 100 }
                }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _tripService.CreateTripAsync(request));
            Assert.Contains("Title is required.", exception.Message);
        }

        [Fact]
        public async Task CreateTripAsync_MissingUserId_ThrowsArgumentException()
        {
            // Arrange
            var request = new TripRequest
            {
                UserId = 0, 
                Title = "Invalid Trip",
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2),
                Activities = new List<ActivityRequest>
                {
                    new ActivityRequest { DestinationId = 1, Duration = 1, Cost = 100 }
                }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _tripService.CreateTripAsync(request));
            Assert.Contains("UserId is required.", exception.Message);
        }

        [Fact]
        public async Task CreateTripAsync_PastStartDate_ThrowsArgumentException()
        {
            // Arrange
            var request = new TripRequest
            {
                UserId = 1,
                Title = "Invalid Trip",
                StartDate = DateTime.Today.AddDays(-1),
                EndDate = DateTime.Today.AddDays(2),
                Activities = new List<ActivityRequest>
                {
                    new ActivityRequest { DestinationId = 1, Duration = 1, Cost = 100 }
                }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _tripService.CreateTripAsync(request));
            Assert.Contains("Start date cannot be in the past.", exception.Message);
        }

        [Fact]
        public async Task CreateTripAsync_PastEndDate_ThrowsArgumentException()
        {
            // Arrange
            var request = new TripRequest
            {
                UserId = 1,
                Title = "Invalid Trip",
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(-1),
                Activities = new List<ActivityRequest>
                {
                    new ActivityRequest { DestinationId = 1, Duration = 1, Cost = 100 }
                }
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _tripService.CreateTripAsync(request));
            Assert.Contains("End date cannot be in the past.", exception.Message);
        }

        [Fact]
        public async Task CreateTripAsync_MissingActivities_ThrowsArgumentException()
        {
            // Arrange
            var request = new TripRequest
            {
                UserId = 1,
                Title = "Invalid Trip",
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(2),
                Activities = new List<ActivityRequest>()
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _tripService.CreateTripAsync(request));
            Assert.Contains("At least one activity is required.", exception.Message);
        }

        [Fact]
        public async Task CreateTripAsync_ValidRequest_CreatesActivitiesInDatabase()
        {
            // Arrange
            var request = new TripRequest
            {
                UserId = 123,
                Title = "Adventure Trip",
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(7),
                Activities = new List<ActivityRequest>
                {
                    new ActivityRequest { DestinationId = 1, Duration = 2, Cost = 100 },
                    new ActivityRequest { DestinationId = 2, Duration = 1, Cost = 50 }
                }
            };

            // Act
            var result = await _tripService.CreateTripAsync(request);

            // Assert
            _mockActivitiesCollection.Verify(x => x.InsertOneAsync(It.IsAny<Activity>(), null, default), Times.Exactly(2));
        }

        [Fact]
        public async Task CreateTripAsync_ValidRequest_CreatesTripInDatabase()
        {
            // Arrange
            var request = new TripRequest
            {
                UserId = 123,
                Title = "Trip to Japan",
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(5),
                Activities = new List<ActivityRequest>
                {
                    new ActivityRequest { DestinationId = 3, Duration = 3, Cost = 300 }
                }
            };

            // Act
            var result = await _tripService.CreateTripAsync(request);

            // Assert
            _mockTripCollection.Verify(x => x.InsertOneAsync(It.IsAny<Trip>(), null, default), Times.Once);
        }
    }
}
