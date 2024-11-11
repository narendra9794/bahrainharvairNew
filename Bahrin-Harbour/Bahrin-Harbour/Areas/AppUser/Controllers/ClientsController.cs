using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Model.AppUserAuth;
using Bahrin.Harbour.Model.ClientModel;
using Bahrin.Harbour.Service.ClientService;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ZXing.QrCode.Internal;

namespace Bahrin_Harbour.Areas.AppUser.Controllers
{
    [Area("user")]
    [Route("[area]/[controller]/[action]")]
    [ApiController]
    [UserAuthorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
           
        }

        [HttpGet]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetClientByQRCode(string qrCodeid)
        {
            var RepresentativeId = HttpContext.Items["RepresentativeId"] as string;
            var client = await _clientService.GetClientByIdAsync(qrCodeid, RepresentativeId, true);

            if (client == null)
            {
                return NotFound("No Detail Found On this QR");
            }
            return Ok(client);
        }
        public async Task<ActionResult> GetClientById(string ClientId)
        {
            var RepresentativeId = HttpContext.Items["RepresentativeId"] as string;

            var client = await _clientService.GetClientByIdAsync(ClientId, RepresentativeId, false);
            if (client == null)
            {
                return NotFound("No Detail Found On this QR");
            }
            return Ok(client);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetRecentCheckIns( DateTime? checkInOrVisitDate = null, bool onlyCount = false, int? NumberOfItems = null)
        {
            var RepresentativeId = HttpContext.Items["RepresentativeId"] as string;

            var recentCheckIns = await _clientService.GetAllVisitsOrRecentCheckins(RepresentativeId, checkInOrVisitDate, true, onlyCount, NumberOfItems);

            return Ok(recentCheckIns);
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllCheckInsOfAClient(string ClientGuid, bool CheckinsOnly = false, bool onlyCount = false)
        {
            var RepresentativeId = HttpContext.Items["RepresentativeId"] as string;

            var recentCheckIns = await _clientService.GetAllCheckInsOfAClient(RepresentativeId, ClientGuid, CheckinsOnly, onlyCount);

            return Ok(recentCheckIns);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult> VisitHistory(DateTime? checkInOrVisitDate = null, bool onlyCount = false, int? NumberOfItems = null)
        {
            var RepresentativeId = HttpContext.Items["RepresentativeId"] as string;

            var recentCheckIns = await _clientService.GetAllVisitsOrRecentCheckins(RepresentativeId,  checkInOrVisitDate, false, onlyCount, NumberOfItems);

            return Ok(recentCheckIns);
        }


        [HttpGet]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> VIPClients()
        {
            var RepresentativeId = HttpContext.Items["RepresentativeId"] as string;

            var allVipClients =await _clientService.GetAllVIPClients(RepresentativeId);

            return Ok(allVipClients);      
        }

        [HttpPost]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckInComments(CommentsViewModel model)
        {
           var RepresentativeId = HttpContext.Items["RepresentativeId"] as string;
 
        var result =    await _clientService.CheckInComments(model);

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(SigninResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Analytics(int lastNumberOfDays = 7)
        {
           var RepresentativeId = HttpContext.Items["RepresentativeId"] as string;
 
           var result = await _clientService.Analytics(RepresentativeId, lastNumberOfDays);

            return Ok(result);
        }

    }
}
