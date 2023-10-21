using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BandManagerPWA.DataAccess.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Event> Events { get; set; }

        // override save to set created and updated dates
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void UpdateTimestamps()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity || e.Entity is ApplicationUser)
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.Entity is BaseEntity baseEntity)
                {
                    baseEntity.UpdatedDate = DateTime.UtcNow;

                    if (entityEntry.State == EntityState.Added)
                    {
                        baseEntity.CreatedDate = DateTime.UtcNow;
                    }
                }
                else if (entityEntry.Entity is ApplicationUser applicationUser)
                {
                    applicationUser.UpdatedDate = DateTime.UtcNow;

                    if (entityEntry.State == EntityState.Added)
                    {
                        applicationUser.CreatedDate = DateTime.UtcNow;
                    }
                }
            }

        }
    }
}
