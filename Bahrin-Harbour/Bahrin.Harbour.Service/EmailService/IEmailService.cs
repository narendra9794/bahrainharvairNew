using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.EmailModel;
using static Bahrin.Harbour.Model.EmailModel.EmailModel;

namespace Bahrin.Harbour.Service.EmailService
{
    public interface IEmailService
    {
        Task SendConfirmationEmailOnSignUp(UserMailOptions mailOptions);
        Task  SendEmailForgetPassword(UserMailOptions mailOptions);
        string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs);
        Task SendOTPEmailToUserOnForgetPassword(UserMailOptions mailOptions);
        Task<StatusModel> SaveOrUpdateMailSetting(SMTPConfigModel sMTPConfig);
        Task<SMTPConfigModel> GetMailSetting();
        Task<StatusModel> SendTestMail();
    }
}