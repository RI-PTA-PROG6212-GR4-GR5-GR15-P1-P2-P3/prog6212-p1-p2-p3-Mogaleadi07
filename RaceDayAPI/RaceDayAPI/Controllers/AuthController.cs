using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;
using RaceDayAPI.Models.DTOs;
using RaceDayAPI.Services;
using System.Security.Claims;

namespace RaceDayAPI.Controllers
{
    [Route("api/[controller]")]
        [ApiController]
        public class AuthController : ControllerBase
        {
            private readonly AppDbContext _db;
            private readonly JwtTokenService _jwt;

            public AuthController(AppDbContext db, JwtTokenService jwt)
            {
                _db = db;
                _jwt = jwt;
            }

            // POST: /api/auth/register
            [HttpPost("register")]
            public IActionResult Register(RegisterDto dto)
            {
                if (_db.Users.Any(u => u.EmailAddress == dto.EmailAddress))
                    return BadRequest("Email already registered.");

                if (dto.Role != "Organiser" && dto.Role != "Participant")
                    return BadRequest("Role must be 'Organiser' or 'Participant'.");

                var user = new User
                {
                    UserName = dto.UserName,
                    EmailAddress = dto.EmailAddress,
                    ContactNumber = dto.ContactNumber,
                    PasswordHash = PasswordService.Hash(dto.Password),
                    CreatedDate = DateTime.Now
                };

                _db.Users.Add(user);
                _db.SaveChanges();

                if (dto.Role == "Organiser")
                    _db.EventOrganisers.Add(new EventOrganiser { UserID = user.UserId });
                else
                    _db.Participants.Add(new Participant { UserID = user.UserId });

                _db.SaveChanges();

                return Ok(new { message = "Registered successfully.", userId = user.UserId });
            }

            // POST: /api/auth/login
            [HttpPost("login")]
            public IActionResult Login(LoginDto dto)
            {
            var user = _db.Users.FirstOrDefault(u => u.EmailAddress == dto.EmailAddress);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password." });

            // Determine Role
            var isOrganiser = _db.EventOrganisers.Any(o => o.UserID == user.UserId);
            var role = isOrganiser ? "Organiser" : "Participant";

            // FIX: Use _jwt and get the tuple
            var (token, expiresAt) = _jwt.GenerateToken(user, role);

            return Ok(new
            {
                user.UserId,
                user.UserName,
                user.EmailAddress,
                Role = role,
                Token = token
            });
            }

            // GET: /api/auth/me  (protected)
            [Authorize]
            [HttpGet("me")]
            public IActionResult Me()
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var role = User.FindFirstValue(ClaimTypes.Role);

                var user = _db.Users.Find(int.Parse(userId!));
                if (user is null) return Unauthorized();

                return Ok(new
                {
                    userId = user.UserId,
                    userName = user.UserName,
                    email = user.EmailAddress,
                    role = role
                });
            }
        }
    }




