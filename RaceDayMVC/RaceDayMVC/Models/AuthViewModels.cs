using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{

    public class LoginViewModel
    {
        [Required, EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        public string ContactNumber { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Participant"; // "Participant" or "Organiser"
    }

    // To catch the response from the API
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }

}
