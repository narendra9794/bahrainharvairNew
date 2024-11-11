using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.ClientModel;
using Bahrin.Harbour.Model.VisitHistoryModel;
using Microsoft.AspNetCore.Http;

namespace Bahrin.Harbour.Service.ClientService
{
    public interface IClientService
    {
        Task<bool> ClientDetails(ClientViewModel clientViewModel, IFormFile ImageFile);
        Task<bool> DeleteClientAsync(string id);
        Task<List<ClientViewModel>> GetAllClientsAsync();
        Task<ClientViewModel> GetClientByIdAsync(string id, string UserId = null, bool isChechIn = false);
        Task<List<ClientViewModel>> GetAllClients();
    Task<List<PropertyViewModel>> GetPropertiesByClientid(Guid id);

       Task<ClientViewModel> GetClientsById(string id);
        Task<StatusModel> CheckInComments(CommentsViewModel model);
        Task<VisitAnalytics> Analytics(string id, int lastNumberOfDays);
        Task DeletePropertyAsync(string id);
        Task<List<VisitedViewModel>> GetAllVisitsOrRecentCheckins(string id, DateTime? date = null, bool onlyCheckIns = false, bool onlyCount = false, int? numberOfItems = null);
        Task<List<VisitedViewModel>> GetAllVIPClients(string id);
         Task<bool> ClientStatusAsync(string id, bool activate);
        Task<ClientViewModel> GetClientByIdAsynctry(string id);

        Task<List<VisitedViewModel>> GetAllCheckInsOfAClient(string representativeId, string id, bool onlyCheckIns = false, bool onlyCount = false);
       Task<bool> AddOrUpdatePropertyAsync(PropertyViewModel property, IFormFile? imgFile = null);

        Task<PropertyViewModel> GetPropertyByPropertyIdAsync(Guid id);

  }
}