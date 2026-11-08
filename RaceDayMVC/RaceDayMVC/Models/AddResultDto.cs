using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models.DTOs
{
    public class AddResultDto
    {
        [Required]
        public int EnrolmentID { get; set; }
        [Required]
        public TimeSpan FinishTime { get; set; }
        public int Position { get; set; }
    }
}
