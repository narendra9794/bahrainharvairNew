using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Data.OutletDA;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.AppUserAuth;
using Bahrin.Harbour.Model.ClientModel;
using Bahrin.Harbour.Model.EmailModel;
using Bahrin.Harbour.Model.OutletModel;
using Bahrin.Harbour.Model.ProjectSession;
using Bahrin.Harbour.Service.EmailService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static Bahrin.Harbour.Model.EmailModel.EmailModel;

namespace Bahrin.Harbour.Service.AppUserService
{
    public class AppUserService : IAppUserService
    {
        private readonly UserManager<ApplicationUser> iUserManager;
        private readonly RoleManager<IdentityRole> iRoleManager;
        private readonly ILogger<AppUserService> _logger;
        private readonly IEmailService _email;
        private readonly IImageService _image;
        private readonly IOutletDA _outletDa;
        private readonly string imageFolderName = Constants.AppUserProfileImages;
        public AppUserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AppUserService> logger, IEmailService email, IImageService image, IOutletDA outletDa)
        {
            iUserManager = userManager;
            iRoleManager = roleManager;
            _logger = logger;
            _email = email;
            _image = image;
            _outletDa = outletDa;

        }
        public async Task<StatusModel> AddAppUserAsync(AppUserViewModel appUser)
        {
            ApplicationUser applicationUser = new ApplicationUser
            {
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email,
                Address = appUser.Address,
                Country = appUser.Country,
                CreatedBy = ProjectSessionModel.admin._id.ToString(),
                EmailConfirmed = Constants.True,
              //IsActive = appUser.IsActive,
              IsActive = appUser._id != Guid.Empty ? appUser.IsActive : true,
              PhoneNumber = appUser.PhoneNumber,
                Role = Constants.AppUser,
                State = appUser.State,
                UserName = appUser.Email,
                CreatedOn = DateTime.Now,
                PhoneNumberConfirmed = Constants.True,
              City = appUser.City,
              ZipCode = appUser.ZipCode

            };
            if (appUser.OutletId != null)
            {
                applicationUser.OutletId = Guid.Parse(appUser.OutletId);

            }
            if (appUser.ProfileImageFile != null)
            {
                applicationUser.ProfileImageFileName = await _image.SaveImageAsync(appUser.ProfileImageFile, imageFolderName);
                applicationUser.ProfileImagePathfolder = imageFolderName;
            }

            var result = await AddAppUserAsync(applicationUser);

            
            if (result.status != true && applicationUser.ProfileImageFileName != null)
            {
              _image.DeleteImage(imageFolderName, applicationUser.ProfileImageFileName);
            }

            return result;
        }
        private async Task<StatusModel> AddAppUserAsync(ApplicationUser appUser)
        {
            try
            {
                var outletName = await _outletDa.GetOutletByIdAsync(appUser.OutletId);
                StatusModel model = new StatusModel();

                if (!await iRoleManager.RoleExistsAsync(Constants.AppUser))
                {
                    await iRoleManager.CreateAsync(new IdentityRole(Constants.AppUser));
                }

                var existingUser = await iUserManager.FindByEmailAsync(appUser.Email);
                if (existingUser == null)
                {
                    var randomPassword = Helper.Helper.RandomPassword(10);
                    var result = await iUserManager.CreateAsync(appUser, randomPassword);
                    if (result.Succeeded)
                    {
                        var roleResult = await iUserManager.AddToRoleAsync(appUser, Constants.AppUser);
                        if (roleResult.Succeeded)
                        {
                            model.status = Constants.True;
                            model.message = Constants.successfullyAdd;
                            try
                            {
                                await SendMailOnAccountCreation(outletName.Name, appUser, randomPassword);
                            }
                            catch
                            {
                                model.message = "User Successfully created an error occured to send email.";
                            }
                            return model;
                        }

                    }
                    else
                    {
                        model.status = Constants.False;
                        model.message = string.Join("\n ", result.Errors.Select(x => x.Description));
                        return model;
                    }
                }

                model.status = Constants.False;
                model.message = Constants.CollectionAlreadyExists;
                return model;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding client with ID {ClientId}", appUser.Email);
                throw;
            }
        }
        public async Task<AppUserViewModel> GetAppUserByIdAsync(string id)
        {
            try
            {
                var user = await iUserManager.FindByIdAsync(id);
                if (user != null)
                {
                    var userModel = new AppUserViewModel
                    {
                        _id = Guid.Parse(user.Id),
                        FirstName = user.FirstName,
                        Address = user.Address,
                        Country = user.Country,
                        Email = user.Email,
                        LastName = user.LastName,
                        IsActive = user.IsActive,
                        PhoneNumber = user.PhoneNumber,
                        ProfileImageLink = string.IsNullOrEmpty(user.ProfileImageFileName) ? "" : _image.GenerateImageUrl(user.ProfileImagePathfolder, user.ProfileImageFileName),
                        State = user.State,
                        OutletId = user.OutletId.ToString(),
                        City= user.City,
                        ZipCode=user.ZipCode,
                    };

                    return userModel;
                }
                return new AppUserViewModel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user with ID {Id}", id);
                throw;
            }
        }

