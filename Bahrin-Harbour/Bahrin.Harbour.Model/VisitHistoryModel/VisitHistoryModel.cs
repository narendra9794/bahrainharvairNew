using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Model.VisitHistoryModel
{

        public class VisitedViewModel
        {
            public string? Name { get; set; }
            public string? ClientId { get; set; }
            public string? Id { get; set; }
            public string? imageUrl { get; set; }
            public DateTime? VisitDate { get; set; }
            public DateTime? CheckInDate { get; set; }
            public int? TotalVisited { get; set; }
            public int? TotalCheckins { get; set; }
            public string? OutletName { get; set; }
            public string? Comments { get; set; }
            public string? Discount { get; set; }

    }

    public class VisitHistoryView
        {
            public Guid? Id { get; set; }
            public Guid? OutletId { get; set; }
            public Guid? ClientId { get; set; }
            public string? ClientIntId { get; set; }
            public string? ClientImageLink { get; set; }
            public string? OutletName { get; set; }
            public Guid? RepresentativeId { get; set; }
            public DateTime? VisitedDate { get; set; }
            public bool? Checkin { get; set; }
            public DateTime? CheckinDate { get; set; }
            public string? Comments { get; set; }
            public bool? Visited { get; set; }
            public string? RepresentativeName { get; set; }
            public string? ClientName { get; set; }
            public string? OutletLocation{ get; set; }
            public string? Discount{ get; set; }
        }
    public class OutletAnalytics()
    {
        public string OutletName { get; set; }
        public int CheckinCount { get; set; }
    }
}

