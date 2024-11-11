using Bahrin.Harbour.Data.DBCollections;

namespace Bahrin.Harbour.Data.LoyalityCardDA
{
    public interface ILoyalityCardDA
    {
        Task<bool> SaveOrUpdateLoyalityCardAsync(LoyaltyCard card);
        Task<LoyaltyCard> GetCardsByIdAsync(string id);
        Task<List<LoyaltyCard>> GetAllCardsAsync();
    }
}