using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoTrack.Models
{
    public class ActivityLog : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Zużycie musi być większe od 0.")]
        public double Quantity { get; set; } // Ilość zużytego zasobu (np. litry paliwa)

        // Przechowujemy obliczoną emisję w bazie, aby zmiany współczynników w przyszłości 
        // nie zmieniały danych historycznych (snapshot).
        public double CalculatedCo2Emission { get; private set; }

        // Powiązanie ze źródłem
        public int EmissionSourceId { get; set; }
        [ForeignKey("EmissionSourceId")]
        public virtual EmissionSource? EmissionSource { get; set; }

        // Powiązanie z użytym współczynnikiem
        public int EmissionFactorId { get; set; }
        [ForeignKey("EmissionFactorId")]
        public virtual EmissionFactor? EmissionFactor { get; set; }

        // Metoda biznesowa do przeliczania emisji
        public void CalculateEmission()
        {
            if (EmissionFactor != null)
            {
                this.CalculatedCo2Emission = this.Quantity * this.EmissionFactor.Co2EquivalentPerUnit;
            }
        }

        // --- Logika Walidacji Biznesowej (IValidatableObject) ---
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // 1. Walidacja daty (nie może być z przyszłości)
            if (Date > DateTime.Now)
            {
                yield return new ValidationResult(
                    "Data aktywności nie może być z przyszłości.",
                    new[] { nameof(Date) });
            }

            // 2. Blokada wpisów wstecznych (np. starszych niż 30 dni) - zgodnie z wymogami projektu
            // Symulacja zamkniętego okresu rozliczeniowego
            if (Date < DateTime.Now.AddDays(-30))
            {
                yield return new ValidationResult(
                    "Nie można dodawać ani edytować wpisów starszych niż 30 dni (okres zamknięty).",
                    new[] { nameof(Date) });
            }

            // 3. Sprawdzenie spójności jednostek (uproszczone)
            // W pełnej implementacji sprawdzalibyśmy czy jednostka Source pasuje do Factor
        }
    }
}