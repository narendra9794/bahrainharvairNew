using Bahrin.Harbour.Data.DataContext;
using Bahrin.Harbour.Data.DBCollections;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Data.VisitHistoryDA
{
    public class VisitHistoryDA : IVisitHistoryDA
    {
        private readonly BahrinHarbourContext _context;
        private readonly ILogger<VisitHistoryDA> _logger;

        public VisitHistoryDA(BahrinHarbourContext context, ILogger<VisitHistoryDA> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<VisitHistory>> GetAllVisitAndCheckinHistry()
        {
            try
            {
                return _context.VisitHistory.ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
