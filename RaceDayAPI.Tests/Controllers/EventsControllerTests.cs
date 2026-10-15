using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Controllers;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using RaceDayAPI.Models.DTOs;
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
            var controller = new EventsController(db);
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims));
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
            return controller;
        }

        [Fact]
        public void PostCreateEvent_ShouldCreateEventInDatabase()
        {
            // Arrange
            var db = GetDbContext();
            var controller = CreateController(db, 1);

            var organiser = new EventOrganiser { OrganiserID = 1, UserID = 1 };
            db.EventOrganisers.Add(organiser);
            db.SaveChanges();

            var addEventDto = new AddEventDto
            {
                EventName = "Marathon 2024",
                Location = "Downtown",
                EventDate = DateTime.Now.AddDays(30),
                Distance = 42.2m,
                EventType = "Run"
            };

            // Act
            var result = controller.AddEvent(addEventDto);

            // Assert
            var createdEvent = ((OkObjectResult)result).Value as Event;
            Assert.NotNull(createdEvent);
            Assert.Equal("Marathon 2024", createdEvent.EventName);
            Assert.Equal(1, db.Events.Count());
        }

        [Fact]
        public void GetAllEvents_ShouldReturnAllEvents()
        {
            // Arrange
            var db = GetDbContext();
            var controller = CreateController(db, 1);

            var organiser = new EventOrganiser { OrganiserID = 1, UserID = 1 };
            db.EventOrganisers.Add(organiser);
            db.Events.AddRange(
                new Event { EventID = 1, OrganiserID = 1, EventName = "Event 1", Location = "Loc1", EventDate = DateTime.Now, Distance = 10m, EventType = "Run", CreatedDate = DateTime.Now },
                new Event { EventID = 2, OrganiserID = 1, EventName = "Event 2", Location = "Loc2", EventDate = DateTime.Now, Distance = 20m, EventType = "Walk", CreatedDate = DateTime.Now }
            );
            db.SaveChanges();

            // Act
            var result = controller.GetAll();

            // Assert
            var events = ((OkObjectResult)result).Value as List<Event>;
            Assert.NotNull(events);
            Assert.Equal(2, events.Count);
        }

        [Fact]
        public void GetEventById_ShouldReturnEvent()
        {
            // Arrange
            var db = GetDbContext();
            var controller = CreateController(db, 1);

            var organiser = new EventOrganiser { OrganiserID = 1, UserID = 1 };
            db.EventOrganisers.Add(organiser);
            db.Events.Add(new Event 
            { 
                EventID = 1, OrganiserID = 1, EventName = "Test Event", Location = "Location",
                EventDate = DateTime.Now, Distance = 5m, EventType = "Walk", CreatedDate = DateTime.Now 
            });
            db.SaveChanges();

            // Act
            var result = controller.GetById(1);

            // Assert
            var ev = ((OkObjectResult)result).Value as Event;
            Assert.NotNull(ev);
            Assert.Equal("Test Event", ev.EventName);
        }

        [Fact]
        public void PutUpdateEvent_ShouldUpdateEventInDatabase()
        {
            // Arrange
            var db = GetDbContext();
            var controller = CreateController(db, 1);

            var organiser = new EventOrganiser { OrganiserID = 1, UserID = 1 };
            db.EventOrganisers.Add(organiser);
            db.Events.Add(new Event 
            { 
                EventID = 1, OrganiserID = 1, EventName = "Old Name", Location = "Location",
                EventDate = DateTime.Now, Distance = 5m, EventType = "Walk", CreatedDate = DateTime.Now 
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
            var updatedEvent = ((OkObjectResult)result).Value as Event;
            Assert.NotNull(updatedEvent);
            Assert.Equal("Updated Name", updatedEvent.EventName);
            Assert.Equal("New Location", updatedEvent.Location);

            var eventFromDb = db.Events.FirstOrDefault(e => e.EventID == 1);
            Assert.Equal("Updated Name", eventFromDb.EventName);
        }

        [Fact]
        public void DeleteEvent_ShouldRemoveEventFromDatabase()
        {
            // Arrange
            var db = GetDbContext();
            var controller = CreateController(db, 1);

            var organiser = new EventOrganiser { OrganiserID = 1, UserID = 1 };
            db.EventOrganisers.Add(organiser);
            db.Events.Add(new Event 
            { 
                EventID = 1, OrganiserID = 1, EventName = "Event to Delete", Location = "Location",
                EventDate = DateTime.Now, Distance = 5m, EventType = "Walk", CreatedDate = DateTime.Now 
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

