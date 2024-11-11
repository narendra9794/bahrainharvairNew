using Bahrin.Harbour.Data.ClientDA;
using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Data.OutletDA;
using Bahrin.Harbour.Data.VisitHistoryService;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.ClientModel;
using Bahrin.Harbour.Model.VisitHistoryModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Bahrin.Harbour.Service.DashboardService
{
    public class DashboardService : IDashboardService
    {
        private readonly IImageService _imageService;
        private readonly IClientDA _clientDA;
        private readonly IOutletDA _outletDA;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVisitHistoryService _visitHistory;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(
            IImageService imageService,
            IClientDA clientDA,
            IOutletDA outletDA,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<DashboardService> logger,
            IVisitHistoryService visitHistory)
        {
            _imageService = imageService;
            _clientDA = clientDA;
            _outletDA = outletDA;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _visitHistory = visitHistory;
    }

    public int ClientCount()
        {
            return _clientDA.GetAllClients().Result.Count();
        }
        public int PropertiesCount()
        {
            return _clientDA.AllPropertyCount();
        }

        public int TotalDiscountCount()
        {
            int recentVisits = _clientDA.GetRecentVisits().Where(x => x.Checkin == true).ToList().Count();

            return recentVisits;
        }
        public int OuletsCount()
        {
            return _outletDA.GetAllOutletsAsync().Result.Count();
        }
        public int RepresentativeCount()
        {
            return _userManager.GetUsersInRoleAsync(Constants.AppUser).Result.Count();
        }
        public async Task<List<VisitHistoryView>> RecentCheckins()
        {
            List<VisitHistory> recentVisits = _clientDA.GetRecentVisits().Where(x=>x.Checkin == true).Take(5).ToList();
            //       List<Outlet?>? outlets = _outletDA?.GetAllOutletsAsync().Result;
            List<Client> AllClients = await _clientDA.GetAllClients();

            List<VisitHistoryView> historyView = recentVisits.Select(visit => new VisitHistoryView
            {
                Id = visit.Id,
        //        OutletId = visit.OutletId,              
           //   OutletName = outlets?.FirstOrDefault(x=>x?.Id == visit?.OutletId).Name, 
                RepresentativeId = visit.RepresentativeId,
                VisitedDate = visit.VisitedDate,
                Comments = visit.Comments,
                Checkin = visit.Checkin,
                CheckinDate= visit.CheckinDate,
                ClientId = visit.ClientId,
                Visited = visit.Visited,
                ClientIntId = Helper.Helper.FormatClientId(AllClients?.FirstOrDefault(x=>x.Id == visit.ClientId)?.ClientId),
                ClientImageLink = _imageService.GenerateImageUrl(AllClients?.FirstOrDefault(x => x.Id == visit.ClientId)?.ImageFolderName,AllClients?.FirstOrDefault(x => x.Id == visit.ClientId)?.ClientProfileImageFileName)

            }).OrderByDescending(x=>x.VisitedDate).ToList();

            return historyView;
        }
    }
}
