namespace RaceDayAPI.Models.DTOs
{
    public class UpdateProfileDto
    {
        public required string UserName { get; set; }
        public required string EmailAddress { get; set; }
        public required string ContactNumber { get; set; }
    }
}
