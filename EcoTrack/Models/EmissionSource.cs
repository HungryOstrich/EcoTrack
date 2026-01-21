using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoTrack.Models
{
    public enum SourceType
    {
        Vehicle,    // Flota
        Building,   // Energia w budynkach
        Production  // Procesy
    }

    public class EmissionSource
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } // np. "Toyota Corolla WA12345"

        [Required]
        public SourceType Type { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Klucz obcy do Organizacji
        public int OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        // Relacja: Jeden-do-wielu z ActivityLog
        public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    }
}