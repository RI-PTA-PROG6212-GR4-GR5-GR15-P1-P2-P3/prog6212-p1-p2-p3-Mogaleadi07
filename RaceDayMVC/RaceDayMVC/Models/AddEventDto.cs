using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models.DTOs
{
    public class AddEventDto
    {
        [Required]
        public required string EventName { get; set; }
        public string? Description { get; set; }
        [Required]
        public required string Location { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        [Required]
        public decimal Distance { get; set; }
        public required string EventType { get; set; }
    }
}
