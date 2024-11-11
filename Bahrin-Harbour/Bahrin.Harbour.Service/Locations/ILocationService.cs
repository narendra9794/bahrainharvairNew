
using Bahrin.Harbour.Data.DBCollections;
using static LocationService;

public interface ILocationService
{
    Task<List<Country>> GetCountriesAsync();
    Task<List<State>> GetStatesAsync(int CountryId);
    Task<List<City>> GetCitiesAsync(int stateId);
}