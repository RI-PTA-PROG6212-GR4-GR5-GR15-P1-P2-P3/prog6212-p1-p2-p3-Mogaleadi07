namespace RaceDayAPI.Models.DTOs
{
    public class AddEventDto
    {
        public required string EventName { get; set; }
        public string? Description { get; set; }
        public required string Location { get; set; }
        public DateTime EventDate { get; set; }
        public decimal Distance { get; set; }
        public required string EventType { get; set; }
    }
}
