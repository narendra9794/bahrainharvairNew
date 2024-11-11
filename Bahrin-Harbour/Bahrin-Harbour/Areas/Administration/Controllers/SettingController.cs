using Bahrin.Harbour.Model.EmailModel;
using Bahrin.Harbour.Service.EmailService;
using Microsoft.AspNetCore.Mvc;

namespace Bahrin_Harbour.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("[area]/[controller]/[action]")]
    public class SettingController : Controller
    {
        private readonly IEmailService _emailService;
        public SettingController(IEmailService emailService)
        {
           _emailService = emailService;    
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> GetMailSetting()
        {
          var Setting = await  _emailService.GetMailSetting();

            return Ok(Setting);
        }

        [HttpPost]
        public async Task<IActionResult> SaveMailSetting(SMTPConfigModel sMTPConfigModel)
        {

            if (ModelState.IsValid)
            {
                var Success = await _emailService.SaveOrUpdateMailSetting(sMTPConfigModel);
                
                return Ok(Success);
            }
            return Ok(false);
        }
        
        
        public async Task<IActionResult> SendtestMail()
        {
            var Success = await _emailService.SendTestMail();
            
            return Ok(Success);
            
        }
    }
}
