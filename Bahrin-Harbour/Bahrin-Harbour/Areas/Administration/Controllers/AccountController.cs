using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.ProjectSession;
using Bahrin.Harbour.Service.AccoutService;
using Bahrin.Harbour.Service.EmailService;
using Bahrin_Harbour.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ZXing;
using static Bahrin.Harbour.Model.EmailModel.EmailModel;

namespace Bahrin_Harbour.Areas.Administration.Controllers
{
   // [AutoValidateAntiforgeryToken]
    [Area("Administration")]
    [Route("[area]/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAccountService iAccountService;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _email;
        private readonly SignInManager<ApplicationUser> iSignInManager;
        private readonly UserManager<ApplicationUser> iUserManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IAccountService AccountService, ILogger<HomeController> logger, IAccountService accountService, IConfiguration configuration, IEmailService email)
        {
            iSignInManager = signInManager;
            iUserManager = userManager;
            iAccountService = AccountService;
            _logger = logger;
            _configuration = configuration;
            _email = email;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return RedirectToAction("Signin")  ;
        }

        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(SignInModel signInModel)
        {
            if (ModelState.IsValid)
            {
                signInModel.isUpdate = true;

                if (string.IsNullOrWhiteSpace(signInModel.email) || string.IsNullOrWhiteSpace(signInModel.password))
                {
                    return View(signInModel);
                }

                var result = await iAccountService.SignIn(signInModel);

                if (result.status.status)
                {
                    HttpContext.Session.SetString("UserEmail", result.data.email);
                    HttpContext.Session.SetString("UserId", result.data._id.ToString());
                    HttpContext.Session.SetString("UserRoles", string.Join(",", result.status.roles));
                    ProjectSessionModel.admin = result.data;
                   // ProjectSessionModel.dateFormat = iSettingService.GetdateFormateSetttingModel();
                    ProjectSessionModel.localTimeZoneOffset = signInModel.localTimeZoneOffset;

                    if (result.status.roles.Contains(Constants.SuperAdmin))
                    {
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                }
                else
                {
                    ModelState.AddModelError("", result.status.message);
                    TempData["Error"] = result.status.message;
                }
            }
            TempData["Error"] =  "Invalid Credentials";

            return View(signInModel);
        }

        [AdminAuthorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            HttpContext.Session.Clear();
            Response.Cookies.Delete(".AspNetCore.Identity.Application");
            ProjectSessionModel.admin = null;
            TempData["Success"] = "Logged Out Successfully";
            return RedirectToAction("Signin", "Account");
        }
         
        public async Task<IActionResult> ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModel passwordModel)
        {
            if (ModelState.IsValid)
            {
                var ResetMailSent = await iAccountService.ForgetPassword(passwordModel);
                if (ResetMailSent.Success)
                {
                    TempData["Success"] = ResetMailSent.Message;
                    return View("Signin");
                }
                else
                {
                    TempData["Error"] = ResetMailSent.Message;
                    ModelState.Clear();
                    return View();
                }
            }
            return View();
        }

        public async Task<IActionResult> ResetPassword(string uid, string token)
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                UserId = uid,
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetmodel)
        {
            if(resetmodel.NewPassword != resetmodel.ConfirmPassword)
            {
                TempData["Error"] = "Password doesn't match.";
                return View(resetmodel);
            }

            var result = await iAccountService.ResetpasswordOnForgetPassword(resetmodel);

                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                    return View("SuccessfulPasswordReset");
                }
                else
                {
                    TempData["Error"] = result.Message;
                    return View(resetmodel);
                }
        }

        /// <summary>
        /// ////Create Admin Users by SuperAdmin
        /// </summary>
        /// <returns></returns>
        /// 
        [AdminAuthorize]
        public IActionResult UpdateAdminUser()
        {
            return View();
        }
       
        [AdminAuthorize]
        public async Task<IActionResult> GetAdminDetails()
        {
            var user = await iAccountService.GetAdminDetails();

            return Ok(user);
        }

        [AdminAuthorize]
        [HttpPost]
        public async Task<IActionResult> UpdateAdminUser(AdminUserModel adminUserModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

           var result = await iAccountService.UpdateAdminDetails(adminUserModel);

            return Ok(result);
        }
        
        [AdminAuthorize]
        public IActionResult UpdateAdminPassword()
        {
            return View();
        }
       

        [AdminAuthorize]
        [HttpPost]
        public async Task<IActionResult> UpdateAdminPassword(ResetNewPasswordModel adminUserModel)
        {
      

           var result = await iAccountService.UpdateAdminPassword(adminUserModel);

            if (result.status)
            {
                TempData["Success"] = result.message;
                return RedirectToAction("Dashboard", "Dashboard", new { area = "Administration" });
            }

            TempData["Error"] = result.message;
            return View(adminUserModel);
        }
    }
}
