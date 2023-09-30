using Microsoft.EntityFrameworkCore;

namespace BandManagerPWA.DataAccess.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.GetProperties();

                foreach (var property in properties)
                {
                    if (property.ClrType == typeof(Guid))
                    {
                        modelBuilder.Entity(entityType.Name)
                                    .Property(property.Name)
                                    .ValueGeneratedOnAdd();
                    }
                }
            }
        }

    }
}
