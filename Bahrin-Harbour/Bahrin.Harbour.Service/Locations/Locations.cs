using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Data.LocationDA;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Text;
using static Bahrin.Harbour.Helper.EnumHelper;
using static LocationService;
public class LocationService : ILocationService
{
    private readonly string _filePath = @"wwwroot/json/locations";
    private readonly IMemoryCache _cache;
    private readonly ILocationDA _locationDa;

    public LocationService(IMemoryCache cache, ILocationDA locationDa)
    {
        _cache = cache;
        _locationDa = locationDa;
    }

    public async Task<List<Country>> GetCountriesAsync()
    {
        var cacheKey = $"countries_cache_key";

        if (_cache.TryGetValue(cacheKey, out List<Country> country))
        {
            return country;
        }

        var path = Path.Combine(_filePath, "Countries.json");
        var json = await File.ReadAllTextAsync(path);
        var countries = JsonConvert.DeserializeObject<List<Country>>(json).ToList();


        _cache.Set(cacheKey, countries);

        return countries;
    }

    public async Task<List<State>> GetStatesAsync(int CountryId)
    {
        var cacheKey = $"states_cache_key_{CountryId}";

        if (_cache.TryGetValue(cacheKey, out List<State> state))
        {
            return state;
        }
        var path = Path.Combine(_filePath, "States.json");
        var json = await File.ReadAllTextAsync(path);
        var states = JsonConvert.DeserializeObject<List<State>>(json).Where(x=>x.CountryId ==CountryId).ToList();

        _cache.Set(cacheKey, states);

        return states;
    }

    public async Task<List<City>> GetCitiesAsync(int stateId)
    {
        var cacheKey = $"cities_cache_key_{stateId}";

        if (_cache.TryGetValue(cacheKey, out List<City> cities))
        {
            return cities;
        }
/*
        var path = Path.Combine(_filePath, "Cities.json");
        //var path1 = Path.Combine(_filePath, "CitiesQuery.json");

        // await GenerateSqlScriptAsync(path, path1);
        var json = await File.ReadAllTextAsync(path);

        cities = JsonConvert.DeserializeObject<List<City>>(json).Where(x => x.StateId == stateId).ToList();*/


         cities =await _locationDa.GetCitiesByStateIdAsync(stateId);
        _cache.Set(cacheKey, cities);

        return cities;
    }


/*    public async Task GenerateSqlScriptAsync(string jsonFilePath, string outputFilePath)
    {
        // Read the JSON file
        var json = await File.ReadAllTextAsync(jsonFilePath);

        // Deserialize the JSON to a list of City objects
        var cities = JsonConvert.DeserializeObject<List<Cityi>>(json);

        // Use a StringBuilder to accumulate the SQL statements
        var sqlScript = new StringBuilder();

        // Add header for the SQL script
        sqlScript.AppendLine("USE YourDatabaseName;");
        sqlScript.AppendLine("GO");
        sqlScript.AppendLine("BEGIN TRANSACTION;");
        sqlScript.AppendLine();

        // Loop through each city and generate the INSERT statement
        foreach (var city in cities)
        {
            // Escape single quotes in strings (e.g. city names)
            string cityName = city.Name?.Replace("'", "''");
            string stateCode = city.StateCode?.Replace("'", "''");
            string stateName = city.StateName?.Replace("'", "''");
            string countryCode = city.CountryCode?.Replace("'", "''");
            string countryName = city.CountryName?.Replace("'", "''");
            string latitude = city.Latitude ?? "NULL"; // Handle null values
            string longitude = city.Longitude ?? "NULL"; // Handle null values

            // Build the INSERT statement
            sqlScript.AppendLine($@"
        INSERT INTO Cities (Id, Name, StateId, StateCode, StateName, CountryId, CountryCode, CountryName, Latitude, Longitude, WikiDataId)
        VALUES ({city.Id}, '{cityName}', {city.StateId}, '{stateCode}', '{stateName}', {city.CountryId}, '{countryCode}', '{countryName}', {latitude}, {longitude}, '{city.WikiDataId}');
        ");
        }

        // Add footer for the SQL script
        sqlScript.AppendLine();
        sqlScript.AppendLine("COMMIT;");
        sqlScript.AppendLine("GO");

        // Write the SQL script to the output file
        await File.WriteAllTextAsync(outputFilePath, sqlScript.ToString());
    }
*/

    public class Country
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]

        public string Name { get; set; }
    }
    public class State
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]

        public string Name { get; set; }
        [JsonProperty("country_id")]

        public int CountryId { get; set; }
    }
/*    public class City
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
     
        [JsonProperty("state_id")]
        public int StateId { get; set; }
    }
*/

/*public class Cityi
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("state_id")]
    public int StateId { get; set; }

    [JsonProperty("state_code")]
    public string StateCode { get; set; }

    [JsonProperty("state_name")]
    public string StateName { get; set; }

    [JsonProperty("country_id")]
    public int CountryId { get; set; }

    [JsonProperty("country_code")]
    public string CountryCode { get; set; }

    [JsonProperty("country_name")]
    public string CountryName { get; set; }

    [JsonProperty("latitude")]
    public string Latitude { get; set; }

    [JsonProperty("longitude")]
    public string Longitude { get; set; }

    [JsonProperty("wikiDataId")]
    public string WikiDataId { get; set; }
}*/


}
