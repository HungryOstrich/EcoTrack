using System.ComponentModel.DataAnnotations;

namespace EcoTrack.Models
{
    public class Organization
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa organizacji jest wymagana.")]
        [StringLength(100, ErrorMessage = "Nazwa nie może przekraczać 100 znaków.")]
        public string Name { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        public string TaxId { get; set; } // NIP

        // Relacja: Jeden-do-wielu z EmissionSource
        public virtual ICollection<EmissionSource> EmissionSources { get; set; } = new List<EmissionSource>();

        // Relacja: Wiele-do-wielu z AppUser
        public virtual ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    }
}