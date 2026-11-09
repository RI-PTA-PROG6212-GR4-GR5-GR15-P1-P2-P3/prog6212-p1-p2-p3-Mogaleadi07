using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models.DTOs;
using RaceDayMVC.Services;

namespace RaceDayMVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApiClient _api;

        public ParticipantController(ApiClient api)
        {
            _api = api;
        }

        // POST: /Participant/Enrol
        [HttpPost]
        public async Task<IActionResult> Enrol(int eventId, int categoryId)
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
                return RedirectToAction("Login", "Account");

            var dto = new { EventID = eventId, CategoryID = categoryId };
            var response = await _api.PostAsync("api/enrolments", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Successfully enrolled!";
                return RedirectToAction("MyEnrolments");
            }

            // --- READ THE REAL ERROR FROM THE API ---
            var errorMsg = await response.Content.ReadAsStringAsync();
            TempData["Error"] = $"API Error ({(int)response.StatusCode}): {errorMsg}";

            return RedirectToAction("Details", "Events", new { id = eventId });
        }

        // GET: /Participant/MyEnrolments
        public async Task<IActionResult> MyEnrolments()
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
                return RedirectToAction("Login", "Account");

            var enrolments = await _api.GetAsync<List<EnrolmentDto>>("api/enrolments/my");
            return View(enrolments ?? new List<EnrolmentDto>());
        }

        // GET: /Participant/MyResults
        public async Task<IActionResult> MyResults()
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
                return RedirectToAction("Login", "Account");

            var results = await _api.GetAsync<List<ResultDto>>("api/results/my");
            return View(results ?? new List<ResultDto>());
        }
    }
}

