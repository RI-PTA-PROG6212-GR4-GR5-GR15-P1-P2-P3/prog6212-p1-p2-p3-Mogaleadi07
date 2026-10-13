namespace RaceDayAPI.Models.DTOs
{
    public class AddResultDto
    {
        public int EnrolmentID { get; set; }
        public TimeSpan FinishTime { get; set; }
        public int Position { get; set; }
    }
}
