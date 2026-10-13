namespace RaceDayAPI.Models
{
    public class Result
    {
        //automatic properties
        public int ResultID { get; set; }
        public int EnrolmentID { get; set; }
        public TimeSpan FinishTime { get; set; }
        public int Position { get; set; }
        public DateTime DateOfResults { get; set; }

        public EventEnrolment Enrolment { get; set; } = null!;

    }
}
