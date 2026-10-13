namespace RaceDayAPI.Models
{
    public class EventCategory
    {
        // automatic properties
        public int CategoryID { get; set; }
        public int EventID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? AgeGroup { get; set; }
        public string? DistanceType { get; set; }
        public DateTime CreatedDate { get; set; }

        public Event Event { get; set; } = null!;
        public ICollection<EventEnrolment> Enrolments { get; set; } = new List<EventEnrolment>();
    }
}
