namespace RaceDayAPI.Models
{
    public class EventRoute
    {
        // automatic properties
        public int RouteID { get; set; }
        public int EventID { get; set; }
        public string? RouteDescription { get; set; }
        public decimal Distance { get; set; }
        public string? Difficulty { get; set; }
        public string? MapURL { get; set; }
        public DateTime CreatedDate { get; set; }

        public Event Event { get; set; } = null!;

    }
}
