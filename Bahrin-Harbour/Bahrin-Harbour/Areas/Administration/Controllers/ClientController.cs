using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.ClientModel;
using Bahrin.Harbour.Service.ClientService;
using Bahrin.Harbour.Service.LoyalityCard;
using Microsoft.AspNetCore.Mvc;


namespace Bahrin_Harbour.Areas.Administration.Controllers
{
  [Area("Administration")]
  [Route("[area]/[controller]/[action]")]
  //[AdminAuthorize]
  public class ClientController : Controller
  {
    private readonly IClientService _clientService;
    private readonly ILoyalityCardService _loyalityCard;

    public ClientController(IClientService clientService, ILoyalityCardService loyalityCard)
    {
      _clientService = clientService;
      _loyalityCard = loyalityCard;
    }

    public async Task<IActionResult> Clients()

    {
      return View();
    }

    public async Task<IActionResult> GetAllClientsAsync()
    {
      var clients = await _clientService.GetAllClientsAsync();

      return Ok(clients);
    }
    public async Task<IActionResult> GetClientById(string Id)
    {
      var clientViewModel = await _clientService.GetClientByIdAsynctry(Id);
      if (clientViewModel == null)
      {
        return NotFound();
      }
      return Ok(clientViewModel);
    }

    public async Task<IActionResult> AddorUpdateClientDetails(string ClientId)
    {
      ClientViewModel model = new ClientViewModel();

      if (Guid.Parse(ClientId) != Guid.Empty)
      {
        model = await _clientService.GetClientByIdAsynctry(ClientId);
      }
      return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> AddorUpdateClientDetails(ClientViewModel clientView, IFormFile ImageFile)
    {
      //if (ModelState.IsValid)
      //{
      var success = await _clientService.ClientDetails(clientView, ImageFile);
      if (success)
      {
        return RedirectToAction("Clients");
      }
      ModelState.AddModelError("", "Failed to create client.");
      //}
      return View(clientView);
    }

    [HttpPost]
    public async Task<IActionResult> DeletePorpertyByIdAsync(string id, Guid ClientId)
    {

      await _clientService.DeletePropertyAsync(id);
      var json = await GetAllPropertyDetails(ClientId);
      return Ok(json);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteClient(string Id)
    {

      StatusModel status = new StatusModel();

      var success = await _clientService.DeleteClientAsync(Id);
      if (success)
      {

        status.status = Constants.True;
        status.message = "Client deleted successfully.";
        return Ok(status);
      }
      else
      {
        status.status = Constants.False;
        status.message = "Client deleted successfully.";
        return Ok(status);
      }
      
    }
    // code by rashi 
    public async Task<IActionResult> GetAllClientDetails()
    {
      var clients = await _clientService.GetAllClients();

      var data = from c in clients
                 select new[]
             {
                       c.Id.ToString(),
                       Helper.FormatClientId(c.ClientId) + "",
                       c.ClientName+"",
                       c.PhoneNumber+"",
                       c.LastVisit+"",
                       c.PropertyCount+"",
                       c.Status+"",
                       c.ClientProfileImageLink+""
                   };

      return Json(new
      {

        iTotalRecords = clients.Count(),
        iTotalDisplayRecords = clients.Count(),
        aaData = data
      });
    }

    public async Task<IActionResult> Details(string Id)
    {
      var clientViewModel = await _clientService.GetClientByIdAsynctry(Id);
      //var model = new ClientViewModel
      //{
      //  ClientName = clientViewModel.ClientName,
      //  ClientId = clientViewModel.ClientId,
      //  Name = clientViewModel.Name,
      //  EmailAddress = clientViewModel.EmailAddress,
      //  Phone = clientViewModel.Phone,
      //  PhoneNumber = clientViewModel.PhoneNumber,
      //  Country = clientViewModel.Country,
      //  City = clientViewModel.City,
      //  State = clientViewModel.State,
      //  Postcode = clientViewModel.Postcode,
      //  Address = clientViewModel.Address,
      //  Properties = clientViewModel.Properties,
      //  TypeOfProperty = clientViewModel.TypeOfProperty,
      //  PropertyLocation = clientViewModel.PropertyLocation,
      //  PropertyPrice = clientViewModel.PropertyPrice,
      //  AvailedDiscount = clientViewModel.AvailedDiscount,
      //  Street = clientViewModel.Street,
      //  LastVisit = clientViewModel.LastVisit,
      //  Status = clientViewModel.Status,
      //  Mode = "Details"
      //};
      return View("AddorUpdateClientDetails", clientViewModel);

    }



    [HttpPost]
    public async Task<IActionResult> ChangeClientStatus(string id, bool activate)
    {
      try
      {
        var success = await _clientService.ClientStatusAsync(id, activate);

        if (success)
        {
          TempData["SuccessMessage"] = activate ? "Client activated successfully." : "Client deactivated successfully.";
          return Json(new { status = true, message = TempData["SuccessMessage"] });
        }
        else
        {
          return Json(new { status = false, message = activate ? "Failed to activate client." : "Failed to deactivate client." });
        }
      }
      catch (Exception ex)
      {
        return Json(new { status = false, message = activate ? "An error occurred while activating the client." : "An error occurred while deactivating the client." });
      }
    }

    /*  public async Task<IActionResult> CreatePdfWithQrCode(string ClientId)
          {
              var array =await _loyalityCard.CreatePdfWithQrCode(ClientId);

              return File(array, "application/pdf", "BahrinHarbour.pdf");
          }
    */

    public async Task<IActionResult> GetAllPropertyDetails(Guid ClientId)
    {
      var clients = await _clientService.GetPropertiesByClientid(ClientId);

      var data = from c in clients
                 select new[]
                 {
                       c.TypeOfProperty+"",
                       c.Country+"",
                       c.State+"",
                       c.Address+"",
                       c.PropertyPrice+"",
                       c.ImageLink+"",
                       c.ClientUserId+"",
                       c.Id+""

                   };

      return Json(new
      {

        iTotalRecords = clients.Count(),
        iTotalDisplayRecords = clients.Count(),
        aaData = data
      });
    }

    //ViewAllpropertiesOfAClient
    public async Task<IActionResult> AddNewProperty(Guid ClientId)
    {
      List<PropertyViewModel> model = new List<PropertyViewModel>();

      model = await _clientService.GetPropertiesByClientid(ClientId);
      ViewBag.ClientId = ClientId;

      return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> AddNewProperty(PropertyViewModel property, IFormFile? ImageFile)
    {

      var model = await _clientService.AddOrUpdatePropertyAsync(property, ImageFile);

      return View(model);
    }


    public async Task<IActionResult> ViewClient(string ClientId)
    {
      ClientViewModel model = new ClientViewModel();

      if (Guid.Parse(ClientId) != Guid.Empty)
      {
        model = await _clientService.GetClientByIdAsynctry(ClientId);
      }
      return View(model);

    }
    public async Task<IActionResult> Property(Guid ClientId)
    {
      ViewBag.ClientId = ClientId;
      return View();
    }
    public async Task<IActionResult> EditProperty(Guid Id)
    {
      PropertyViewModel model = new PropertyViewModel();

      model = await _clientService.GetPropertyByPropertyIdAsync(Id);

      return View(model);

    }
    public async Task<IActionResult> ViewProperty(Guid Id)
    {
      PropertyViewModel model = new PropertyViewModel();

      model = await _clientService.GetPropertyByPropertyIdAsync(Id);
      return View(model);

    }

  }
}

