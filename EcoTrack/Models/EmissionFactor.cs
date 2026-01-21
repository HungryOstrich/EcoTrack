using System.ComponentModel.DataAnnotations;

namespace EcoTrack.Models
{
    public class EmissionFactor
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } // np. "Diesel B7", "Energia elektryczna - Mix PL"

        [Required]
        public string Unit { get; set; } // np. "litry", "kWh"

        [Required]
        [Range(0.0001, double.MaxValue, ErrorMessage = "Współczynnik musi być dodatni.")]
        public double Co2EquivalentPerUnit { get; set; } // Wartość przelicznika

        public int Year { get; set; } // Rok obowiązywania współczynnika

        public bool IsActive { get; set; } = true;
    }
}