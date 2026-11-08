using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using RaceDayAPI.Models.DTOs;
using System.Security.Claims;

namespace RaceDayAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly AppDbContext _db;
        public CategoriesController(AppDbContext db) { _db = db; }

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        // GET: /api/categories/event/{eventId}  (Both roles)
        [HttpGet("event/{eventId}")]
        public IActionResult GetForEvent(int eventId)
        {
            var categories = _db.EventCategories
                .Where(c => c.EventID == eventId)
                .ToList();
            return Ok(categories);
        }

        // POST: /api/categories/event/{eventId}  (Organiser only)
        [Authorize(Roles = "Organiser")]
        [HttpPost("event/{eventId}")]
        public IActionResult AddCategory(int eventId, AddCategoryDto dto)
        {
            var ev = _db.Events.Find(eventId);
            if (ev is null) return NotFound("Event not found.");

            var organiser = _db.EventOrganisers
                .FirstOrDefault(o => o.UserID == CurrentUserId);
            if (organiser is null || ev.OrganiserID != organiser.OrganiserID)
                return Forbid();

            if (string.IsNullOrWhiteSpace(dto.AgeGroup) &&
                string.IsNullOrWhiteSpace(dto.DistanceType))
                return BadRequest("Provide an age group or a distance type.");

            var category = new EventCategory
            {
                EventID = eventId,
                CategoryName = dto.CategoryName,
                AgeGroup = dto.AgeGroup,
                DistanceType = dto.DistanceType
            };
            _db.EventCategories.Add(category);
            _db.SaveChanges();
            return Ok(category);
        }

        // DELETE: /api/categories/{id}  (Organiser only)
        [Authorize(Roles = "Organiser")]
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _db.EventCategories.Find(id);
            if (category is null) return NotFound();

            var ev = _db.Events.Find(category.EventID);
            var organiser = _db.EventOrganisers
                .FirstOrDefault(o => o.UserID == CurrentUserId);

            if (ev is null || organiser is null || ev.OrganiserID != organiser.OrganiserID)
                return Forbid();

            _db.EventCategories.Remove(category);
            _db.SaveChanges();
            return Ok(new { message = "Category deleted." });
        }
    }
}
