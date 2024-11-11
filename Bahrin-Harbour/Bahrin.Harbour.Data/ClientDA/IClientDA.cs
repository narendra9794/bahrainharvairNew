using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Model.ClientModel;

namespace Bahrin.Harbour.Data.ClientDA
{
    public interface IClientDA
    {
        Task<bool> AddClientAsync(Client client);
        Task<bool> DeleteClientAsync(Guid id);
        Task<List<Client>> GetAllClientsAsync();
        Task<Client?>? GetClientByIdAsync(string id);
        Task<bool> UpdateClientAsync(Client client);
        Task<List<Client>> GetAllClients();
        Task<List<Property>> GetPropertyByIdAsync(Guid id);
        int GetPropertyCountByIdAsync(Guid id);
        Task<bool> DeletePropertyAsync(string id);
        Task AddVisitHistoryAsync(VisitHistory history);
        Task AddOrUpdatePropertyAsync(Property property);
        Task<bool> UpdateVisitHistoryAsync(CommentsViewModel model);
        Task<List<VisitHistory>> GetVisitHistoryByClientIdAsync(Guid id);
        Task<List<VisitHistory>> GetVisitHistoryRepresentativeIdAsync(Guid id);
        DateTime? LastVisitOfAClient(Guid id);
        Task<bool> ClientStatusAsync(Guid id, bool activate);
        Task<bool> UpdateClientLoyalityCardStatusAsync(Client client);
        Task<List<VisitHistory?>?> GetVisitHistoryByOutletIdAsync(Guid? id);
        int AllPropertyCount();
        List<VisitHistory> GetRecentVisits();
    Task<Property> GetPropertyByPropertyIdAsync(Guid id);
 

  }
}