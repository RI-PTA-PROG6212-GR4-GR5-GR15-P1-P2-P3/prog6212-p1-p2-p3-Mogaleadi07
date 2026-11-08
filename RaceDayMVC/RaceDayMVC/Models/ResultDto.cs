using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models.DTOs
{
    public class ResultDto
    {
        //automatic properties
        public int ResultID { get; set; }
        public int EnrolmentID { get; set; }
        public string EventName { get; set;  }
        public TimeSpan FinishTime { get; set; }
        public int Position { get; set; }
        public DateTime DateOfResults { get; set; }

    }
}
