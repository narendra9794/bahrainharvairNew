using Bahrin.Harbour.Model.ClientModel;
using Bahrin.Harbour.Model.VisitHistoryModel;

namespace Bahrin.Harbour.Service.DashboardService
{
    public interface IDashboardService
    {
        int ClientCount();
        int OuletsCount();
        int PropertiesCount();
        int RepresentativeCount();
        int TotalDiscountCount();
        Task<List<VisitHistoryView>> RecentCheckins();
    }
}