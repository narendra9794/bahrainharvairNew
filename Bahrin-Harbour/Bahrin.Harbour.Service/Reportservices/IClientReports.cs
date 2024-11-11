using Bahrin.Harbour.Model.ReportModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Service.Reportservices
{
    public interface IClientReports
    {
        public List<ClientVM> Getclientdetails(DateTime Startdate, DateTime dateEnddate);
        public List<LoyaltyCardVM> GetLoyaltyCarddetails(DateTime startDate, DateTime endDate);
        public List<DropdownVM> GetDropdowndetails();
        public List<OutletletsVM> GetOutletvisitdetails(DateTime Startdate, DateTime dateEnddate, List<Guid> Id);
    }
}
