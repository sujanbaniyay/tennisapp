namespace Tennis.Web.Models.Domain
{
    public class CoachBioMD
    {
        // Contain coachId and biography
        public int Id { get; set; }
        public int? CoachId { get; set; }
        public string? Biography { get; set; }
    }
}
