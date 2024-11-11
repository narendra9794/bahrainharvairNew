
namespace Bahrin.Harbour.Data.DBCollections
{
    public class VisitHistory : BaseEntity
    {
        public Guid ClientId { get; set; }
        public Guid? OutletId { get; set; }
        public Guid? RepresentativeId { get; set; }
        public bool Visited { get; set; }
        public DateTime VisitedDate { get; set; }
        public bool Checkin  { get; set; }
        public DateTime? CheckinDate { get; set; }
        public string? Comments { get; set; }
        public int? Discount { get; set; }
    }
}
