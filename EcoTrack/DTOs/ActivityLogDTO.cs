using EcoTrack.Models;

namespace EcoTrack.DTOs
{
    public class ActivityLogDTO
    {
        public int Id { get; set; }
        public string DateFormatted { get; set; } // Data jako string (np. "2023-10-15")
        public double Quantity { get; set; }
        public double CalculatedCo2 { get; set; }

        // Informacje o źródle
        public int EmissionSourceId { get; set; }
        public string SourceName { get; set; }

        // Informacje o czynniku (paliwie)
        public int EmissionFactorId { get; set; }
        public string FactorName { get; set; }
        public string Unit { get; set; }

        public ActivityLogDTO() { }

        public ActivityLogDTO(ActivityLog log)
        {
            Id = log.Id;
            DateFormatted = log.Date.ToShortDateString();
            Quantity = log.Quantity;
            CalculatedCo2 = log.CalculatedCo2Emission;

            EmissionSourceId = log.EmissionSourceId;
            EmissionFactorId = log.EmissionFactorId;

            // Płaskie mapowanie nazw (bezpieczne dla nulli)
            SourceName = log.EmissionSource?.Name ?? "Nieznane źródło";
            FactorName = log.EmissionFactor?.Name ?? "Nieznany czynnik";
            Unit = log.EmissionFactor?.Unit ?? "-";
        }
    }
}