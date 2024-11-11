using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Data.DBCollections
{
    public class Outlet : BaseEntity
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string? ContactPersonName { get; set; }
        public string? ContactPersonEmail { get; set; }
        public string? ContactPersonPhoneNumber { get; set; }
        public string? OutletImageName { get; set; }
        public string? OutletImageFolderName { get; set; }
        public Guid? RepresentativeId { get; set; }
        public ICollection<UserOutletRelation>? UserOutlets { get; set; }

    }
}
