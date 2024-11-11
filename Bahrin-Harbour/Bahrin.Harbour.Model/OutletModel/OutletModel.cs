using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Model.OutletModel
{
        public class OutletViewModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string Address { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Property Price must be a positive value.")]
    public decimal DiscountPercentage { get; set; }
            public string ContactPersonName { get; set; }
            public string ContactPersonEmail { get; set; }
            public string ContactPersonPhoneNumber { get; set; }
            public string? OutletImageLink { get; set; }
            public IFormFile? ImageFile { get; set; }
            public bool AciveStatus { get; set; }
            public string? RepresentativeName { get; set; }
            public string? RepresentativeId { get; set; }
            public string? ProfileImageLink { get; set; }

            public ICollection<UserOutletRelationViewModel>? UserOutlets { get; set; }

        }

        public class UserOutletRelationViewModel
        {
            public Guid UserId { get; set; }
            public Guid OutletId { get; set; }
        }

    
}
