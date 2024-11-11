using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.EmailModel;
using Bahrin.Harbour.Model.ProjectSession;
using Bahrin.Harbour.Service.EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Bahrin.Harbour.Service.AccoutService
{
    public class AccountService : IAccountService
    {

        private readonly SignInManager<ApplicationUser> iSignInManager;
        private readonly UserManager<ApplicationUser> iUserManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _email;
        private readonly IImageService _image;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailService email, IImageService image, IHttpContextAccessor httpContextAccessor)
        {
            iSignInManager = signInManager;
            iUserManager = userManager;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _email = email;
            _image = image;
        }

        public async Task<AdminSigninResponse> SignIn(SignInModel signInModel)
        {
            var adminResponse = new AdminSigninResponse();

            if (signInModel == null || string.IsNullOrWhiteSpace(signInModel.email) || string.IsNullOrWhiteSpace(signInModel.password))
            {
                adminResponse.status = new StatusModel
                {
                    status = false,
                    message = "Invalid input data"
                };
                return adminResponse;
            }

            try
            {
                var adminModel = await iUserManager.FindByEmailAsync(signInModel.email);

                if (adminModel != null)
                {
                    var result = await iSignInManager.PasswordSignInAsync(adminModel.UserName, signInModel.password, signInModel.rememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        var roles = await iUserManager.GetRolesAsync(adminModel);

                        adminResponse.data = new Model.AccountModel.AdminModel
                        {
                            email = adminModel.Email,
                            _id = Guid.Parse(adminModel.Id),
                            name = adminModel.FirstName + " " + adminModel.LastName,
                            image = _image.GenerateImageUrl(adminModel.ProfileImagePathfolder, adminModel.ProfileImageFileName)
                        };

                        adminResponse.status = new StatusModel
                        {
                            status = true,
                            message = "Login successfully",
                            roles = roles.ToList()
                        };
                    }
                    else
                    {
                        adminResponse.status = new StatusModel
                        {
                            status = false,
                            message = "Invalid credentials"
                        };
                    }
                }
                else
                {
                    adminResponse.status = new StatusModel
                    {
                        status = false,
                        message = "Details not found"
                    };
                }
            }
            catch (Exception ex)
            {
                adminResponse.status = new StatusModel
                {
                    status = false,
                    message = "An error occurred during login"
                };
            }

            return adminResponse;
        }
        public async Task<Response> ForgetPassword(ForgetPasswordModel model)
        {
            var user = iUserManager.FindByEmailAsync(model.email).Result;
            if (user != null)
            {
                var passwordResetToken = await iUserManager.GeneratePasswordResetTokenAsync(user);
                if (!string.IsNullOrEmpty(passwordResetToken))
                {
                    var token = Helper.Helper.EnryptToken(passwordResetToken);
                    var result = await SendMailOnForgetPassword(model, user, token);

                    return new Response { Success = result, Message = "Reset password email sent on your registered mail." };
                }
                return new Response { Success = false, Message = "Password Reset failed" }; ;
            }
            return new Response { Success = false, Message = "User does not exist" };
            
        }
        public async Task<bool> SendMailOnForgetPassword(ForgetPasswordModel model, ApplicationUser user, String token)
        {

            var request = _httpContextAccessor.HttpContext.Request;
            var host = request.Host.Value;
            string appDomain = request.Scheme;
            string ConfirmationEmail = _configuration.GetSection("Application:ForgetPassword").Value;

            UserMailOptions mailOptions = new UserMailOptions()
            {
                ToEmail = new List<string>() { model.email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{Name}}", user.FirstName ),
                    new KeyValuePair<string, string>("{{ResetLink}}", string.Format(appDomain+$"://{host}/" + ConfirmationEmail, user.Id, token) )
                }
            };
            try
            {
                await _email.SendEmailForgetPassword(mailOptions);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Response> ResetpasswordOnForgetPassword(ResetPasswordModel model)
        {
            //  string decodedtoken = Encoding.UTF8.GetString(Convert.FromBase64String(model.Token));
            var decodedtoken = Helper.Helper.DecryptToken(model.Token);

            if (Convert.ToInt32(decodedtoken.ValidUnixTime) > DateTimeOffset.Now.ToUnixTimeSeconds())
            {
                model.Token = decodedtoken.token;
                var user = iUserManager.FindByIdAsync(model.UserId).Result;
                if (user != null)
                {
                    var passwordResetToken = await iUserManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                    if (passwordResetToken.Succeeded)
                    {
                        return new Response { Success = true, Message = Constants.passwordSuccessfullyUpdate };
                    }
                    return new Response { Success = false, Message = string.Join(", ", passwordResetToken.Errors.Select(x => x.Description).ToList()) }; ;
                }
                return new Response { Success = false, Message = Constants.DataNotFound };
            }
            return new Response { Success = false, Message = "Password reset link expire" };
        }

        public async Task CreateUser(AdminUserModel adminUserModel)
        {
            var user = new ApplicationUser()
            {
                Email = adminUserModel.Email,
                PhoneNumber = adminUserModel.PhoneNumber,
                PhoneNumberConfirmed = adminUserModel.PhoneNumberConfirmed,
                FirstName = adminUserModel.FirstName,
                LastName = adminUserModel.LastName,
                Address = adminUserModel.Address,
                State = adminUserModel.State,
                Country = adminUserModel.Country,
                IsActive = adminUserModel.IsActive,
                CreatedBy = ProjectSessionModel.admin._id.ToString(),
                CreatedOn = DateTime.Now,
            };

            var existingUser = await iUserManager.FindByEmailAsync(user.Email);
            if (existingUser == null)
            {
                var password = Helper.Helper.RandomPassword(10);
                var result = await iUserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var res = await iUserManager.AddToRoleAsync(user, "Admin");
                    if (res.Succeeded)
                    {
                        ///send Email and password on email address
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Error creating user {user.Email}: {error.Description}");
                    }
                }
            }

        }
        public async Task<AdminUserModel> GetAdminDetails()
        {
            var user = await iUserManager.FindByIdAsync(ProjectSessionModel.admin._id.ToString());

            if (user == null)
            {
                return null;
            }
            var adminDetails = new AdminUserModel
            {
                id = Guid.Parse(user.Id),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = "SuperAdmin",
                Address = user.Address,
                City = user.City,
                State = user.State,
                Country = user.Country,
                DateOfBirth = user.DateOfBirth,
                Pin = user.Pin,
            };
            if (!string.IsNullOrEmpty(user.ProfileImageFileName))
            {
                adminDetails.ImageLink = _image.GenerateImageUrl(user.ProfileImagePathfolder, user.ProfileImageFileName);
            }
            return adminDetails;
        }

        public async Task<StatusModel> UpdateAdminDetails(AdminUserModel userModel)
        {
            StatusModel model = new StatusModel();
            var user = await iUserManager.FindByIdAsync(ProjectSessionModel.admin._id.ToString());

            if (user == null)
            {
                model.status = false;
                model.message = "No user found with this details";
                return model;
            }

            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.Email = userModel.Email;
            user.PhoneNumber = userModel.PhoneNumber;
            user.Role = "SuperAdmin";
            user.Address = userModel.Address;
            user.City = userModel.City;
            user.Country = userModel.Country;
            user.DateOfBirth = userModel.DateOfBirth;
            user.State = userModel.State;
            user.Pin = userModel.Pin;

            if (userModel.ImageFile != null)
            {
                user.ProfileImageFileName = await _image.UpdateImageAsync(userModel.ImageFile, Constants.SuperAdmin, user.ProfileImageFileName);
                user.ProfileImagePathfolder = Constants.SuperAdmin;
            }

            var result = await iUserManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                ProjectSessionModel.admin.name = user.FirstName + " " + user.LastName;
                ProjectSessionModel.admin.image = _image.GenerateImageUrl(Constants.SuperAdmin, user.ProfileImageFileName);
                model.status = true;
                model.message = "Admin Details Updated successfully.";
                return model;
            }
            model.status = true;
            model.message = "There are some problem to update details.";
            return model;
        }


        public async Task<StatusModel> UpdateAdminPassword(ResetNewPasswordModel userModel)
        {
            StatusModel model = new StatusModel();
            var user = await iUserManager.FindByIdAsync(ProjectSessionModel.admin._id.ToString());


            if (userModel.NewPassword != userModel.ConfirmPassword)
            {
                model.status = false;
                model.message = "Password doesn't matched.";
                return model;
            }

            if (user == null)
            {
                model.status = false;
                model.message = "No user found with this details";
                return model;
            }

            var result = await iUserManager.ChangePasswordAsync(user, userModel.OldPassword, userModel.NewPassword);

            if (result.Succeeded)
            {
                ProjectSessionModel.admin.name = user.FirstName + " " + user.LastName;
                ProjectSessionModel.admin.image = _image.GenerateImageUrl(Constants.SuperAdmin, user.ProfileImageFileName);
                model.status = true;
                model.message = "Admin Password Updated successfully.";
                return model;
            }

            model.status = false;
            model.message = string.Join(", ", result.Errors.Select(x => x.Description));
            return model;
        }


        public async Task<StatusModel> UpdateUserPassword(ChangePasswordModel userModel)
        {
            StatusModel model = new StatusModel();
            var user = await iUserManager.FindByIdAsync(userModel.UserId);


            if (userModel.NewPassword != userModel.ConfirmPassword)
            {
                model.status = false;
                model.message = "Password doesn't matched.";
                return model;
            }
            if (user == null)
            {
                model.status = false;
                model.message = "No user found with this details";
                return model;
            }

            var result = await iUserManager.ChangePasswordAsync(user, userModel.OldPassword, userModel.NewPassword);

            if (result.Succeeded)
            {
                model.status = true;
                model.message = "Password Updated successfully.";
                return model;
            }
            model.status = false;
            model.message = string.Join(", ", result.Errors.Select(x => x.Description));
            return model;
        }



    }
}
