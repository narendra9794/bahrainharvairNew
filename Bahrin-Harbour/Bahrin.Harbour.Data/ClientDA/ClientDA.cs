using Bahrin.Harbour.Data.DataContext;
using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.ClientModel;
using Bahrin.Harbour.Model.ProjectSession;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Bahrin.Harbour.Data.ClientDA
{
    public class ClientDA : IClientDA
    {
        private readonly BahrinHarbourContext _context;
        private readonly ILogger<ClientDA> _logger;

        public ClientDA(BahrinHarbourContext context, ILogger<ClientDA> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> AddClientAsync(Client client)
        {
            try
            {
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding client with ID {ClientId}", client.ClientId);
                throw;
            }
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            try
            {
                return await _context.Clients.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all clients.");
                throw;
            }
        }

        public async Task<Client?>? GetClientByIdAsync(string id)
        {
            try
            {
                return await _context.Clients.FirstOrDefaultAsync(c => c.Id == Guid.Parse(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving client with ID {ClientId}", id);
                throw;
            }
        }
        

        public async Task<bool> UpdateClientAsync(Client client)
        {
            try
            {
                var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Id == client.Id);
                if (existingClient != null)
                {
                    existingClient.ClientName = client.ClientName;
          existingClient.EmailAddress = client.EmailAddress;
                    existingClient.Phone = client.Phone;
          existingClient.Country = client.Country;
          existingClient.City = client.City;
          existingClient.State = client.State;
          existingClient.Postcode = client.Postcode;
          existingClient.Address = client.Address;
          existingClient.ClientProfileImageFileName = client.ClientProfileImageFileName            ;
          existingClient.ImageFolderName = client.ImageFolderName;


                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    _logger.LogWarning("Client with ID {ClientId} not found for update.", client.ClientId);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating client with ID {ClientId}", client.ClientId);
                throw;
            }
        }
        
        public async Task<bool> UpdateClientLoyalityCardStatusAsync(Client client)
        {
            try
            {
                var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.ClientId == client.ClientId);
                if (existingClient != null)
                {
                    existingClient.isLoyalityCardGenerated = client.isLoyalityCardGenerated;
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    _logger.LogWarning("Client with ID {ClientId} not found for update.", client.ClientId);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating client with ID {ClientId}", client.ClientId);
                throw;
            }
        }
        
       public async Task<bool> DeleteClientAsync(Guid Id)
 {
     try
     {
         var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == Id);
         if (client != null)
         {
             _context.Clients.Remove(client);
             await _context.SaveChangesAsync();
             return true;
         }
         else
         {
             _logger.LogWarning("Client with ID {ClientId} not found for deletion.", Id);
             return false;
         }
     }
     catch (Exception ex)
     {
         _logger.LogError(ex, "Error occurred while deleting client with ID {ClientId}", Id);
         throw;
     }
 }

        // code by rashi
        public async Task<List<Client>> GetAllClients()
        {
            try
            {
        return await _context.Clients.ToListAsync();
        
      }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all clients.");
                throw;
            }
        }
        /// <summary>
        /// /
        /// 
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        ///
        public async Task AddVisitHistoryAsync(VisitHistory history)
        {
            try
            {
                _context.VisitHistory.Add(history);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding client with ID {ClientId}", history.ClientId);
                throw;
            }
        }
        public async Task<List<VisitHistory>> GetVisitHistoryRepresentativeIdAsync(Guid id)
        {
            try
            {
                return _context.VisitHistory.Where(x => x.RepresentativeId == id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding client with ID {ClientId}", id.ToString());
                throw;
            }
        }
         
          public async Task<List<VisitHistory>> GetVisitHistoryByClientIdAsync(Guid id)
          {
            try
            {
                return _context.VisitHistory.Where(x => x.ClientId == id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding client with ID {ClientId}", id.ToString());
                throw;
            }
        }
              public async Task<List<VisitHistory?>?> GetVisitHistoryByOutletIdAsync(Guid? id)
        {
            try
            {
                if (id == null || id == Guid.Empty)
                {
                    return null;
                }
                return _context.VisitHistory?.Where(x => x.OutletId == id)?.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding client with ID {ClientId}", id.ToString());
                throw;
            }
        }

        public DateTime? LastVisitOfAClient(Guid id)
        {
            try
            {
                return _context.VisitHistory.Where(x => x.ClientId == id).OrderByDescending(x => x.ClientId).FirstOrDefault()?.VisitedDate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding client with ID {ClientId}", id.ToString());
                throw;
            }
        }

        public List<VisitHistory> GetRecentVisits()
        {
            try
            {
                return _context.VisitHistory.OrderByDescending(x => x.CheckinDate).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred");
                throw;
            }
        }
        public async Task<bool> UpdateVisitHistoryAsync(CommentsViewModel model)
        {
            try
            {
                var existingVH = _context.VisitHistory.Where(x => x.Id == Guid.Parse(model.Id)).FirstOrDefault();
                if (existingVH != null)
                {
                    existingVH.Checkin = Constants.True;
                    existingVH.Comments = model.Comments;
                    existingVH.CheckinDate = DateTime.Now;

                  await _context.SaveChangesAsync();
                  return true;
                }
                else
                {
                    _logger.LogWarning("VisitHistory with ID {ClientId} not found for update.", existingVH.Id);
                    return false;
                }
            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, "Error occurred while adding client with ID {ClientId}", id.ToString());
                throw;
            }

          
        }

        /// <summary>
        /// //Property
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="activate"></param>
        /// <returns></returns>
        /// 
        public async Task AddOrUpdatePropertyAsync(Property property)
        {
            try
            {
                var existingProperty = await _context.Properties.FirstOrDefaultAsync(c => c.Id == property.Id);

                if (existingProperty != null)
                {
                    existingProperty.UploadedImageName = property.UploadedImageName;
                    existingProperty.UploadedImageFolderName = property.UploadedImageFolderName;
                    existingProperty.ClientUserId = property.ClientUserId;
                    existingProperty.TypeOfProperty = property.TypeOfProperty;
                    existingProperty.Country = property.Country;
                    existingProperty.State = property.State;
                    //existingProperty.City = property.City;
                    existingProperty.Address = property.Address;
                    existingProperty.DateModified = DateTime.Now;
                    existingProperty.ModifiedBy = ProjectSessionModel.admin._id;
          existingProperty.PropertyPrice = property.PropertyPrice;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    await _context.Properties.AddAsync(property);
                 var a=   await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<Property>> GetPropertyByIdAsync(Guid id)
        {
            try
            {
                return await _context.Properties.Where(c => c.ClientUserId == id).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Property with ID {ClientId}", id);
                throw;
            }
        }
        public async Task<Property> GetPropertyByPropertyIdAsync(Guid id)
        {
            try
            {
                return await _context.Properties.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Property with ID {ClientId}", id);
                throw;
            }
        }
        public int GetPropertyCountByIdAsync(Guid id)
        {
            try
            {
                return _context.Properties.Where(c => c.ClientUserId == id).Count();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving Property with ID {ClientId}", id);
                throw;
            }
        }

        public async Task<bool> DeletePropertyAsync(string id)
        {
            try
            {
                var property = await _context.Properties.FirstOrDefaultAsync(x=>x.Id == Guid.Parse(id));

                if (property == null)
                {
                    return false;
                }

                _context.Properties.Remove(property);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
         public int AllPropertyCount()
        {
            try
            {
                var property =  _context.Properties.Count();

                return property;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> ClientStatusAsync(Guid Id, bool activate)
    {
      try
      {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == Id);
        if (client != null)
        {
          client.AciveStatus = activate;
          await _context.SaveChangesAsync();


          string statusMessage = activate ? "Client Activated successfully." : "Client Deactivated successfully.";
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


