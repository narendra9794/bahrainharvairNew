using Bahrin.Harbour.Data.DataContext;
using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Model.ProjectSession;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Data.LocationDA
{
    public class LocationDA : ILocationDA
    {
        private readonly BahrinHarbourContext _context;
        private readonly ILogger<LocationDA> _logger;
        public LocationDA(BahrinHarbourContext context, ILogger<LocationDA> logger)
        {
            _context = context;
            _logger = logger;
        }


        public async Task<List<City>> GetCountriesAsync()
        {

            return await _context.Cities
                .FromSqlRaw("EXEC GetCitiesByStateId ")
                .ToListAsync();
        }

        public async Task<List<City>> GetStatesByCountryIdAsync(int CountryId)
        {
            var parameter = new SqlParameter("@StateId", CountryId);

            return await _context.Cities
                .FromSqlRaw("EXEC GetCitiesByStateId @StateId", parameter)
                .ToListAsync();
        }

        public async Task<List<City>> GetCitiesByStateIdAsync(int stateId)
        {
            var parameter = new SqlParameter("@StateId", stateId);

            return await _context.Cities
                .FromSqlRaw("EXEC GetCitiesByStateId @StateId", parameter)
                .ToListAsync();
        }
    }
}