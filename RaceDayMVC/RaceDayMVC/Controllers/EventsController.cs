using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models.DTOs;
using RaceDayMVC.Services;

namespace RaceDayMVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApiClient _api;

        public EventsController(ApiClient api)
        {
            _api = api;
        }

        // GET: /Events/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // Fetch the full event details (including categories) from the API
            var ev = await _api.GetAsync<EventDetailDto>($"api/events/{id}");
            if (ev == null) return NotFound();

            return View(ev);
        }

    }
}
