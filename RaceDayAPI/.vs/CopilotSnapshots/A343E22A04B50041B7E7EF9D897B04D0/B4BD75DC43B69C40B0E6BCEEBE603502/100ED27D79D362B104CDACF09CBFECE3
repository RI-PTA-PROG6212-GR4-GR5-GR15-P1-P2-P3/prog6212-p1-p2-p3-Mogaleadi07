namespace RaceDayAPI.Models
{
    public class Event
    {
        // automatic properties
        public int EventID { get; set; }
        public int OrganiserID { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Location { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public decimal Distance { get; set; }
        public string EventType { get; set; } = string.Empty;  // Walk | Run | Cycle
        public DateTime CreatedDate { get; set; }

        public EventOrganiser Organiser { get; set; } = null!;
        public EventRoute? Route { get; set; }
        public ICollection<EventCategory> Categories { get; set; } = new List<EventCategory>();
        public ICollection<EventEnrolment> Enrolments { get; set; } = new List<EventEnrolment>();
    }
}
