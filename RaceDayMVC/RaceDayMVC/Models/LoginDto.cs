using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models.DTOs
{
    public class LoginDto
    {
        [Required]
        public required string EmailAddress { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
