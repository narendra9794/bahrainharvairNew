using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.AppUserAuth;

namespace Bahrin.Harbour.Service.UserAccountService
{
    public interface IUserAccountService
    {
        Task<SigninResponse> Signin(SignInModel data);
        Task<ResetPasswordResponse> AppUserForgetPassword(ForgetPasswordModel forgetPasswordModel);
    }
}