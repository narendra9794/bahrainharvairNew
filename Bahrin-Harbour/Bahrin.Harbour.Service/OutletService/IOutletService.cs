using Bahrin.Harbour.Model.OutletModel;

public interface IOutletService
{
    Task<OutletViewModel> CreateOrUpdateOutletAsync(OutletViewModel model);
    Task DeleteOutletByIdAsync(Guid id);
    Task<List<OutletViewModel>> GetAllOutletsAsync();
    Task<OutletViewModel> GetOutletByIdAsync(Guid id);
    Task<bool> OutletStatusAsync(string id, bool activate);
}