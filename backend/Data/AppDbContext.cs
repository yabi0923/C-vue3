using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Resident> Residents { get; set; }
        public DbSet<TestMessage> TestMessages { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
    }
}