        public async Task<List<AppUserViewModel>> GetAllAppUsersAsync()
        {
            try
            {
                var Users = await Task.FromResult(iUserManager.Users.Where(x => x.Role == Constants.AppUser).ToList());
                var Outlet = await _outletDa.GetAllOutletsAsync();
                var userModel = Users.OrderByDescending(x => x.CreatedOn).Select(x => new AppUserViewModel
                
                    {
                    _id = Guid.Parse(x.Id),
                    FirstName = x.FirstName,
                    Address = x.Address,
                    Country = x.Country,
                    Email = x.Email,
                    ProfileImageLink = string.IsNullOrEmpty(x.ProfileImageFileName) ? "" : _image.GenerateImageUrl(x.ProfileImagePathfolder, x.ProfileImageFileName),
                    LastName = x.LastName,
                    OutletAssigned = Outlet?.FirstOrDefault(y=>y.Id == x.OutletId)?.Name,
                    IsActive = x.IsActive,
                    PhoneNumber = x.PhoneNumber,
                    State = x.State
                }).ToList();
        
        return userModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all users");
                throw;
            }
        }

        // Update
        public async Task<StatusModel> UpdateAppUserAsync(AppUserViewModel appUser)
        {
            try
            {
                var existingUser = await iUserManager.FindByIdAsync(appUser._id.ToString());
                if (existingUser != null)
                {
                    //if (!string.IsNullOrEmpty(existingUser.ProfileImageFileName))
                    //{
                    //    existingUser.ProfileImageFileName = await _image.UpdateImageAsync((IFormFile)appUser.ProfileImageFile, imageFolderName, existingUser.ProfileImageFileName);
                    //}
                    existingUser.FirstName = appUser.FirstName;
                    existingUser.LastName = appUser.LastName;
                    existingUser.Email = appUser.Email;
                    existingUser.Address = appUser.Address;
                    existingUser.Country = appUser.Country;
                    existingUser.IsActive = appUser.IsActive;
                    existingUser.PhoneNumber = appUser.PhoneNumber;
                    existingUser.State = appUser.State;
                    existingUser.City = appUser.City;
                    existingUser.ZipCode = appUser.ZipCode;
          existingUser.OutletId = Guid.Parse(appUser.OutletId);

          if (appUser.ProfileImageFile != null)
                    {
                        existingUser.ProfileImageFileName = await _image.SaveImageAsync((IFormFile)appUser.ProfileImageFile, imageFolderName);
                        existingUser.ProfileImagePathfolder = imageFolderName;
                    }

                    var result = await iUserManager.UpdateAsync(existingUser);
                    if (result.Succeeded)
                    {
                        return new StatusModel { status = Constants.True, message = Constants.successfullyUpdate };
                    }
                    else
                    {
                        return new StatusModel { status = Constants.False, message = string.Join(", ", result.Errors.Select(x => x.Description)) };
                    }
                }
                return new StatusModel { status = Constants.False, message = Constants.NotExistEmailPhone };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user with ID {UserId}", appUser.Email);
                throw;
            }
        }

        public async Task<StatusModel> DeActivateUser(string userId)
        {
            try
            {
                var existingUser = await iUserManager.FindByIdAsync(userId);
                if (existingUser != null)
                {
                    existingUser.IsActive = existingUser.IsActive ? false : true;
                    var result = await iUserManager.UpdateAsync(existingUser);
                    if (result.Succeeded)
                    {
                        return new StatusModel { status = Constants.True, message = Constants.successfullyDeleted };
                    }
                    else
                    {
                        return new StatusModel { status = Constants.False, message = string.Join(", ", result.Errors.Select(x => x.Description)) };
                    }
                }
                return new StatusModel { status = Constants.False, message = Constants.NotExistEmailPhone };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user with ID {UserId}", userId);
                throw;
            }
        }
        public async Task<StatusModel> HardDeleteAppUserAsync(string userId)
        {
            try
            {
                var existingUser = await iUserManager.FindByIdAsync(userId);
                if (existingUser != null)
                {
                    if (!string.IsNullOrEmpty(existingUser.ProfileImageFileName))
                    {
                       if(await Helper.Helper.FileExistsAsync(Path.Combine(imageFolderName, existingUser.ProfileImageFileName)))
                        {
                            _image.DeleteImage(imageFolderName, existingUser.ProfileImageFileName);
                        }
                    }
                    var result = await iUserManager.DeleteAsync(existingUser);
                    if (result.Succeeded)
                    {
                        return new StatusModel { status = Constants.True, message = Constants.successfullyDeleted };
                    }
                    else
                    {
                        return new StatusModel { status = Constants.False, message = string.Join(", ", result.Errors.Select(x => x.Description)) };
                    }
                }
                return new StatusModel { status = Constants.False, message = Constants.NotExistEmailPhone };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while permanently deleting user with ID {UserId}", userId);
                throw;
            }
        }
        public async Task<bool> SendMailOnAccountCreation(string outletName, ApplicationUser user, string Password)
        {
            UserMailOptions mailOptions = new UserMailOptions()
            {
                ToEmail = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{FirstName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{LastName}}", user.LastName),
                    new KeyValuePair<string, string>("{{Email}}", user.Email ),
                    new KeyValuePair<string, string>("{{Password}}",  Password),
                    new KeyValuePair<string, string>("{{SupportEmail}}",  "support@bahrainharbour.com"),
                    new KeyValuePair<string, string>("{{SupportContactNumber}}",  "+1-000-000-0000"),
                    new KeyValuePair<string, string>("{{OutletName}}",  outletName)
                }
            };
            try
            {
                await _email.SendConfirmationEmailOnSignUp(mailOptions);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
