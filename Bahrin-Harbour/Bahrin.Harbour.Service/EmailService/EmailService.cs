using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Data.EMailDA;
using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.EmailModel;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Text;
using MailKit.Net.Smtp;

namespace Bahrin.Harbour.Service.EmailService
{
    public class EmailService : IEmailService
    {
        private const string templatePath = @"wwwroot/Email/Templates/{0}.html";
        private readonly SMTPConfigModel _configmodel;
        private readonly IEMailDA _eMailDA;

        public EmailService(IOptions<SMTPConfigModel> configmodel, IEMailDA eMailDA)
        {
            _configmodel = configmodel.Value;
            _eMailDA = eMailDA;
            _configmodel = GetMailSetting().Result;

        }

        public async Task SendEmailForgetPassword(UserMailOptions mailOptions)
        {
            mailOptions.Subject = UpdatePlaceHolders("{{Name}} Password Reset Request", mailOptions.PlaceHolders);
            mailOptions.Body = UpdatePlaceHolders(FindTemplate("ForgetPasswordSendMail"), mailOptions.PlaceHolders);

            await SendEmail(mailOptions);
        }
        public async Task SendOTPEmailToUserOnForgetPassword(UserMailOptions mailOptions)
        {
            mailOptions.Subject = UpdatePlaceHolders("Password Reset Request", mailOptions.PlaceHolders);
            mailOptions.Body = UpdatePlaceHolders(FindTemplate("SendOTPMailOnForgetPassword"), mailOptions.PlaceHolders);

            await SendEmail(mailOptions);
        }

        public async Task SendConfirmationEmailOnSignUp(UserMailOptions mailOptions)
        {
            mailOptions.Subject = UpdatePlaceHolders("{{FirstName}} {{LastName}} Account Created Successfully", mailOptions.PlaceHolders);
            mailOptions.Body = UpdatePlaceHolders(FindTemplate("ConfirmEmailOnSignUp"), mailOptions.PlaceHolders);

            await SendEmail(mailOptions);
        }
 

        /// <summary>
        /// Send Mail Configuration
        /// </summary>
        /// <param name="userMailOptions"></param>
        /// <returns></returns>
      /*  private async Task SendEmail(UserMailOptions userMailOptions)
        {
            MailMessage mailMessage = new MailMessage()
            {
                Subject = userMailOptions.Subject,
                Body = userMailOptions.Body,
                IsBodyHtml = _configmodel.IsHTMLBody,
                From = new MailAddress(_configmodel.SenderAddress, _configmodel.SenderDisplayName)
            };

            foreach (var mailAddress in userMailOptions.ToEmail)
            {
                mailMessage.To.Add(mailAddress);
            }


            NetworkCredential networkCredential = new NetworkCredential()
            {
                UserName = _configmodel.UserName,
                Password = _configmodel.Password
            };

            SmtpClient smtpClient = new SmtpClient(_configmodel.Host, _configmodel.Port)
            {
                EnableSsl = _configmodel.EnableSSL,
                UseDefaultCredentials = _configmodel.UseDefaultCredentials,
                Credentials = networkCredential
            };

            mailMessage.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mailMessage);
        }
*/
        private string FindTemplate(string templateName)
        {
            var template = File.ReadAllText(string.Format(templatePath, templateName));
            return template;
        }
        public string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var pair in keyValuePairs)
                {
                    if (text.Contains(pair.Key))
                    {
                        text = text.Replace(pair.Key, pair.Value);
                    }
                }
                return text;
            }
            return text;
        }


        /// GetMail setting Segment
        ///////////////
        ////////////////
        ///
        public async Task<StatusModel> SaveOrUpdateMailSetting(SMTPConfigModel sMTPConfig)
        {
            StatusModel status = new StatusModel();
            var email = new Email
            {
                SenderAddress = sMTPConfig.SenderAddress,
                SenderDisplayName = sMTPConfig.SenderDisplayName,
                UserName = sMTPConfig.UserName,
                Password = sMTPConfig.Password,
                Port = sMTPConfig.Port,
                Host = sMTPConfig.Host,
                EnableSSL = sMTPConfig.EnableSSL,
                UseDefaultCredentials = sMTPConfig.UseDefaultCredentials,
                IsHTMLBody = sMTPConfig.IsHTMLBody,
                CC = sMTPConfig.CC,
                TestEmailTo = sMTPConfig.TestEmailTo
            };

           var response = await _eMailDA.SaveOrUpdateEmailSettingAsync(email);
            status.status = response;

            if (response)
            {
                status.message = "Mail settings successfully updated.";
                return status;
            }
            status.message = "Mail settings Not updated.";
            return status;

        }

        public async Task<SMTPConfigModel> GetMailSetting()
        {
            var Setting = await _eMailDA.GetEmailSettingAsync();
            if (Setting == null)
            {
                return null;
            }
            var email = new SMTPConfigModel
            {
                SenderAddress = Setting.SenderAddress,
                SenderDisplayName = Setting.SenderDisplayName,
                UserName = Setting.UserName,
                Password = Setting.Password,
                Port = Setting.Port,
                Host = Setting.Host,
                EnableSSL = Setting.EnableSSL,
                UseDefaultCredentials = Setting.UseDefaultCredentials,
                IsHTMLBody = Setting.IsHTMLBody,
                CC = Setting.CC,
                TestEmailTo = Setting.TestEmailTo
            };

           return email;
        }
          public async Task<StatusModel> SendTestMail()
        {
            var Setting = await _eMailDA.GetEmailSettingAsync();
            StatusModel status = new StatusModel();

            UserMailOptions userMailOptions = new UserMailOptions();
            List<string> mailTo = new List<string>();
            mailTo.Add(_configmodel?.TestEmailTo);
            userMailOptions.ToEmail = mailTo;
            userMailOptions.Subject = "Test Email: Verifying Your Email Configuration";
            userMailOptions.Body = FindTemplate("TestMail");

            try
            {
                await SendEmail(userMailOptions);
                status.status = true;
                status.message = $"Email successfully sent on email : {_configmodel?.TestEmailTo } ";
            }
            catch(Exception ex) 
            {
                status.status = false;
                status.message = $"Error occured on sending email. \n Exception : {ex.Message} \n Inner Exception : {ex.InnerException} \n Source Exception : {ex.Source}";
            }
            return status;

        }








        public async Task SendEmail(UserMailOptions userMailOptions)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_configmodel.SenderDisplayName, _configmodel.SenderAddress));
            foreach (var recipient in userMailOptions.ToEmail)
            {
                message.To.Add(new MailboxAddress("",recipient));
            }
            message.Subject = userMailOptions.Subject;
            message.Body = new TextPart("html")
            {
                Text = userMailOptions.Body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configmodel.Host, _configmodel.Port, _configmodel.EnableSSL);
                await client.AuthenticateAsync(_configmodel.UserName, _configmodel.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

    }
}
