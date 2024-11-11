using Bahrin.Harbour.Data.VisitHistoryService;
using Bahrin.Harbour.Service.ClientService;
using Bahrin.Harbour.Service.DashboardService;
using Microsoft.AspNetCore.Mvc;

namespace Bahrin_Harbour.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("[area]/[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IClientService _clientService;
        private readonly IVisitHistoryService _visitHistory;

        public DashboardController(IDashboardService dashboardService, IClientService clientService, IVisitHistoryService visitHistory)
        {
            _dashboardService = dashboardService;       
            _clientService = clientService;
            _visitHistory = visitHistory;

        }
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult ClientCount()
        {
           var count = _dashboardService.ClientCount();
           
           return Ok(count);
        }
        public IActionResult PropertiesCount()
        {
            var count = _dashboardService.PropertiesCount();

            return Ok(count);
        }
        public IActionResult OuletsCount()
        {
            var count = _dashboardService.OuletsCount();

            return Ok(count);
        }
        public IActionResult RepresentativeCount()
        {
            var count = _dashboardService.RepresentativeCount();

            return Ok(count);
        }
        public IActionResult TotalDiscountCount()
        {
            var count = _dashboardService.TotalDiscountCount();

            return Ok(count);
        }
      
        public async Task<IActionResult> GetRecent()
        {
            var recentCheckins = await _dashboardService.RecentCheckins();

            return PartialView("RecentCheckinsView", recentCheckins);
        }
        
        public async Task<IActionResult> CheckinAnalytics(int i = 10)
        {
            var recentCheckins = await _visitHistory.Analytics(i);

            return Ok( recentCheckins);
        }
         public async Task<IActionResult> OutletAnalytics(int i)
        {
            var recentCheckins = await _visitHistory.OutletAnalytics(i);

            return Ok( recentCheckins);
        }

    }
}
