using Bahrin.Harbour.Data.ClientDA;
using Bahrin.Harbour.Data.DataContext;
using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Model.ClientModel;
using Bahrin.Harbour.Model.ProjectSession;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Data.OutletDA
{
    public class OutletDA : IOutletDA
    {
        private readonly BahrinHarbourContext _context;
        private readonly ILogger<OutletDA> _logger;
    public OutletDA(BahrinHarbourContext context, ILogger<OutletDA> logger)
        {
            _context = context;
      _logger = logger;
    }

        public async Task<List<Outlet>> GetAllOutletsAsync()
        {
            return await _context.Outlets.ToListAsync();
        }

        public async Task<Outlet> GetOutletByIdAsync(Guid? id)
        {
            return await _context.Outlets.FindAsync(id);
        }
         public async Task<Outlet> GetOutletByRepresentativeIdAsync(Guid id)
        {
            return await _context.Outlets.Where(x=>x.RepresentativeId == id).FirstOrDefaultAsync();
        }

        public async Task AddOutletAsync(Outlet outlet)
        {
      try
      {
        //await _context.Outlets.AddAsync(outlet);
        //await _context.SaveChangesAsync();
        _context.Outlets.Add(outlet);
        await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {

      }
    }
    
    public async Task UpdateOutletAsync(Outlet outlet)
        {
      try
      {
        outlet.ModifiedBy = ProjectSessionModel.admin._id;
        outlet.DateModified = DateTime.Now;

        _context.Outlets.Update(outlet);
        await _context.SaveChangesAsync();
      }
      catch(Exception ex)
      {

      }
            
        }

        public async Task DeleteOutletAsync(Guid id)
        {
            var outlet = await _context.Outlets.FindAsync(id);
            if (outlet != null)
            {
                _context.Outlets.Remove(outlet);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> OutletExistsAsync(Guid id)
        {
            return await _context.Outlets.AnyAsync(o => o.Id == id);
        }

    public async Task<bool> OutletStatusAsync(Guid Id, bool activate)
    {
      try
      {
        var client = await _context.Outlets.FirstOrDefaultAsync(c => c.Id == Id);
        if (client != null)
        {
          client.AciveStatus = activate;
          await _context.SaveChangesAsync();


          string statusMessage = activate ? "Outlet Activated successfully." : "Outlet Deactivated successfully.";
          _logger.LogInformation(statusMessage);
          return true;
        }
        else
        {
          _logger.LogWarning("Client with ID {ClientId} not found .", Id);
          return false;
        }
      }
      catch (Exception ex)
      {

        throw;
      }
    }
  }

}

