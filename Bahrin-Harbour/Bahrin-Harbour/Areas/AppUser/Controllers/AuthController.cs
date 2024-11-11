using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.AppUserAuth;
using Bahrin.Harbour.Service.AccoutService;
using Bahrin.Harbour.Service.UserAccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ZXing;

namespace Bahrin_Harbour.Areas.AppUser.Controllers
{

    [Area("user")]
    [Route("[area]/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly IAccountService _accountService;

        public AuthController(IUserAccountService userAccountService, IAccountService AccountService)
        {
            _userAccountService = userAccountService;   
            _accountService = AccountService;   
        }
        /// <summary>
        /// Login User
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// 

        [HttpPost]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Signin(SignInModel data)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    SigninResponse response =await _userAccountService.Signin(data);

                    if (response.data != null)
                    {
                    
                        if (response.status.roles.Contains(Constants.AppUser))
                        {
                            Response.Headers["Authorization"] = $"Bearer {response.data.deviceToken}";

                            return Accepted(response);
                        }
                        else
                        {
                            Unauthorized();
                        }
                    }
                    else
                    {
                        return NotFound(response);
                    }
                }
                catch (KeyNotFoundException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return null;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModel passwordModel)
        {
            if (ModelState.IsValid)
            {
                var OtpMailSent = await _userAccountService.AppUserForgetPassword(passwordModel);

                return Ok(OtpMailSent);
            }
            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel resetmodel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.ResetpasswordOnForgetPassword(resetmodel);
                if (result.Success == Constants.True) return Ok(result);

                return new BadRequestObjectResult(result);
            }
            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel Changetmodel)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.UpdateUserPassword(Changetmodel);
                if (result.status == Constants.True) return Ok(result);

                return new BadRequestObjectResult(result);
            }
            return BadRequest();
        }
    }
}
