using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Model.AccountModel;
using PuthaganModel.Admin;

namespace Bahrin.Harbour.Service.AccoutService
{
    public interface IAccountService
    {
        Task<AdminSigninResponse> SignIn(SignInModel signInModel);
        Task CreateUser(AdminUserModel adminUserModel);
        Task<Response> ForgetPassword(ForgetPasswordModel model);
        Task<Response> ResetpasswordOnForgetPassword(ResetPasswordModel model);
        Task<AdminUserModel> GetAdminDetails();
        Task<StatusModel> UpdateAdminDetails(AdminUserModel userModel);
        Task<StatusModel> UpdateAdminPassword(ResetNewPasswordModel userModel);
        Task<StatusModel> UpdateUserPassword(ChangePasswordModel userModel);
    }
}