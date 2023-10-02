using Microsoft.EntityFrameworkCore;

namespace Tennis.Web.Models.Domain
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        // Add DbSet properties here
        public DbSet<Person> People { get; set; }
        public DbSet<SchedulesMD> Schedules { get; set; }
        public DbSet<CoachBioMD> CoachBio { get; set; }
        public DbSet<ScheduleAssign> ScheduleAssign { get; set; }
    }
}
