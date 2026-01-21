using EcoTrack.Models;

namespace EcoTrack.DTOs
{
    public class EmissionSourceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; } // Enum zamieniony na string dla czytelności w JSON/HTML

        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }

        public EmissionSourceDTO() { }

        public EmissionSourceDTO(EmissionSource source)
        {
            Id = source.Id;
            Name = source.Name;
            Description = source.Description;
            Type = source.Type.ToString(); // Konwersja Enuma na tekst

            OrganizationId = source.OrganizationId;

            // Mapowanie nazwy organizacji (jeśli załadowana)
            if (source.Organization != null)
            {
                OrganizationName = source.Organization.Name;
            }
        }
    }
}