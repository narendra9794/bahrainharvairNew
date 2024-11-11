using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Data.OutletDA;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.AppUserAuth;
using Bahrin.Harbour.Model.EmailModel;
using Bahrin.Harbour.Service.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Bahrin.Harbour.Model.EmailModel.EmailModel;

namespace Bahrin.Harbour.Service.UserAccountService
{
    public class UserAccountService : IUserAccountService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageService _imageService;
        private readonly IEmailService _email;
        private readonly IOutletDA _outletDa;

        public UserAccountService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IImageService imageService, IEmailService email, IOutletDA outlet)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _imageService = imageService;
            _email = email;
            _outletDa = outlet;
        }
    public async Task<SigninResponse> Signin(SignInModel data)
        {
            SigninResponse response = new SigninResponse();

            try
            {
                ApplicationUser userModel = await _userManager.FindByEmailAsync(data.email);

               
                if (userModel != null)
                {
                    if (!userModel.IsActive)
                    {
                       response.status = new StatusModel
                        {
                            status = false,
                            message = "This user is blocked by admin."
                        };

                        return response;
                    }

                    var result = await _signInManager.PasswordSignInAsync(data.email, data.password, data.rememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(userModel);

                        response.data = new AppUserViewModel
                        {
                            _id = Guid.Parse(userModel.Id),
                            Email = userModel.Email,
                            FirstName = userModel.FirstName,
                            LastName = userModel.LastName,
                            Address = userModel.Address,
                            State = userModel.State,
                            Country = userModel.Country,
                            IsActive = userModel.IsActive,
                            ProfileImageLink = _imageService.GenerateImageUrl(userModel.ProfileImagePathfolder, userModel.ProfileImageFileName),
                            PhoneNumber = userModel.PhoneNumber,
                        };
                        response.data.deviceToken = GenerateJwtToken(userModel);

                        if(roles.Contains(Constants.AppUser))
                        {

                            if (userModel.OutletId != null && userModel.OutletId != Guid.Empty)
                            {
                                var outlet = await _outletDa.GetOutletByIdAsync((Guid)userModel.OutletId);

                                response.data.OutletAssigned = outlet.Name;
                                response.data.AvailablePercentage = Math.Round(outlet.DiscountPercentage).ToString();
                            }
                        }

                        response.status = new StatusModel
                        {
                            status = true,
                            message = "Login successfully",
                            roles = roles.ToList()
                        };
                    }
                    else
                    {
                        response.status = new StatusModel
                        {
                            status = false,
                            message = "Invalid credentials"
                        };
                    }
                }
                else
                {
                    response.status = new StatusModel
                    {
                        status = false,
                        message = "Details not found"
                    };
                }
            }
            catch (Exception ex)
            {
                response.status = new StatusModel
                {
                    status = false,
                    message = "An error occurred during login"
                };
            }

            return response;
        }


        private string GenerateJwtToken(ApplicationUser userModel)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("12345678901234567890123456789012");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, userModel.FirstName +" "+ userModel.LastName),
                new Claim(ClaimTypes.NameIdentifier, userModel.Id),
                new Claim(ClaimTypes.Role, userModel.Role),
                new Claim(ClaimTypes.Email, userModel.Email),
                new Claim(ClaimTypes.Country, userModel.Country),
                new Claim(ClaimTypes.MobilePhone, userModel.PhoneNumber),
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "shubham.m@mishainfotech.com",
                Audience = "rashi.k@mishainfotech.com"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<ResetPasswordResponse> AppUserForgetPassword(ForgetPasswordModel forgetPasswordModel)
        {

            ApplicationUser userModel = await _userManager.FindByEmailAsync(forgetPasswordModel.email);
            ResetPasswordResponse statusModel = new ResetPasswordResponse();

            if (userModel != null)
            {
                var otp = Helper.Helper.GenerateOTPNumber().ToString();

                await SendOTPEmailOnForgetPassword(userModel.FirstName, userModel.Email, otp);

                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(userModel);
                passwordResetToken = Helper.Helper.EnryptToken(passwordResetToken);

                statusModel.status = Constants.True;
                statusModel.message = Constants.otpSentOnEmail;
                statusModel.otp = otp;
                statusModel.token = passwordResetToken;
                statusModel.UserId = userModel.Id;

                return statusModel;
            }
            statusModel.status = Constants.False;
            statusModel.message = Constants.NotExistEmailPhone;

            return statusModel;

        }

        public async Task SendOTPEmailOnForgetPassword(string RepresentativeName, string email, string otp)
        {
            UserMailOptions mailOptions = new UserMailOptions()
            {
                ToEmail = new List<string>() { email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{OTP}}", otp),
                    new KeyValuePair<string, string>("{{RepresentativeName}}", RepresentativeName),
                    new KeyValuePair<string, string>("{{SupportEmail}}",  "support@bahrainharbour.com"),
                    new KeyValuePair<string, string>("{{SupportContactNumber}}",  "+1-000-000-0000"),
                }
            };
            try
            {
                await _email.SendOTPEmailToUserOnForgetPassword(mailOptions);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

     
    }
}
