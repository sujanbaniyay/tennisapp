using System.ComponentModel.DataAnnotations;

namespace Tennis.Web.Models.Domain
{
    public class SchedulesMD
    {
        public int Id { get; set; }
        // Location
        [Required]
        public string? Location { get; set; }
        // Event name
        [Required]
        public string? EventName { get; set; }
        //Date
        [Required]
        public DateTime? Date { get; set; }
        // Coach Id
        public int? CoachId { get; set; }
    }
}
