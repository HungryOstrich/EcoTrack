using EcoTrack.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcoTrack.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<EmissionSource> EmissionSources { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<EmissionFactor> EmissionFactors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

          
            builder.Entity<Organization>()
                .HasMany(o => o.Users)
                .WithMany(u => u.Organizations)
                .UsingEntity(j => j.ToTable("OrganizationUsers"));

           

            builder.Entity<EmissionSource>()
                .HasOne(e => e.Organization)
                .WithMany(o => o.EmissionSources)
                .HasForeignKey(e => e.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade); // Usunięcie organizacji usuwa źródła

            builder.Entity<ActivityLog>()
                .HasOne(a => a.EmissionSource)
                .WithMany(e => e.ActivityLogs)
                .HasForeignKey(a => a.EmissionSourceId)
                .OnDelete(DeleteBehavior.Cascade); // Usunięcie źródła usuwa logi
        }
    }
}
