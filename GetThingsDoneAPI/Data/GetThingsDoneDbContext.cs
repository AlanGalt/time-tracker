using GetThingsDoneAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace GetThingsDoneAPI.Data
{
    public class GetThingsDoneDbContext : DbContext
    {
        public GetThingsDoneDbContext(DbContextOptions<GetThingsDoneDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }
    }
}
