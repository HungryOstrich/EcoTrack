using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoTrack.Models
{
    public enum SourceType
    {
        Vehicle, Building, Production
    }

    public class EmissionSource
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Musisz wybrać typ źródła")]
        public SourceType Type { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization? Organization { get; set; }

        public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
    }
}