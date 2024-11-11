
using Bahrin.Harbour.Data.DBCollections;

namespace Bahrin.Harbour.Data.VisitHistoryDA
{
    public interface IVisitHistoryDA
    {
        Task<List<VisitHistory>> GetAllVisitAndCheckinHistry();
    }
}