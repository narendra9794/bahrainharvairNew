using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Service.AccoutService;
using Microsoft.AspNetCore.Mvc;

namespace Bahrin_Harbour.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService iAccountService;
        public AccountController(IAccountService AccountService)
        {
            iAccountService = AccountService;
        }

        public IActionResult CreateAdminUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdminUser(AdminUserModel adminUserModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

           await iAccountService.CreateUser(adminUserModel);

           return Ok();
        }
    }
}
