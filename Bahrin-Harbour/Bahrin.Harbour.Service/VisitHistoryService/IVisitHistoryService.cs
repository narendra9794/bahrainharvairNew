using Bahrin.Harbour.Model.VisitHistoryModel;
using Bahrin.Harbour.Service.ClientService;

namespace Bahrin.Harbour.Data.VisitHistoryService
{
    public interface IVisitHistoryService
    {
        Task<List<VisitHistoryView>> GetAllCheckins();
        Task<VisitAnalytics> Analytics(int i );
        Task<List<OutletAnalytics>> OutletAnalytics(int i);
    }
}