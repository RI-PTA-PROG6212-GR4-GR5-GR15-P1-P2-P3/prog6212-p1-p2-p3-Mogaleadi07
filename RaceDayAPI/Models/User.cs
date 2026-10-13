namespace RaceDayAPI.Models
{
    public class User
    {
        // automatic properties
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }

        // Navigation
        public EventOrganiser? EventOrganiser { get; set; }
        public Participant? Participant { get; set; }
    }
}
