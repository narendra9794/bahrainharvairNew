using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Model.ReportModel
{
    public class OutletletsVM
    {
        public int Clientid { get; set; }
        public Guid Representiveid { get; set; }
        public string? ClientName { get; set; }
        public string? OutletName { get; set; }
        public DateTime? VisitDate { get; set; }
        public DateTime? checkinDate { get; set; }
        public string? RepresentativeName { get; set; }
        public string? RepresentativeDiscount { get; set; }
        public int? SalesRefersDIS { get; set; }
    }
}
