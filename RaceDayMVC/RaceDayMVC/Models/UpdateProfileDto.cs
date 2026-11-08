using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models.DTOs
{
    public class UpdateProfileDto
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string EmailAddress { get; set; }
        public required string ContactNumber { get; set; }
    }
}
