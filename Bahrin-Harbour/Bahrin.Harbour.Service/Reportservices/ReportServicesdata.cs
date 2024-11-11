using Bahrin.Harbour.Data.DataContext;
using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.ReportModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Service.Reportservices
{
    public class ReportServicesdata : IClientReports
    {
        private readonly BahrinHarbourContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportServicesdata(BahrinHarbourContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// Getclientdetails
        /// </summary>
        /// <param name="Startdate"></param>
        /// <param name="dateEnddate"></param>
        /// <returns></returns>
        public List<ClientVM> Getclientdetails(DateTime Startdate, DateTime dateEnddate)
        {
            try
            {
                var data = (from cl in _context.Clients
                            join pr in _context.Properties on cl.Id equals pr.ClientUserId
                            join vh in _context.VisitHistory on cl.Id equals vh.ClientId
                            where cl.CreatedDate >= Startdate && cl.CreatedDate <= dateEnddate
                            group new { pr, vh } by new { cl.ClientId, cl.Name } into g
                            select new ClientVM
                            {
                                ClientId = g.Key.ClientId,
                                Name = g.Key.Name,
                                Properties = g.Count(x => x.pr.ClientUserId != null),
                                NoOfVisit = g.Count(x => x.vh.Checkin == false),
                                NoCheckIn = g.Count(x => x.vh.Checkin == true),
                                LastVisit = g.Max(x => x.vh.VisitedDate)
                            }).OrderByDescending(c => c.NoCheckIn)

              .ThenBy(c => c.LastVisit)
              .ToList();

                return data;
            }
            catch (Exception ex)
            {


                throw;
            }
        }

        /// <summary>
        /// Loyalty Card
        /// </summary>
        /// <param name="Startdate"></param>
        /// <param name="dateEnddate"></param>
        /// <returns></returns>
        public List<LoyaltyCardVM> GetLoyaltyCarddetails(DateTime startDate, DateTime endDate)
        {
            try
            {
                var data = (from lc in _context.LoyaltyCards
                            where lc.CreatedDate >= startDate && lc.CreatedDate <= endDate && lc.AciveStatus == true
                            select new LoyaltyCardVM
                            {
                                UserId = lc.ClientId,
                                Name = lc.ClientName,
                                CreatedDatecard = lc.CreatedDate,
                                CardCounter = 0
                            }).ToList();
                
                var groupedData = data.GroupBy(x => x.CreatedDatecard.Date)
                                      .Select(g => new LoyaltyCardVM
                                      {
                                          CreatedDatecard = g.Key,
                                          CardCounter = g.Count()
                                      })
                                      .ToList();
               
                var output = new List<LoyaltyCardVM>();
                foreach (var group in groupedData)
                {
                    
                    output.Add(new LoyaltyCardVM
                    {
                        CreatedDatecard = group.CreatedDatecard,
                        CardCounter = group.CardCounter
                    });
                    
                    foreach (var card in data.Where(x => x.CreatedDatecard.Date == group.CreatedDatecard.Date))
                    {
                        output.Add(new LoyaltyCardVM
                        {
                            UserId = card.UserId,
                            Name = card.Name,
                            CreatedDatecard = card.CreatedDatecard,
                            CardCounter = 0
                        });
                    }
                }
                output = output.Where(x => x.UserId != 0).OrderByDescending(x => x.CreatedDatecard).ToList();

                return output;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// GetDropdowndata
        /// </summary>
        /// <param name="Startdate"></param>
        /// <param name="dateEnddate"></param>
        /// <returns></returns>
        public List<DropdownVM> GetDropdowndetails()
        {
            try
            {
                var data = (from ol in _context.Outlets
                            where ol.AciveStatus == true
                            select new DropdownVM
                            {
                                Id = ol.Id,
                                outletName = ol.Name
                            }).ToList();

                return data;
            }
            catch (Exception ex)
            {


                throw;
            }
        }
        /// <summary>
        /// GetOutletvisitdetails
        /// </summary>
        /// <param name="Startdate"></param>
        /// <param name="dateEnddate"></param>
        /// <returns></returns>
        public List<OutletletsVM> GetOutletvisitdetails(DateTime Startdate, DateTime dateEnddate, List<Guid> alloutletId)
        {
            try
            {
                var users = _userManager.GetUsersInRoleAsync(Constants.AppUser).Result.ToList().Where(x => x.IsActive == true);
                var activeUserIds = users.Select(u => Guid.Parse(u.Id)).ToList();
                var data = (from VH in _context.VisitHistory
                            join clientd in _context.Clients on VH.ClientId equals clientd.Id
                            join outletid in _context.Outlets on VH.OutletId equals outletid.Id
                            where VH.VisitedDate >= Startdate && VH.VisitedDate <= dateEnddate && alloutletId.Contains((Guid)VH.OutletId) 
                            select new OutletletsVM
                            {
                                Clientid = clientd.ClientId,
                                ClientName = clientd.Name,
                                OutletName = outletid.Name,
                                Representiveid = (Guid)VH.RepresentativeId,
                                SalesRefersDIS = VH.Discount,
                                VisitDate = VH.Checkin == false ? VH.VisitedDate : (DateTime?)null,
                                checkinDate = VH.Checkin == true ? VH.CheckinDate : (DateTime?)null
                            }).OrderBy(x => x.checkinDate).ThenByDescending(x => x.VisitDate).ToList();

                if (data.Count > 0)
                {
                    foreach (var outlet in data)
                    { 
                        var user = users.FirstOrDefault(u => Guid.Parse(u.Id) == outlet.Representiveid);
                        if (user != null)
                        {
                            outlet.RepresentativeName = user.FirstName; 
                        }
                    }
                }
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }

        }





    }
}
