using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Data.DBCollections
{
    public class LoyaltyCard : BaseEntity
    {
        public Guid? ClientGuid { get; set; }
        public int ClientId { get; set; }
        public string? ClientName { get; set; }
        public string? Email { get; set; }
        public string? ContactNumber { get; set; }
        public DateTime? ExpireDate { get; set; }
    
    }
}
