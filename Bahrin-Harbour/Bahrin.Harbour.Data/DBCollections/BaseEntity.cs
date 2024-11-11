
using Bahrin.Harbour.Model.ProjectSession;

namespace Bahrin.Harbour.Data.DBCollections
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DateModified { get; set; }
        public Guid Createdby { get; set; } 
        public Guid? ModifiedBy { get; set; }
        public bool AciveStatus { get; set; }
    }
}
