namespace RaceDayAPI.Models.DTOs
{

        public class EventSummaryDto
        {
            public int EventID { get; set; }
            public string EventName { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public DateTime EventDate { get; set; }
            public decimal Distance { get; set; }
            public string EventType { get; set; } = string.Empty;
            public string OrganiserName { get; set; } = string.Empty;
           public string? BannerUrl { get; set; }
           public int TotalEnrolments { get; set; }
    }

        // Used for the single event detail page
        public class EventDetailDto
        {
            public int EventID { get; set; }
            public string EventName { get; set; } = string.Empty;
            public string? Description { get; set; }
            public string Location { get; set; } = string.Empty;
            public DateTime EventDate { get; set; }
            public decimal Distance { get; set; }
            public string EventType { get; set; } = string.Empty;
            public int OrganiserID { get; set; }
            public string OrganiserName { get; set; } = string.Empty;
           public string? BannerUrl { get; set; }
        public int TotalEnrolments { get; set; }
        // We will add Route and Categories later if needed
        public List<CategoryDto> Categories { get; set; } = new();
        }

        // Helper DTO for categories
        public class CategoryDto
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; } = string.Empty;
            public string? AgeGroup { get; set; }
            public string? DistanceType { get; set; }
        }
    }

