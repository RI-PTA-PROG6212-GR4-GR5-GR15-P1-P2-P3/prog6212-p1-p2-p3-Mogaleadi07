namespace RaceDayAPI.Models.DTOs
{
    public class LoginDto
    {
        public required string UserName { get; set; }
        public required string EmailAddress { get; set; }
        public required string ContactNumber { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}
