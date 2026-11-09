using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models.DTOs;
using RaceDayMVC.Services;

namespace RaceDayMVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly ApiClient _api;

        public OrganiserController(ApiClient api)
        {
            _api = api;
        }

        private bool IsOrganiser() => HttpContext.Session.GetString("UserRole") == "Organiser";

        // GET: /Organiser/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");

            var events = await _api.GetAsync<List<EventSummaryDto>>("api/events/organiser");
            return View(events ?? new List<EventSummaryDto>());
        }

        // GET: /Organiser/CreateEvent
        [HttpGet]
        public IActionResult CreateEvent() => IsOrganiser() ? View() : RedirectToAction("Login", "Account");

        // POST: /Organiser/CreateEvent
        [HttpPost]
        public async Task<IActionResult> CreateEvent(AddEventDto dto, IFormFile? ImageFile)
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");
            if (!ModelState.IsValid) return View(dto);

            var content = new MultipartFormDataContent();
            content.Add(new StringContent(dto.EventName), "EventName");
            content.Add(new StringContent(dto.Description ?? ""), "Description");
            content.Add(new StringContent(dto.Location), "Location");
            content.Add(new StringContent(dto.EventDate.ToString("yyyy-MM-ddTHH:mm:ss")), "EventDate");
            content.Add(new StringContent(dto.Distance.ToString()), "Distance");
            content.Add(new StringContent(dto.EventType), "EventType");

            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileContent = new StreamContent(ImageFile.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ImageFile.ContentType);
                content.Add(fileContent, "image", ImageFile.FileName);
            }

            var response = await _api.PostFormDataAsync("api/events", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Event created successfully!";
                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "Failed to create event. Please check your inputs and try again.");
            return View(dto);
        }

        // GET: /Organiser/ManageCategories/5
        public async Task<IActionResult> ManageCategories(int id)
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");

            // Fetch the event (includes categories)
            var ev = await _api.GetAsync<EventDetailDto>($"api/events/{id}");
            if (ev == null) return NotFound();

            return View(ev);
        }

        // POST: /Organiser/AddCategory
        [HttpPost]
        public async Task<IActionResult> AddCategory(int eventId, AddCategoryDto dto)
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");

            var response = await _api.PostAsync($"api/eventcategories?eventId={eventId}", dto);
            return RedirectToAction("ManageCategories", new { id = eventId });
        }

        // GET: /Organiser/ViewEnrolments/5
        public async Task<IActionResult> ViewEnrolments(int id)
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");

            var enrolments = await _api.GetAsync<List<EnrolmentDto>>($"api/enrolments/event/{id}");

            // Save event ID in ViewBag for the UI
            ViewBag.EventID = id;
            return View(enrolments ?? new List<EnrolmentDto>());
        }
    }
}
