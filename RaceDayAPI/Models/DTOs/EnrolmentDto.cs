namespace RaceDayAPI.Models.DTOs
{
    public class EnrolmentDto
    {
        public int EventID { get; set; }
        public int EnrolmentID { get; set; }
        public int ParticipantID { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public string EventName { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public DateTime EnrolmentDate { get; set; }
        public string Status { get; set; }
    }
}
