using Bahrin.Harbour.Model.AppUserAuth;
using Bahrin.Harbour.Model.ClientModel;
using Bahrin.Harbour.Service.AppUserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZXing;

namespace Bahrin_Harbour.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("[area]/[controller]/[action]")]
    //  [AdminAuthorize]
    public class UserManagementController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly IOutletService _outletService;

        public UserManagementController(IAppUserService appUserService, IOutletService outletService)
        {
            _appUserService = appUserService;
            _outletService = outletService;
        }
        public async Task<IActionResult> AppUsers()
        {
            return View();
        }
        public async Task<IActionResult> GetAllAppUsers()
        {
            var users = await _appUserService.GetAllAppUsersAsync();
      var data = from c in users
                 select new[]
             {
                       c.FirstName+"",
                       c.Email+ "",
                       c.OutletAssigned+"",
                       c.IsActive+"",
                       c._id+"",
                       c.ProfileImageLink+""
                   };

      return Json(new
      {

        iTotalRecords = users.Count(),
        iTotalDisplayRecords = users.Count(),
        aaData = data
      });
    }
        public async Task<IActionResult> GetAppUserById(string id)
        {
            var appUser = await _appUserService.GetAppUserByIdAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return Ok(appUser);
        }

        public async Task<IActionResult> CreateAppUser()
        {
            var outlets = await _outletService.GetAllOutletsAsync();

            var outletSelectList = outlets.Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Name
            }).ToList();

            ViewBag.Outlets = new SelectList(outletSelectList, "Value", "Text");

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAppUser(AppUserViewModel appUserViewModel)
        {     
          var result =  await _appUserService.AddAppUserAsync(appUserViewModel);
            if (result.status)
            {
                TempData["success"] = result.message;
                return Ok(result);

            }
            TempData["error"] = result.message;
            return Ok(result);
        }
        public async Task<IActionResult> UpdateAppUser(string id)
        {
            AppUserViewModel viewModel = new AppUserViewModel();
             viewModel = await _appUserService.GetAppUserByIdAsync(id);
      var outlets = await _outletService.GetAllOutletsAsync(); 
      var outletSelectList = outlets.Select(o => new SelectListItem
      {
        Value = o.Id.ToString(),
        Text = o.Name
      }).ToList();

      ViewBag.Outlets = new SelectList(outletSelectList, "Value", "Text");
      return View("UpdateAppUser", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAppUser(AppUserViewModel appUserViewModel)
        {
          var result = await _appUserService.UpdateAppUserAsync(appUserViewModel);

           if (result.status)
           {
             TempData["success"] = result.message;
             return Ok(result);
          
           }
           TempData["error"] = result.message;
           return Ok(result);
      ////if (ModelState.IsValid)
      ////{
      //    var result = await _appUserService.UpdateAppUserAsync(appUserViewModel);
      //    if (result.status)
      //    {
      //        return RedirectToAction("Administration/UserManagement/AppUsers");
      //    }
      //    //ModelState.AddModelError("", result.message);
      ////}
      //return View(appUserViewModel);
    }

        [HttpPost]
        public async Task<IActionResult> DeleteAppUser(string id)
        {
            var result = await _appUserService.HardDeleteAppUserAsync(id);
            if (result.status)
            {
                return RedirectToAction("Administration/UserManagement/AppUsers");
            }
            return NotFound(result.message);
        }

        [HttpPost]
        public async Task<IActionResult> DeActivateUser(string id)
        {
            var result = await _appUserService.DeActivateUser(id);
            if (result.status)
            {

              return Json(new { status = true, message = "User deactivated successfully" });
       
            }
          return Json(new { status = false, message = "User not found or deactivation failed" });
        }
    public async Task<IActionResult> ViewUser(string id)
    {
      AppUserViewModel viewModel = new AppUserViewModel();
      viewModel = await _appUserService.GetAppUserByIdAsync(id);
      var outlets = await _outletService.GetAllOutletsAsync();
      var outletSelectList = outlets.Select(o => new SelectListItem
      {
        Value = o.Id.ToString(),
        Text = o.Name
      }).ToList();

      ViewBag.Outlets = new SelectList(outletSelectList, "Value", "Text");
      return View(viewModel);

    }
  }
}
