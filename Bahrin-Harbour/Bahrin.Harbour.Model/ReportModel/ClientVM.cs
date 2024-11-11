using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Model.ReportModel
{
    public class ClientVM
    {
        public int ClientId { get; set; }
        public string? Name { get; set; }
        public int Properties { get; set; }
        public int NoOfVisit { get; set; }
        public int NoCheckIn { get; set; }
        public DateTime LastVisit { get; set; }

    }
}
