namespace RaceDayAPI.Models.DTOs
{
    public class AddCategoryDto
    {
        public required string CategoryName { get; set; }
        public string? AgeGroup { get; set; }
        public string? DistanceType { get; set; }
    }
}
