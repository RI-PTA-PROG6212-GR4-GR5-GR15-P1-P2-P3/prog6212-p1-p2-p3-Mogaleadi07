using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models.DTOs
{
    public class EnrolmentDto
    {
        [Required]
        public int EnrolmentID { get; set; }
        public int ParticipantID { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public int EventID { get; set; }
        [Required]
        public string EventName { get; set; } = string.Empty;

        [Required]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public DateTime EnrolmentDate { get; set;  }
        public string Status { get; set; } = string.Empty;
    }
}
