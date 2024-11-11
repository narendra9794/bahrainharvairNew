using Bahrin.Harbour.Data.DBCollections;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bahrin.Harbour.Model.ClientModel
{
    public class Client : BaseEntity
    {
        public string ClientName { get; set; }
        public string? Name { get; set; }
       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }
        public string? EmailAddress { get; set; }
        public string? Phone { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string State { get; set; } 
        public string Postcode { get; set; } 
        public string Address { get; set; } 
        public string? ClientProfileImageFileName { get; set; }
        public string? ImageFolderName { get; set; }
        public bool isLoyalityCardGenerated { get; set; }
        public ICollection<Property>? Properties { get; set; }
      
    }
}
