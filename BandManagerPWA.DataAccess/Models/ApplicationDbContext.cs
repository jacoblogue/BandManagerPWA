using Microsoft.EntityFrameworkCore;

namespace BandManagerPWA.DataAccess.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Event> Events { get; set; }
    }
}
