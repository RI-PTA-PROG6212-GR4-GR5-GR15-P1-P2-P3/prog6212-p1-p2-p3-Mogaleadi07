using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RaceDayAPI.Controllers;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using RaceDayAPI.Models.DTOs;
using RaceDayAPI.Services;
using System.Security.Claims;
using Xunit;

namespace RaceDayAPI.Tests.Controllers
{
    public class EventsControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private EventsController CreateController(AppDbContext db, int userId)
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:AzureStorage"] = "UseDevelopmentStorage=true"
                })
                .Build();

            var blobService = new BlobService(config);
            var controller = new EventsController(db, blobService);
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
            return controller;
        }

        private EventOrganiser SeedOrganiser(AppDbContext db, int userId = 1)
        {
            var user = new User
            {
                UserId = userId,
                UserName = "TestUser",
                EmailAddress = "test@example.com",
                ContactNumber = "1234567890",
                PasswordHash = "hash"
            };

            var organiser = new EventOrganiser
            {
                OrganiserID = 1,
                UserID = userId,
                User = user
            };

            db.Users.Add(user);
            db.EventOrganisers.Add(organiser);
            db.SaveChanges();
            return organiser;
        }

        [Fact]
        public async Task PostCreateEvent_ShouldCreateEventInDatabase()
        {
            // Arrange
            var db = GetDbContext();
            var controller = CreateController(db, 1);
            SeedOrganiser(db, 1);

            var addEventDto = new AddEventDto
            {
                EventName = "Marathon 2024",
                Location = "Downtown",
                EventDate = DateTime.Now.AddDays(30),
                Distance = 42.2m,
                EventType = "Run"
            };

            // Act
            var result = await controller.AddEvent(addEventDto, null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            Assert.Equal(1, db.Events.Count());
        }

        [Fact]
        public void GetAllEvents_ShouldReturnAllEvents()
        {
            // Arrange
            var db = GetDbContext();
            var controller = CreateController(db, 1);
            var organiser = SeedOrganiser(db, 1);

            db.Events.AddRange(
                new Event { OrganiserID = organiser.OrganiserID, Organiser = organiser, EventName = "Event 1", Location = "Loc1", EventDate = DateTime.Now, Distance = 10m, EventType = "Run", CreatedDate = DateTime.Now },
                new Event { OrganiserID = organiser.OrganiserID, Organiser = organiser, EventName = "Event 2", Location = "Loc2", EventDate = DateTime.Now, Distance = 20m, EventType = "Walk", CreatedDate = DateTime.Now }
            );
            db.SaveChanges();

            // Act
            var result = controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var events = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Equal(2, events.Count());
        }

        [Fact]
        public void GetEventById_ShouldReturnEvent()
        {
            // Arrange
            var db = GetDbContext();
            var controller = CreateController(db, 1);
            var organiser = SeedOrganiser(db, 1);

            db.Events.Add(new Event
            {
                OrganiserID = organiser.OrganiserID,
                Organiser = organiser,
                EventName = "Test Event",
                Location = "Location",
                EventDate = DateTime.Now,
                Distance = 5m,
                EventType = "Walk",
                CreatedDate = DateTime.Now
            });
            db.SaveChanges();

            // Act
            var result = controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void PutUpdateEvent_ShouldUpdateEventInDatabase()
        {
            // Arrange
            var db = GetDbContext();
            var controller = CreateController(db, 1);
            var organiser = SeedOrganiser(db, 1);

            db.Events.Add(new Event
            {
                OrganiserID = organiser.OrganiserID,
                Organiser = organiser,
                EventName = "Old Name",
                Location = "Location",
                EventDate = DateTime.Now,
                Distance = 5m,
                EventType = "Walk",
                CreatedDate = DateTime.Now
            });
            db.SaveChanges();

            var updateDto = new AddEventDto
            {
                EventName = "Updated Name",
                Location = "New Location",
                EventDate = DateTime.Now.AddDays(10),
                Distance = 10m,
                EventType = "Run"
            };

            // Act
            var result = controller.UpdateEvent(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult);

            var eventFromDb = db.Events.FirstOrDefault(e => e.EventID == 1);
            Assert.NotNull(eventFromDb);
            Assert.Equal("Updated Name", eventFromDb.EventName);
            Assert.Equal("New Location", eventFromDb.Location);
        }

        [Fact]
        public void DeleteEvent_ShouldRemoveEventFromDatabase()
        {
            // Arrange
            var db = GetDbContext();
            var controller = CreateController(db, 1);
            var organiser = SeedOrganiser(db, 1);

            db.Events.Add(new Event
            {
                OrganiserID = organiser.OrganiserID,
                Organiser = organiser,
                EventName = "Event to Delete",
                Location = "Location",
                EventDate = DateTime.Now,
                Distance = 5m,
                EventType = "Walk",
                CreatedDate = DateTime.Now
            });
            db.SaveChanges();

            // Act
            var result = controller.DeleteEvent(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Null(db.Events.FirstOrDefault(e => e.EventID == 1));
        }
    }
}
