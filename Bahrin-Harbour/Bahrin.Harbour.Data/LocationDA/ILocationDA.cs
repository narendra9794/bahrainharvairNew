using Bahrin.Harbour.Data.DBCollections;

namespace Bahrin.Harbour.Data.LocationDA
{
    public interface ILocationDA
    {
        Task<List<City>> GetCitiesByStateIdAsync(int stateId);
        Task<List<City>> GetCountriesAsync();
        Task<List<City>> GetStatesByCountryIdAsync(int CountryId);
    }
}