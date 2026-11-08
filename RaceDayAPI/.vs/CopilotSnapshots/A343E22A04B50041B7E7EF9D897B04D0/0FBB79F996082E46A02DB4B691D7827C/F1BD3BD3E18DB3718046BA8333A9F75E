namespace RaceDayAPI.Models
{
    public class EventOrganiser
    {
        // automatic properties
        public int OrganiserID { get; set; }
        public int UserID { get; set; }

        public User User { get; set; } = null!;
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
