using Microsoft.AspNetCore.Mvc;

namespace Bahrin_Harbour.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("[area]/[controller]/[action]")]
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountries()
        {
            var countries = await _locationService.GetCountriesAsync();
            return Json(countries);
        }

        [HttpGet]
        public async Task<IActionResult> GetStates(int countryId)
        {
            var states = await _locationService.GetStatesAsync(countryId);
            return Json(states);
        }

        [HttpGet]
        public async Task<IActionResult> GetCities(int stateId)
        {
            var cities = await _locationService.GetCitiesAsync(stateId);
            return Json(cities);
        }
    }

}