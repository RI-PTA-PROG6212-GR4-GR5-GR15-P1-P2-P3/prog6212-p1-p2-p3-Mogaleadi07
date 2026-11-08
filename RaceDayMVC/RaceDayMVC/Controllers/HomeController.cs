using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models.DTOs;
using RaceDayMVC.Services;


namespace RaceDayMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _api;

        public HomeController(ApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch the events from the API
            var events = await _api.GetAsync<List<EventSummaryDto>>("api/events");

            // If the API is off or returns null, send an empty list so the page doesn't crash
            return View(events ?? new List<EventSummaryDto>());
        }
    }
}
