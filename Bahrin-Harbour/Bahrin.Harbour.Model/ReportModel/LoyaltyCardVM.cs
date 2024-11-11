using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Model.ReportModel
{
    public class LoyaltyCardVM
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedDatecard { get; set; }
        public int CardCounter { get; set; }
    }
}
