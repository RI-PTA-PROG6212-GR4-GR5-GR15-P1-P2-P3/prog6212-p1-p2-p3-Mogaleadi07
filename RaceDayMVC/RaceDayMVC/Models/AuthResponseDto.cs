using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models.DTOs
{
    public class AuthResponseDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
        [Required]
        public DateTime ExpiresAt { get; set; }
    }
}
