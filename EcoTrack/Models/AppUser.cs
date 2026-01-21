using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EcoTrack.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relacja: Wiele-do-wielu z Organization
        public virtual ICollection<Organization> Organizations { get; set; } = new List<Organization>();
    }
}
