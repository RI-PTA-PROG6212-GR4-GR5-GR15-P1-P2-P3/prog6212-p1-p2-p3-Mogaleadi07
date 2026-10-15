using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using RaceDayAPI.Models.DTOs;
using System.Security.Claims;

namespace RaceDayAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public EventsController(AppDbContext db) { _db = db; }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // GET: /api/events  (Both roles)
        [HttpGet]
        public IActionResult GetAll()
        {
            var events = _db.Events
                .Include(e => e.Categories)
                .Include(e => e.Route)
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
                .FirstOrDefault(e => e.EventID == id);
            if (ev is null) return NotFound();
            return Ok(ev);
        }

        // POST: /api/events  (Organiser only)
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public IActionResult AddEvent(AddEventDto dto)
        {
            var organiser = _db.EventOrganisers
                .FirstOrDefault(o => o.UserID == CurrentUserId);
            if (organiser is null) return Forbid();

            var ev = new Event
            {
                OrganiserID = organiser.OrganiserID,
                EventName = dto.EventName,
                Description = dto.Description,
                Location = dto.Location,
                EventDate = dto.EventDate,
                Distance = dto.Distance,
                EventType = dto.EventType,
                CreatedDate = DateTime.Now
            };
            _db.Events.Add(ev);
            _db.SaveChanges();
            return Ok(ev);
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
            return Ok(ev);
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
