using Microsoft.AspNetCore.Identity;

namespace Bahrin.Harbour.Data.DBCollections
{
    public class ApplicationUser : IdentityUser
    {
        public Guid UserGuid { get; set; } = Guid.NewGuid();
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Pin { get; set; }
        public string? Country { get; set; }
        public Guid? OutletId { get; set; }
      //  public Guid? OutletId { get; set; }
        public string Role { get; set; }
        public string? ZipCode { get; set; }
        public string? ProfileImageFileName { get; set; }
        public string? ProfileImagePathfolder { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public ICollection<UserOutletRelation>? UserOutlets { get; set; }

    }
}
