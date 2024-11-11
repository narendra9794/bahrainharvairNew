using Bahrin.Harbour.Data.ClientDA;
using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Data.OutletDA;
using Bahrin.Harbour.Data.VisitHistoryDA;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.ClientModel;
using Bahrin.Harbour.Model.VisitHistoryModel;
using Bahrin.Harbour.Service.ClientService;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Data.VisitHistoryService
{

    public class VisitHistoryService : IVisitHistoryService
    {
        private readonly IVisitHistoryDA _visitHistory;
        private readonly IOutletDA _outletDA;
        private readonly IClientDA _clientDA;
        private readonly IImageService _image;
        private readonly UserManager<ApplicationUser> _user;
        public VisitHistoryService(IVisitHistoryDA visitHistory, IImageService image, IClientDA clientDA, IOutletDA outletDA, UserManager<ApplicationUser> user)
        {
            _visitHistory = visitHistory;
            _image = image;
            _clientDA = clientDA;
            _outletDA = outletDA;
            _user = user;
        }

        public async Task<List<VisitHistoryView>> GetAllCheckins()
        {
            List<VisitHistoryView> visitHistoryViews = new List<VisitHistoryView>();

            List<VisitHistory> allVisitsAndCheckins = await _visitHistory.GetAllVisitAndCheckinHistry();
            List<Client> Clients = await _clientDA.GetAllClientsAsync();
            List<Outlet> Outlets = await _outletDA.GetAllOutletsAsync();
            var user = await _user.GetUsersInRoleAsync(Constants.AppUser);

            var allCheckinsOnly = allVisitsAndCheckins.Where(x => x.Checkin).OrderByDescending(x => x.CheckinDate).ToList();


            foreach (var item in allCheckinsOnly)
            {
                Client? client = Clients.FirstOrDefault(x => x.Id == item.ClientId);
                Outlet? Outlet = Outlets.FirstOrDefault(x => x.Id == item.OutletId);
                var User = user.FirstOrDefault(x => x.Id == item.RepresentativeId.ToString());


                VisitHistoryView checkin = new VisitHistoryView
                {
                    Id = item.Id,
                    ClientId = item.ClientId,
                    OutletId = item.OutletId,
                    Checkin = item.Checkin,
                    CheckinDate = item.CheckinDate,
                    Visited = item.Visited,
                    VisitedDate = item.VisitedDate,
                    Comments = item.Comments,
                    ClientIntId = Helper.Helper.FormatClientId(client?.ClientId),
                    ClientImageLink = _image.GenerateImageUrl(client?.ImageFolderName, client?.ClientProfileImageFileName),
                    OutletName = Outlet?.Name,
                    RepresentativeName = User?.FirstName + User?.LastName,
                    ClientName = client?.Name,
                    OutletLocation = Outlet?.Address
                };
                visitHistoryViews.Add(checkin);
            }

            return visitHistoryViews;
        }

        public async Task<VisitAnalytics> Analytics(int i)
        {
            List<VisitHistory> allVisitsAndCheckins = await _visitHistory.GetAllVisitAndCheckinHistry();


            var allCheckinsOnly = allVisitsAndCheckins.OrderByDescending(x => x.CheckinDate).ToList();

            var startDate = DateTime.Today.AddDays(-i);
            var endDate = DateTime.Today;

            var allDates = Enumerable.Range(0, (endDate - startDate).Days + 1)
                                      .Select(offset => startDate.AddDays(offset))
                                      .ToList();

            var groupedVisits = allCheckinsOnly
                .Where(v => v.Visited && v.VisitedDate.Date >= startDate && v.VisitedDate.Date <= endDate)
                .GroupBy(v => v.VisitedDate.Date)
                .Select(g => new Visit
                {
                    Date = g.Key,
                    CheckinCount = g.Count(x => x.Checkin == true),
                    VisitCount = g.Count()
                })
                .ToList();

            var result = allDates
                .GroupJoin(
                    groupedVisits,
                    date => date,
                    visit => visit.Date,
                    (date, visits) => new Visit
                    {
                        Date = date,
                        CheckinCount = visits.FirstOrDefault()?.CheckinCount ?? 0,
                        VisitCount = visits.FirstOrDefault()?.VisitCount ?? 0
                    })
                .ToList();

            var totalVisits = result.Sum(v => v.VisitCount);
            var totalCheckins = result.Sum(v => v.CheckinCount);

            double percentageDiscount = 0;

            if (totalVisits != 0)
            {
                percentageDiscount = ((double)totalCheckins / totalVisits) * 100;

            }

            return new VisitAnalytics
            {
                Visits = result,
                TotalVisit = totalVisits,
                PercentageDiscount = Math.Round(percentageDiscount, 1)
            };
        }
        

        public async Task<List<OutletAnalytics>> OutletAnalytics(int i)
        {
            var startDate = DateTime.Today.AddDays(-i);
            var endDate = DateTime.Today;

            List<VisitHistory> allVisitsAndCheckins = await _visitHistory.GetAllVisitAndCheckinHistry();
             allVisitsAndCheckins = allVisitsAndCheckins.Where(v=>v.VisitedDate.Date >= startDate && v.VisitedDate.Date <= endDate).ToList();
            List<Outlet> Outlets = await _outletDA.GetAllOutletsAsync();

            return allVisitsAndCheckins.Where(x=>x.Checkin).GroupBy(x => x.OutletId).Select(visit => new OutletAnalytics
            {
                OutletName = Outlets?.FirstOrDefault(x => x.Id == visit.FirstOrDefault()?.OutletId)?.Name,
                CheckinCount = visit.Count(x=>x.Checkin)
            }).OrderByDescending(x=>x.CheckinCount).Where(x=> !string.IsNullOrWhiteSpace(x.OutletName)).ToList();

        }

    }

    
}
