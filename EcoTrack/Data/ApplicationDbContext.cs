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
            public DbSet<EcoTrack.Models.ActivityLog> ActivityLog { get; set; } = default!;
            public DbSet<EcoTrack.Models.EmissionFactor> EmissionFactor { get; set; } = default!;
            public DbSet<EcoTrack.Models.EmissionSource> EmissionSource { get; set; } = default!;
            public DbSet<EcoTrack.Models.Organization> Organization { get; set; } = default!;
        }
    
}
