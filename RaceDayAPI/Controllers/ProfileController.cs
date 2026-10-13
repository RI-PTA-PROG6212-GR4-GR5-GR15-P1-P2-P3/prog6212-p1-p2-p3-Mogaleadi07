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
        public class ProfileController : ControllerBase
        {
            private readonly AppDbContext _db;
            public ProfileController(AppDbContext db) { _db = db; }

            private int CurrentUserId =>
                int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // GET: /api/profile
            [HttpGet]
            public IActionResult GetProfile()
            {
                var user = _db.Users.Find(CurrentUserId);
                if (user is null) return NotFound();

                return Ok(new
                {
                    user.UserId,
                    user.UserName,
                    user.EmailAddress,
                    user.ContactNumber,
                    Role = User.FindFirstValue(ClaimTypes.Role)
                });
            }

            // PUT: /api/profile
            [HttpPut]
            public IActionResult UpdateProfile(UpdateProfileDto dto)
            {
                var user = _db.Users.Find(CurrentUserId);
                if (user is null) return NotFound();

                user.UserName = dto.UserName;
                user.EmailAddress = dto.EmailAddress;
                user.ContactNumber = dto.ContactNumber;
                _db.SaveChanges();

                return Ok(new { message = "Profile updated." });
            }
        }
    }

