using Bahrin.Harbour.Data.DBCollections;

namespace Bahrin.Harbour.Data.EMailDA
{
    public interface IEMailDA
    {
        Task<Email> GetEmailSettingAsync();
        Task<bool> SaveOrUpdateEmailSettingAsync(Email mail);
    }
}