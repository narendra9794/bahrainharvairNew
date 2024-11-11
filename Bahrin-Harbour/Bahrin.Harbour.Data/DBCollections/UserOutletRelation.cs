using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Data.DBCollections
{
    public class UserOutletRelation
    {

            public Guid UserId { get; set; }
            public ApplicationUser User { get; set; }
            public Guid OutletId { get; set; }
            public Outlet Outlet { get; set; }
    }
}
