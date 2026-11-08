using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using RaceDayMVC.Services;
using System.Net.Http.Json;

namespace RaceDayMVC.Controllers
{
    public class AccountController: Controller
    {
        private readonly ApiClient _api;

        public AccountController(ApiClient api)
        {
            _api = api;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Call the API Login endpoint
            var response = await _api.PostAsync("api/auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();

            if (authResponse != null)
            {
                // SAVE TO SESSION!
                HttpContext.Session.SetString("JwtToken", authResponse.Token);
                HttpContext.Session.SetString("UserRole", authResponse.Role);
                HttpContext.Session.SetString("UserName", authResponse.UserName);
                HttpContext.Session.SetInt32("UserId", authResponse.UserId);

                // Redirect based on role
                if (authResponse.Role == "Organiser")
                    return RedirectToAction("Dashboard", "Organiser"); // We will build this in Chunk 4
                else
                    return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var response = await _api.PostAsync("api/auth/register", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Registration failed. Email might be in use.");
                return View(model);
            }

            // Auto-login after registration
            return await Login(new LoginViewModel { EmailAddress = model.EmailAddress, Password = model.Password });
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Destroy session
            return RedirectToAction("Index", "Home");
        }
    }

}

