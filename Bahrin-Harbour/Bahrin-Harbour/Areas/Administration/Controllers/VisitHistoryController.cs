using Bahrin.Harbour.Data.VisitHistoryService;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Service.ClientService;
using Bahrin.Harbour.Service.LoyalityCard;
using Microsoft.AspNetCore.Mvc;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Bahrin_Harbour.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("[area]/[controller]/[action]")]
    //[AdminAuthorize]
    public class VisitHistoryController : Controller
    {
        private readonly IClientService _clientService;
        private readonly IVisitHistoryService _visitHistory;
        private readonly ILoyalityCardService _loyalityCard;

        public VisitHistoryController(IClientService clientService, ILoyalityCardService loyalityCard, IVisitHistoryService visitHistory)
        {
            _clientService = clientService;
            _loyalityCard = loyalityCard;
            _visitHistory = visitHistory;
        }
        public IActionResult Index()
        {
            return View();
        }

        // public async Task<IActionResult> GetAll()
        //{
        //   var allCheckins = await _visitHistory.GetAllCheckins();
        //    return Ok(allCheckins);
        //}
    // code by rashi 

    public async Task<IActionResult> GetAll()
    {
      var allCheckins = await _visitHistory.GetAllCheckins();

      var data = from c in allCheckins
                 select new[]
             {
                       c.OutletName+"",                      
                       c.OutletLocation+"",
                       c.RepresentativeName+"",
                       c.ClientName+"",
                       c.VisitedDate.HasValue ? c.VisitedDate.Value.ToString("HH:mm:ss") : "",
                       c.Comments+"",
                       c.Id+"",
                       c.ClientId+"",
                       c.ClientImageLink+""

                 };

      return Json(new
      {

        iTotalRecords = allCheckins.Count(),
        iTotalDisplayRecords = allCheckins.Count(),
        aaData = data
      });
    }


    public async Task<IActionResult> GetAllDetailsOfVisitedClientById(string ClientId, string VisitId)
    {
      var allCheckins = await _clientService.GetClientByIdAsynctry(ClientId);
     
      allCheckins.VisitHistory = allCheckins?.VisitHistory?.Where(x => x.Id == Guid.Parse(VisitId)).ToList();

      return Ok(allCheckins);
    }

  }
}
