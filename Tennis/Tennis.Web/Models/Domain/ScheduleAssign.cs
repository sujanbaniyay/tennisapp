namespace Tennis.Web.Models.Domain
{
    public class ScheduleAssign
    {
        public int Id { get; set; }
        // User Id
        public int? UserId { get; set; }
        // Schedule Id
        public int? ScheduleId { get; set; }
    }
}
