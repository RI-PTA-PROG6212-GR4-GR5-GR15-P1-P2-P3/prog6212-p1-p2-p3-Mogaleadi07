using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models.DTOs
{

        public class EventSummaryDto
        {
        [Required]
            public int EventID { get; set; }

        public string EventName { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public DateTime EventDate { get; set; }
            public decimal Distance { get; set; }
            public string EventType { get; set; } = string.Empty;
            public string OrganiserName { get; set; } = string.Empty;
            public int TotalEnrolments { get; set; }
            public string? BannerUrl { get; set; }

    }

        // Used for the single event detail page
        public class EventDetailDto
        {
        [Required]
            public int EventID { get; set; }
            public string EventName { get; set; } = string.Empty;
            public string? Description { get; set; }
            public string Location { get; set; } = string.Empty;
            public DateTime EventDate { get; set; }
            public decimal Distance { get; set; }
            public string EventType { get; set; } = string.Empty;
            public int OrganiserID { get; set; }
            public string OrganiserName { get; set; } = string.Empty;
            public int TotalEnrolments { get; set; }
            public string? BannerUrl { get; set; }

        // We will add Route and Categories later if needed
        public List<CategoryDto> Categories { get; set; } = new();
        }

        // Helper DTO for categories
        public class CategoryDto
        {
            [Required]
            public int CategoryID { get; set; }
            [Required]
            public string CategoryName { get; set; } = string.Empty;
            public string? AgeGroup { get; set; }
            public string? DistanceType { get; set; }
        }
    }

