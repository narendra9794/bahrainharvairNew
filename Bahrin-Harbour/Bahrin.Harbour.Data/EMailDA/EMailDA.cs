using Bahrin.Harbour.Data.DataContext;
using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Model.EmailModel;
using Bahrin.Harbour.Model.ProjectSession;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Data.EMailDA
{
    public class EMailDA : IEMailDA
    {
        private readonly BahrinHarbourContext _context;
        private readonly ILogger<EMailDA> _logger;

        public EMailDA(BahrinHarbourContext context, ILogger<EMailDA> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> SaveOrUpdateEmailSettingAsync(Email mail)
        {
            try
            {

                var emailSetting = _context.Email.FirstOrDefault();

                if (emailSetting == null)
                {
                    mail.Id = Guid.NewGuid();
                    mail.CreatedDate = DateTime.Now;
                    mail.Createdby = ProjectSessionModel.admin._id;
                    mail.AciveStatus = true;

                    await _context.Email.AddAsync(mail);
                }
                else
                {
                    emailSetting.SenderDisplayName = mail.SenderDisplayName;
                    emailSetting.UserName = mail.UserName;
                    emailSetting.Password = mail.Password;
                    emailSetting.UseDefaultCredentials = mail.UseDefaultCredentials;
                    emailSetting.EnableSSL = mail.EnableSSL;
                    emailSetting.IsHTMLBody = mail.IsHTMLBody;
                    emailSetting.SenderAddress = mail.SenderAddress;
                    emailSetting.Host = mail.Host;
                    emailSetting.Port = mail.Port;
                    emailSetting.TestEmailTo = mail.TestEmailTo;
                    emailSetting.CC = mail.CC;
                    emailSetting.AciveStatus = true;
                    emailSetting.DateModified = DateTime.Now;
                    emailSetting.ModifiedBy = ProjectSessionModel.admin._id;
                    _context.Email.Update(emailSetting);
                }

                var saved = await _context.SaveChangesAsync();

                return saved > 0;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Email> GetEmailSettingAsync()
        {
            try
            {
                return _context.Email.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
