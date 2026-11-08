
using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models.DTOs
{
    public class AddCategoryDto
    {
        [Required]
        public required string CategoryName { get; set; }
        public string? AgeGroup { get; set; }
        public string? DistanceType { get; set; }
    }
}
