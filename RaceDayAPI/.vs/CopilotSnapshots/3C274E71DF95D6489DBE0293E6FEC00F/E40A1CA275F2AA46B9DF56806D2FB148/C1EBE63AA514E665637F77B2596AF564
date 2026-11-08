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
    public class EnrolmentsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public EnrolmentsController(AppDbContext db) { _db = db; }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // POST: /api/enrolments  (Participant only)
        [Authorize(Roles = "Participant")]
        [HttpPost]
        public IActionResult Enrol(EnrolmentDto dto)
        {
            var participant = _db.Participants
                .FirstOrDefault(p => p.UserID == CurrentUserId);
            if (participant is null) return Forbid();

            var ev = _db.Events.Find(dto.EventID);
            if (ev is null) return NotFound("Event not found.");

            var category = _db.EventCategories.FirstOrDefault(c =>
                c.CategoryID == dto.CategoryID && c.EventID == dto.EventID);
            if (category is null) return NotFound("Category not found for this event.");

            bool exists = _db.EventEnrolments.Any(e =>
                e.ParticipantID == participant.ParticipantID &&
                e.EventID == dto.EventID &&
                e.CategoryID == dto.CategoryID);
            if (exists) return BadRequest("Already enrolled in this category.");

            var enrolment = new EventEnrolment
            {
                ParticipantID = participant.ParticipantID,
                EventID = dto.EventID,
                CategoryID = dto.CategoryID,
                EnrolmentDate = DateTime.Now,
                Status = "Confirmed"
            };
            _db.EventEnrolments.Add(enrolment);
            _db.SaveChanges();
            return Ok(enrolment);
        }

        // GET: /api/enrolments/my  (Participant only)
        [Authorize(Roles = "Participant")]
        [HttpGet("my")]
        public IActionResult MyEnrolments()
        {
            var participant = _db.Participants
                .FirstOrDefault(p => p.UserID == CurrentUserId);
            if (participant is null) return Forbid();

            var enrolments = _db.EventEnrolments
                .Where(e => e.ParticipantID == participant.ParticipantID)
                .Include(e => e.Event)
                .Include(e => e.Category)
                .ToList();
            return Ok(enrolments);
        }

        // GET: /api/enrolments/event/{eventId}  (Organiser only)
        [Authorize(Roles = "Organiser")]
        [HttpGet("event/{eventId}")]
        public IActionResult EnrolmentsForEvent(int eventId)
        {
            var organiser = _db.EventOrganisers
                .FirstOrDefault(o => o.UserID == CurrentUserId);
            if (organiser is null) return Forbid();

            var ev = _db.Events.Find(eventId);
            if (ev is null) return NotFound();
            if (ev.OrganiserID != organiser.OrganiserID) return Forbid();

            var enrolments = _db.EventEnrolments
                .Where(e => e.EventID == eventId)
                .Include(e => e.Participant).ThenInclude(p => p!.User)
                .Include(e => e.Category)
                .ToList();
            return Ok(enrolments);
        }
    }
}
