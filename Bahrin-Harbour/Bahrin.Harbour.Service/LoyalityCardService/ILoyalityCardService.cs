using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.ClientModel;

namespace Bahrin.Harbour.Service.LoyalityCard
{
    public interface ILoyalityCardService
    {
        Task<StatusModel> CreateOrUpdateAsync(string ClientId, bool active);
        Task<LoyaltyCardViewModel> GetCardAsync(string id);
        Task<List<LoyaltyCardViewModel>> GetAllCard();
        Task<(byte[] PdfData, string QrCodeString)> CreatePdfWithQrCode(string clientId);
    }
}