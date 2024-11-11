using Bahrin.Harbour.Data.DBCollections;

namespace Bahrin.Harbour.Data.OutletDA
{
    public interface IOutletDA
    {
        Task AddOutletAsync(Outlet outlet);
        Task DeleteOutletAsync(Guid id);
        Task<List<Outlet>> GetAllOutletsAsync();
        Task<Outlet> GetOutletByIdAsync(Guid? id);
        Task<bool> OutletExistsAsync(Guid id);
        Task UpdateOutletAsync(Outlet outlet);
        Task<Outlet> GetOutletByRepresentativeIdAsync(Guid id);

    Task<bool> OutletStatusAsync(Guid Id, bool activate);

    }
}