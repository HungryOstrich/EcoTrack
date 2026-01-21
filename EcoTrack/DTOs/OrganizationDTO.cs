using EcoTrack.Models;

namespace EcoTrack.DTOs
{
    public class OrganizationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TaxId { get; set; }
        public string Address { get; set; }

        // Pola wyliczeniowe (nie mapowane 1:1 z bazy, ale przydatne w widoku)
        public int UsersCount { get; set; }

        public OrganizationDTO() { }

        // Konstruktor mapujący
        public OrganizationDTO(Organization org)
        {
            Id = org.Id;
            Name = org.Name;
            TaxId = org.TaxId;
            Address = org.Address;
            UsersCount = org.Users?.Count ?? 0;
        }
    }
}