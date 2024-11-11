using Bahrin.Harbour.Model.OutletModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Bahrin_Harbour.Areas.Administration.Controllers
{

    [Area("Administration")]
    [Route("[area]/[controller]/[action]")]
    public class OutletController : Controller
    {
        private readonly IOutletService _outletService;
        public OutletController(IOutletService outletService)
        {
            _outletService = outletService;
        }

        public async Task<IActionResult> Index()
        {
            //var outlets = await _outletService.GetAllOutletsAsync();
            return View();
        }

        public async Task<IActionResult> ViewOutlet(Guid id)
        {
            OutletViewModel model = await _outletService.GetOutletByIdAsync(id); 

            return View( model);
        }

         public async Task<IActionResult> CreateOrUpdateOutletAsync(Guid id)
        {
            OutletViewModel model = id == Guid.Empty
                ? new OutletViewModel()
                : await _outletService.GetOutletByIdAsync(id); 

            return View("CreateOrUpdateOutletAsync", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateOutletAsync(OutletViewModel model, IFormFile? ImageFile)
        {
      //if (!ModelState.IsValid)
      //    return View(model);
      try
      {
        await _outletService.CreateOrUpdateOutletAsync(model);
        return Ok();
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { error = "An error occurred while processing your request." });
      }
        }
        public async Task<IActionResult> GetAllOutletsAsync()
        {
            var outlets = await _outletService.GetAllOutletsAsync();
            var data = from c in outlets
                 select new[]
                 {
                       c.Id.ToString(),
                       c.Name + "",
                       c.Country+"",
                       c.DiscountPercentage+"",
                       c.RepresentativeName+"",
                       c.AciveStatus+"",
                       c.ProfileImageLink+""

                  };

              return Json(new
              {
             
                iTotalRecords = outlets.Count(),
                iTotalDisplayRecords = outlets.Count(),
                aaData = data
              });
        }

        public async Task<IActionResult> DeleteOutletByIdAsync(Guid id)
        {
            await _outletService.DeleteOutletByIdAsync(id);
            return Ok();
        }

    [HttpPost]
    public async Task<IActionResult> ChangeOutletStatus(string id, bool activate)
    {
      try
      {
        var success = await _outletService.OutletStatusAsync(id, activate);

        if (success)
        {
          
            return Ok();
        }
        else
        {
          return Json(new { status = false, message = activate ? "Failed to activate Outlet." : "Failed to deactivate Outlet." });
        }
      }
      catch (Exception ex)
      {
        return Json(new { status = false, message = activate ? "An error occurred while activating the client." : "An error occurred while deactivating the Outlet." });
      }
    }
  }
}
