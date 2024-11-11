using Bahrin.Harbour.Data.DataContext;
using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Model.ProjectSession;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bahrin.Harbour.Data.LoyalityCardDA
{
    public class LoyalityCardDA : ILoyalityCardDA
    {
        private readonly BahrinHarbourContext _context;
        private readonly ILogger<LoyalityCardDA> _logger;

        public LoyalityCardDA(BahrinHarbourContext context, ILogger<LoyalityCardDA> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> SaveOrUpdateLoyalityCardAsync(LoyaltyCard card)
        {
            try
            {
                var existingCard = await _context.LoyaltyCards.Where(x => x.ClientGuid == card.ClientGuid).FirstOrDefaultAsync();

                if (existingCard == null)
                {
                    card.Id = Guid.NewGuid();
                    card.CreatedDate = DateTime.Now;
                    card.Createdby = ProjectSessionModel.admin._id;

                    await _context.LoyaltyCards.AddAsync(card);
                }
                else
                {
                    existingCard.Email = card.Email;
                    existingCard.ExpireDate = card.ExpireDate;
                    existingCard.ContactNumber = card.ContactNumber;
                    existingCard.AciveStatus = card.AciveStatus;
                    existingCard.ClientName = card.ClientName;
                    existingCard.DateModified = DateTime.Now;
                    existingCard.ModifiedBy = ProjectSessionModel.admin._id;
                    _context.LoyaltyCards.Update(existingCard);
                }

                var saved = await _context.SaveChangesAsync();

                return saved > 0;
            }
            catch(Exception ex)
            {
                throw ex;
            }
          
        }

        public async Task<List<LoyaltyCard>> GetAllCardsAsync()
        {
            try
            {
                return await _context.LoyaltyCards.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllCardsAsync: {ex.Message}");
                return new List<LoyaltyCard>(); 
            }
        }


        public async Task<LoyaltyCard> GetCardsByIdAsync(string id)
        {
            try
            {
                var cardId = Guid.Parse(id);

                return await _context.LoyaltyCards.FirstOrDefaultAsync(c => c.ClientGuid == cardId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetCardsByIdAsync for Id {id}: {ex.Message}");
                return null;
            }
        }

    }
}
