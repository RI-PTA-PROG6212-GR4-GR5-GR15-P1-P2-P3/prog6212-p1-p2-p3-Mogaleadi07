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

            // Create the payload matching your API's EnrolmentDto
            var dto = new { EventID = eventId, CategoryID = categoryId };

            // Send the POST request to the API
            var response = await _api.PostAsync("api/enrolments", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Successfully enrolled!";
                return RedirectToAction("MyEnrolments");
            }

            TempData["Error"] = "Failed to enrol. You might already be enrolled in this category.";
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

