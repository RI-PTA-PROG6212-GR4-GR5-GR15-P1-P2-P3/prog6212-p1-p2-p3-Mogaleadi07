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
    public class ResultsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ResultsController(AppDbContext db) { _db = db; }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // POST: /api/results  (Organiser only)
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public IActionResult AddResult(AddResultDto dto)
        {
            var enrolment = _db.EventEnrolments
                .Include(e => e.Event)
                .FirstOrDefault(e => e.EnrolmentID == dto.EnrolmentID);
            if (enrolment is null) return NotFound("Enrolment not found.");

            var organiser = _db.EventOrganisers
                .FirstOrDefault(o => o.UserID == CurrentUserId);
            if (organiser is null ||
                enrolment.Event!.OrganiserID != organiser.OrganiserID)
                return Forbid();

            if (_db.Results.Any(r => r.EnrolmentID == dto.EnrolmentID))
                return BadRequest("Result already captured.");

            var result = new Result
            {
                EnrolmentID = dto.EnrolmentID,
                FinishTime = dto.FinishTime,
                Position = dto.Position
            };
            _db.Results.Add(result);
            _db.SaveChanges();
            return Ok(result);
        }

        // GET: /api/results/my  (Participant only)
        [Authorize(Roles = "Participant")]
        [HttpGet("my")]
        public IActionResult MyResults()
        {
            var participant = _db.Participants
                .FirstOrDefault(p => p.UserID == CurrentUserId);
            if (participant is null) return Forbid();

            var results = _db.Results
                .Include(r => r.Enrolment).ThenInclude(e => e!.Event)
                .Where(r => r.Enrolment!.ParticipantID == participant.ParticipantID)
                .ToList();
            return Ok(results);
        }

        // GET: /api/results/event/{eventId}  (Organiser only)
        [Authorize(Roles = "Organiser")]
        [HttpGet("event/{eventId}")]
        public IActionResult ResultsForEvent(int eventId)
        {
            var organiser = _db.EventOrganisers
                .FirstOrDefault(o => o.UserID == CurrentUserId);
            if (organiser is null) return Forbid();

            var ev = _db.Events.Find(eventId);
            if (ev is null) return NotFound();
            if (ev.OrganiserID != organiser.OrganiserID) return Forbid();

            var results = _db.Results
                .Include(r => r.Enrolment).ThenInclude(e => e!.Participant).ThenInclude(p => p!.User)
                .Include(r => r.Enrolment).ThenInclude(e => e!.Category)
                .Where(r => r.Enrolment!.EventID == eventId)
                .ToList();
            return Ok(results);
        }
    }
}
