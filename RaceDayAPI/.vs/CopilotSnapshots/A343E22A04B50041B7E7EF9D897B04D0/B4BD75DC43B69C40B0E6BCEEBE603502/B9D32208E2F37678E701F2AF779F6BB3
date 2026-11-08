using System.ComponentModel.DataAnnotations;

namespace RaceDayAPI.Models
{
    public class Participant
    {
        //automatic properties
        [Key]
        public int ParticipantID { get; set; }
        public int UserID { get; set; }

        public User User { get; set; } = null!;
        public ICollection<EventEnrolment> Enrolments { get; set; } = new List<EventEnrolment>();
    }
}
