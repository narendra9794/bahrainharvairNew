using Bahrin.Harbour.Model.VisitHistoryModel;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Bahrin.Harbour.Model.ClientModel
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Client name cannot exceed 100 characters.")]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [Required]
        [Range(100000, 999999, ErrorMessage = "ClientId must be a 6-digit number.")]
        [Display(Name = "Client ID")]
        public int ClientId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Postcode cannot exceed 10 characters.")]  
        [Phone(ErrorMessage = "Invalid phone number format.")]                      
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Phone Number cannot exceed 10 characters.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters.")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        
        [StringLength(10, ErrorMessage = "Postcode cannot exceed 10 characters.")]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Type Of Property cannot exceed 100 characters.")]
        [Display(Name = "Type of Property")]
        public string TypeOfProperty { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Property Location cannot exceed 100 characters.")]
        [Display(Name = "Property Location")]
        public string PropertyLocation { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Property Price must be a positive value.")]
        [Display(Name = "Property Price")]
        public decimal PropertyPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Availed Discount must be a positive value.")]
        [Display(Name = "Availed Discount")]
        public decimal AvailedDiscount { get; set; }

        [StringLength(100, ErrorMessage = "Street cannot exceed 100 characters.")]
        [Display(Name = "Street")]
        public string Street { get; set; }
        public bool Status { get; set; }
        public string? ClientProfileImageLink { get; set; }
        public string? ClientQrCodeImageLink { get; set; }
        public DateTime? LastVisit { get; set; }

        public string? Mode { get; set; }
        public Guid? CurrentVisitId { get; set; }
        public IFormFile? ImageFile { get; set; }
        public List<PropertyViewModel>? Properties { get; set; }    
        public int? PropertyCount { get; set; }    
        public List<VisitHistoryView>? VisitHistory{ get; set; }    
  }
    public class PropertyViewModel
    {
        public Guid Id { get; set; }
        public string? ImageLink { get; set; }
        public Guid ClientUserId { get; set; } 
        public string? TypeOfProperty { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? ClientUserName { get; set; }
    public string? PropertyPrice { get; set; }
    public IFormFile? ImgFile { get; set; } 
        public string? FormattedAddress => $"{Address}, {City}, {State}, {Country}";
    }
      
    public class CommentsViewModel
    {
        public string Id { get; set; }
        public string Comments { get; set; }
    }

    public class LoyaltyCardViewModel
    {
        public Guid? ClientGuid { get; set; }
        public int? ClientId { get; set; }
        public string? ClientName { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public DateTime? ExpireDate { get; set; }
        public bool? Active { get; set; }

    }

}
