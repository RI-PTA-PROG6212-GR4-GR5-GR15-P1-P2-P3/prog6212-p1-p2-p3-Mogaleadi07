namespace RaceDayAPI.Models.DTOs
{
    public class ResultDto
    {
        public int ResultID { get; set; }
        public int EnrolmentID { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string ParticipantName { get; set; } = string.Empty;
        public TimeSpan FinishTime { get; set; }
        public int Position { get; set; }
        public DateTime DateOfResults { get; set; }
    }
}
