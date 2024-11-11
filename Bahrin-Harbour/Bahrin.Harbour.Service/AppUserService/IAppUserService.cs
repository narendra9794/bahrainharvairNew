using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.AppUserAuth;

namespace Bahrin.Harbour.Service.AppUserService
{
    public interface IAppUserService
    {
        Task<StatusModel> AddAppUserAsync(AppUserViewModel appUser);
        Task<StatusModel> DeActivateUser(string userId);
        Task<List<AppUserViewModel>> GetAllAppUsersAsync();
        Task<AppUserViewModel> GetAppUserByIdAsync(string id);
        Task<StatusModel> HardDeleteAppUserAsync(string userId);
        Task<bool> SendMailOnAccountCreation(string outletName, ApplicationUser user, string Password);
        Task<StatusModel> UpdateAppUserAsync(AppUserViewModel appUser);
    }
}