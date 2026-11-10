using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using RaceDayAPI.Models.DTOs;
using RaceDayAPI.Services; // <-- Required for BlobService
using System.Security.Claims;

namespace RaceDayAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly BlobService _blobService; // <-- ADDED

        // <-- INJECTED BlobService HERE
        public EventsController(AppDbContext db, BlobService blobService)
        {
            _db = db;
            _blobService = blobService;
        }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // GET: /api/events  (Both roles)
        [HttpGet]
        public IActionResult GetAll()
        {
            var events = _db.Events
                .Include(e => e.Organiser).ThenInclude(o => o.User)
                .Select(e => new EventSummaryDto
                {
                    EventID = e.EventID,
                    EventName = e.EventName,
                    Location = e.Location,
                    EventDate = e.EventDate,
                    Distance = e.Distance,
                    EventType = e.EventType,
                    OrganiserName = e.Organiser.User.UserName,
                    BannerUrl = e.BannerUrl // <-- ADDED
                })
                .ToList();

            return Ok(events);
        }

        // GET: /api/events/organiser  (Organiser only)
        [Authorize(Roles = "Organiser")]
        [HttpGet("organiser")]
        public IActionResult GetOrganiserEvents()
        {
            var organiser = _db.EventOrganisers.FirstOrDefault(o => o.UserID == CurrentUserId);
            if (organiser is null) return Forbid();

            var events = _db.Events
                .Where(e => e.OrganiserID == organiser.OrganiserID)
                .Include(e => e.Enrolments)
                .Select(e => new EventSummaryDto
                {
                    EventID = e.EventID,
                    EventName = e.EventName,
                    Location = e.Location,
                    EventDate = e.EventDate,
                    Distance = e.Distance,
                    EventType = e.EventType,
                    OrganiserName = e.Organiser.User.UserName,
                    TotalEnrolments = e.Enrolments.Count,
                    BannerUrl = e.BannerUrl // <-- ADDED
                })
                .ToList();

            return Ok(events);
        }

        // GET: /api/events/{id}  (Both roles)
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var ev = _db.Events
                .Include(e => e.Categories)
                .Include(e => e.Route)
                .Include(e => e.Organiser).ThenInclude(o => o.User)
                .Select(e => new EventDetailDto
                {
                    EventID = e.EventID,
                    EventName = e.EventName,
                    Description = e.Description,
                    Location = e.Location,
                    EventDate = e.EventDate,
                    Distance = e.Distance,
                    EventType = e.EventType,
                    OrganiserID = e.OrganiserID,
                    OrganiserName = e.Organiser.User.UserName,
                    BannerUrl = e.BannerUrl, // <-- ADDED
                    Categories = e.Categories.Select(c => new CategoryDto
                    {
                        CategoryID = c.CategoryID,
                        CategoryName = c.CategoryName,
                        AgeGroup = c.AgeGroup,
                        DistanceType = c.DistanceType
                    }).ToList()
                })
                .FirstOrDefault(e => e.EventID == id);

            if (ev is null) return NotFound();
            return Ok(ev);
        }

        // POST: /api/events  (Organiser only)
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<IActionResult> AddEvent([FromForm] AddEventDto dto, IFormFile? image) // <-- CHANGED TO [FromForm] and IFormFile
        {
            var organiser = _db.EventOrganisers
                .FirstOrDefault(o => o.UserID == CurrentUserId);
            if (organiser is null) return Forbid();

            // Upload image if provided
            string bannerUrl = string.Empty;
            bool imageWasReceived = false;
            string fileName = "None";
            if (image != null && image.Length > 0)
            {
                bannerUrl = await _blobService.UploadFileAsync(image);
            }

            var ev = new Event
            {
                OrganiserID = organiser.OrganiserID,
                EventName = dto.EventName,
                Description = dto.Description,
                Location = dto.Location,
                EventDate = dto.EventDate,
                Distance = dto.Distance,
                EventType = dto.EventType,
                CreatedDate = DateTime.Now,
                BannerUrl = bannerUrl // <-- ADDED
            };
            _db.Events.Add(ev);
            _db.SaveChanges();

            return Ok(new
            {
                message = "Event created successfully",
                eventId = ev.EventID           // Did Azurite give us a URL?
            });
        }

        // PUT: /api/events/{id}  (Organiser only)
        [Authorize(Roles = "Organiser")]
        [HttpPut("{id}")]
        public IActionResult UpdateEvent(int id, AddEventDto dto)
        {
            var ev = _db.Events.Find(id);
            if (ev is null) return NotFound();

            var organiser = _db.EventOrganisers
                .FirstOrDefault(o => o.UserID == CurrentUserId);
            if (organiser is null || ev.OrganiserID != organiser.OrganiserID)
                return Forbid();

            ev.EventName = dto.EventName;
            ev.Description = dto.Description;
            ev.Location = dto.Location;
            ev.EventDate = dto.EventDate;
            ev.Distance = dto.Distance;
            ev.EventType = dto.EventType;
            _db.SaveChanges();

            return Ok(new { message = "Event updated successfully" });
        }

        // DELETE: /api/events/{id}  (Organiser only)
        [Authorize(Roles = "Organiser")]
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var ev = _db.Events.Find(id);
            if (ev is null) return NotFound();

            var organiser = _db.EventOrganisers
                .FirstOrDefault(o => o.UserID == CurrentUserId);
            if (organiser is null || ev.OrganiserID != organiser.OrganiserID)
                return Forbid();

            _db.Events.Remove(ev);
            _db.SaveChanges();
            return Ok(new { message = "Event deleted." });
        }
    }
}