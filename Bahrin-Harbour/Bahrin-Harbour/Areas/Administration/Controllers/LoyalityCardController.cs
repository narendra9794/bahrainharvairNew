using Bahrin.Harbour.Data.ClientDA;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Service.LoyalityCard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bahrin_Harbour.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("[area]/[controller]/[action]")]
    public class LoyalityCardController : Controller
    {
        private ILoyalityCardService _loyalityCardService;
        private IClientDA _clientDA;
        public LoyalityCardController(ILoyalityCardService loyalityCardService, IClientDA clientDA)
        {
            _loyalityCardService = loyalityCardService;
            _clientDA = clientDA;
        }
        public async Task<ActionResult> Index()
        {
            var clients = await _clientDA.GetAllClients();

            var AcitveClients = clients.Where(x => x.AciveStatus && !x.isLoyalityCardGenerated).Select(o => new SelectListItem
            {
                Value = o.Id.ToString(),
                Text = o.Name
            }).ToList();

            ViewBag.Clients = new SelectList(AcitveClients, "Value", "Text");

            return View();
        }

        //public async Task<IActionResult> GetAll()
        //{
        //  var allCards = await _loyalityCardService.GetAllCard();
        //  return Ok(allCards);
        //}
        // rashi try code 
        public async Task<IActionResult> GetAll()
        {
            var allCards = await _loyalityCardService.GetAllCard();
            var data = from c in allCards
                       select new[]
                       {
                       c.ClientGuid+"",
                       c.ClientName+"",
                       c.Email+"",
                       c.ContactNumber+"",
                       c.Active+"" ,
                       c.ClientId+""
                 };
            return Json(new
            {

                iTotalRecords = allCards.Count(),
                iTotalDisplayRecords = allCards.Count(),
                aaData = data
            });
        }
        public async Task<IActionResult> CreateOrUpdate(string ClientGuidId, bool? ActiveStatus = null)
        {
            var Active = ActiveStatus == null ? true : ActiveStatus;

            var status = await _loyalityCardService.CreateOrUpdateAsync(ClientGuidId, (bool)Active);

            if (status.status)
            {
                TempData["Success"] = status.message;
                return Ok();
            }
            TempData["Error"] = status.message;
            return new BadRequestResult();
        }
        public async Task<IActionResult> Download(string ClientGuidId)
        {
            var data = await _loyalityCardService.CreatePdfWithQrCode(ClientGuidId);

            return File(data.PdfData, "application/pdf", $"{data.QrCodeString + DateTime.Now.TimeOfDay}.pdf");
        }
        //public async Task<IActionResult> View(string ClientGuidId)
        //{
        //  var card = await _loyalityCardService.GetCardAsync(ClientGuidId);
        //  return View(card);
        //}
        
    }

}
