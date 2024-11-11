using Bahrin.Harbour.Data.ClientDA;
using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Data.LoyalityCardDA;
using Bahrin.Harbour.Data.OutletDA;
using Bahrin.Harbour.Data.VisitHistoryDA;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.ClientModel;
using Bahrin.Harbour.Model.ProjectSession;
using Bahrin.Harbour.Model.VisitHistoryModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Bahrin.Harbour.Service.ClientService
{
  public class ClientService : IClientService
  {
    private readonly IClientDA _clientDA;
    private readonly ILoyalityCardDA _loyalityCard;
    private readonly IVisitHistoryDA _visit;
    private readonly IImageService _image;
    private readonly string imageFolderName = Constants.ClientProfileImages;
    private readonly IOutletDA _outletDa;
    private readonly UserManager<ApplicationUser> _userManager;

    public ClientService(IClientDA clientDA, IVisitHistoryDA visit, IImageService image, IOutletDA outlet, UserManager<ApplicationUser> userManager, ILoyalityCardDA loyalityCard)
    {
      _clientDA = clientDA;
      _visit = visit;
      _outletDa = outlet;
      _image = image;
      _userManager = userManager;
      _loyalityCard = loyalityCard;
    }
    public async Task<List<ClientViewModel>> GetAllClientsAsync()
    {
      List<Client> clients = await _clientDA.GetAllClientsAsync();

      List<ClientViewModel> ListClientViewModel = clients.Select(client => new ClientViewModel
      {
        Id = client.Id,
        ClientName = client.ClientName,
        ClientId = client.ClientId,
        Name = client.Name,
        EmailAddress = client.EmailAddress,
        Phone = client.Phone,
        Country = client.Country,
        City = client.City,
        State = client.State,
        Postcode = client.Postcode,
        Address = client.Address,
        Status = client.AciveStatus,
        ClientProfileImageLink = _image.GenerateImageUrl(imageFolderName, client.ClientProfileImageFileName),
      }).ToList();

      return ListClientViewModel;
    }

    public async Task<bool> ClientDetails(ClientViewModel clientViewModel, IFormFile? ImageFile = null)
    {
      Client client = new Client
      {
        Id = clientViewModel.Id != Guid.Empty ? clientViewModel.Id : Guid.NewGuid(),
        ClientName = clientViewModel.ClientName,
        ClientId = clientViewModel.ClientId,
        Name = clientViewModel.Name,
        EmailAddress = clientViewModel.EmailAddress,
        Phone = clientViewModel.Phone,
        Country = clientViewModel.Country,
        City = clientViewModel.City,
        State = clientViewModel.State,
        Postcode = clientViewModel.Postcode,
        Address = clientViewModel.Address,
        AciveStatus = clientViewModel?.Status == null ? true : clientViewModel.Status,
        CreatedDate = DateTime.Now,
        ModifiedBy = ProjectSessionModel.admin._id,
        //AciveStatus = clientViewModel.Id != Guid.Empty ? clientViewModel.Status : true,
      };

      if (ImageFile != null)
      {
        client.ClientProfileImageFileName = await _image.SaveImageAsync(ImageFile, imageFolderName);
        client.ImageFolderName = imageFolderName;
      }

      if (clientViewModel.Id == Guid.Empty)
      {

        await _clientDA.AddClientAsync(client);

        client.Properties = new List<Data.DBCollections.Property>();

        if (clientViewModel.Properties != null)
        {
          foreach (var item in clientViewModel.Properties)
          {
            item.ClientUserId = client.Id;
            await AddOrUpdatePropertyAsync(item, item.ImgFile);
          }
        }
      }
      else
      {
        await _clientDA.UpdateClientAsync(client);

        if (clientViewModel.Properties != null)
        {
          foreach (var item in clientViewModel.Properties)
          {
            item.ClientUserId = clientViewModel.Id;
            await AddOrUpdatePropertyAsync(item, item.ImgFile);
          }
        }
      }
      return true;
    }

    public async Task<bool> AddOrUpdatePropertyAsync(PropertyViewModel property, IFormFile? imgFile = null)
    {
      var prop = new Data.DBCollections.Property
      {
        Id = property.Id,
        AciveStatus = true,
        Address = property.Address,
        //City = property.City,
        ClientUserId = property.ClientUserId,
        //     ClientId = property.ClientUserId,
        Country = property.Country,
        Createdby = ProjectSessionModel.admin._id,
        CreatedDate = DateTime.Now,
        State = property.State,
        TypeOfProperty = property.TypeOfProperty,
        PropertyPrice = Convert.ToInt64(property.PropertyPrice),

      };

      if (imgFile != null)
      {
        prop.UploadedImageName = await _image.SaveImageAsync(imgFile, Constants.PropertyImages);
        prop.UploadedImageFolderName = Constants.PropertyImages;
      }
      await _clientDA.AddOrUpdatePropertyAsync(prop);
      return true;
    }
    public async Task DeletePropertyAsync(string id)
    {
      await _clientDA.DeletePropertyAsync(id);
    }

    public async Task<bool> DeleteClientAsync(string id)
    {
      var client = await GetClientByIdAsynctry(id)
;
      //if (client != null)
      //{
      //    _image.DeleteImage(imageFolderName, client.ClientProfileImageLink);
      //    _image.DeleteImage(Constants.QrCodeImages, client.ClientQrCodeImageLink);
      //}
      if (client.Properties != null)
      {
        foreach (var item in client.Properties)
        {
          await DeletePropertyAsync(item.Id.ToString());
        }
      }
      return await _clientDA.DeleteClientAsync(Guid.Parse(id));
    }

    public async Task<ClientViewModel> GetClientByIdAsync(string id, string UserId = null, bool isVisited = false)
    {
      Client client = await _clientDA.GetClientByIdAsync(id);
      LoyaltyCard LoyalityCard = await _loyalityCard.GetCardsByIdAsync(id);

      if (client == null || LoyalityCard == null )
      {
        return null;
      }
       if (!client.AciveStatus || !LoyalityCard.AciveStatus)
      {
        return null;
      }


      var properties = await _clientDA.GetPropertyByIdAsync(client.Id);

      ClientViewModel clientViewModel = new ClientViewModel
      {
        Id = client.Id,
        ClientName = client.ClientName,
        ClientId = client.ClientId,
        Name = client.Name,
        EmailAddress = client.EmailAddress,
        Phone = client.Phone,
        Country = client.Country,
        City = client.City,
        State = client.State,
        Postcode = client.Postcode,
        Address = client.Address,
        Status = client.AciveStatus,
        //ClientProfileImageLink = /*"https://picsum.photos/200/300"//*/_image.GenerateImageUrl(imageFolderName, client.ClientProfileImageFileName),
        ClientProfileImageLink = _image.GenerateImageUrl(imageFolderName, client.ClientProfileImageFileName),

      };

      if (properties != null)
      {
        var ClientProperties = new List<PropertyViewModel>();

        foreach (var item in properties)
        {
          var property = new PropertyViewModel
          {
            ClientUserId = client.Id,
            TypeOfProperty = item.TypeOfProperty,
            Address = item.Address,
            State = item.State,
            City = item.City,
            Country = item.Country,
            //ClientUserName = "js",
            ClientUserName = $"{item.Address}, {item.City}, {item.State}, {item.Country}",
            ImageLink = "https://picsum.photos/200/300"

          };
          if (!string.IsNullOrEmpty(item.UploadedImageFolderName) && !string.IsNullOrEmpty(item.UploadedImageName))
          {
            property.ImageLink = _image.GenerateImageUrl(item.UploadedImageFolderName, item.UploadedImageName);
          }
          ClientProperties.Add(property);
        }
        clientViewModel.Properties = ClientProperties;
      }

      var Representative = await _userManager.FindByIdAsync(UserId);

      var outlet = await _outletDa.GetOutletByIdAsync((Guid)Representative.OutletId);

      if (isVisited)
      {
        var visit = new VisitHistory()
        {
          Id = Guid.NewGuid(),
          RepresentativeId = Guid.Parse(UserId),
          ClientId = client.Id,
          VisitedDate = DateTime.Now,
          AciveStatus = true,
          Checkin = Constants.False,
          Visited = Constants.True,
          OutletId = outlet.Id,
          Discount = Convert.ToInt32(outlet.DiscountPercentage)
        };

        clientViewModel.CurrentVisitId = visit.Id;

        await _clientDA.AddVisitHistoryAsync(visit);
      }

      var history = await _clientDA.GetVisitHistoryByOutletIdAsync(outlet.Id);
      List<VisitHistoryView> visitHistoryView = new List<VisitHistoryView>();

      foreach (var item in history.Where(x => x.Checkin == true))
      {
        var visits = new VisitHistoryView
        {
          Id = item.ClientId,
          OutletId = outlet.Id,
          OutletName = outlet.Name,
          RepresentativeId = Guid.Parse(UserId),
          VisitedDate = item.VisitedDate,
          Comments = item.Comments,
          CheckinDate = item.CheckinDate,
        };

        visitHistoryView.Add(visits);
      }

      clientViewModel.VisitHistory = visitHistoryView.OrderByDescending(x => x.VisitedDate).ToList();

      return clientViewModel;
    }
    public async Task<ClientViewModel> GetClientByIdAsynctry(string id)
    {
      Client client = await _clientDA.GetClientByIdAsync(id);

      if (client == null)
      {
        return null;
      }

      ClientViewModel clientViewModel = new ClientViewModel
      {
        Id = client.Id,
        ClientName = client.ClientName,
        ClientId = client.ClientId,
        Name = client.Name,
        EmailAddress = client.EmailAddress,
        Phone = client.Phone,
        Country = client.Country,
        City = client.City,
        State = client.State,
        Postcode = client.Postcode,
        Address = client.Address,
        ClientProfileImageLink = _image.GenerateImageUrl(client.ImageFolderName, client.ClientProfileImageFileName)
      };

      var properties = await _clientDA.GetPropertyByIdAsync(client.Id);

      if (properties != null)
      {
        var ClientProperties = new List<PropertyViewModel>();

        foreach (var item in properties)
        {
          var property = new PropertyViewModel
          {
            Id = item.Id,
            ClientUserId = client.Id,
            TypeOfProperty = item.TypeOfProperty,
            Address = item.Address,
            State = item.State,
            City = item.City,
            Country = item.Country,
            ClientUserName = $"{item.Address}, {item.City}, {item.State}, {item.Country}",
            ImageLink = _image.GenerateImageUrl(item.UploadedImageFolderName, item.UploadedImageName)

          };
          if (!string.IsNullOrEmpty(item.UploadedImageFolderName) && !string.IsNullOrEmpty(item.UploadedImageName))
          {
            property.ImageLink = _image.GenerateImageUrl(item.UploadedImageFolderName, item.UploadedImageName);
          }
          ClientProperties.Add(property);
        }
        clientViewModel.Properties = ClientProperties;
      }


      var history = await _clientDA.GetVisitHistoryByClientIdAsync(client.Id);
      var Outlets = await _outletDa.GetAllOutletsAsync();


      List<VisitHistoryView> visitHistoryView = new List<VisitHistoryView>();

      foreach (var item in history)
      {
        var visits = new VisitHistoryView
        {
          Id = item.Id,
          ClientId = item.ClientId,
          OutletId = Outlets.FirstOrDefault(x => x.Id == item.OutletId)?.Id,
          OutletName = Outlets.FirstOrDefault(x => x.Id == item.OutletId)?.Name,
          RepresentativeId = item.RepresentativeId,
          VisitedDate = item.CheckinDate,
          Comments = item.Comments,
          OutletLocation = Outlets.FirstOrDefault(x => x.Id == item.OutletId)?.Address,
          Discount = item.Discount.ToString()
        };

        visitHistoryView.Add(visits);
      }

      clientViewModel.VisitHistory = visitHistoryView.Where(x => x.VisitedDate != null).OrderByDescending(x => x.VisitedDate).ToList();
      return clientViewModel;
    }

    public async Task<StatusModel> CheckInComments(CommentsViewModel model)
    {
      StatusModel StatusModel = new StatusModel();

      StatusModel.status = await _clientDA.UpdateVisitHistoryAsync(model);
      StatusModel.message = "Discount Successfully Applied";
      return StatusModel;
    }

    public async Task<List<VisitedViewModel>> GetAllVIPClients(string id)
    {
      List<VisitedViewModel> VipClients = new List<VisitedViewModel>();

      var Representative = await _userManager.FindByIdAsync(id);

      var outlet = await _outletDa.GetOutletByIdAsync((Guid)Representative.OutletId);

      var allVisitedPersions = await _clientDA?.GetVisitHistoryByOutletIdAsync(outlet?.Id);

      var groupVisited = allVisitedPersions?.Where(x => x.Visited == true).GroupBy(x => x.ClientId).ToList();

      foreach (var item in groupVisited)
            {
                var Client = await _clientDA?.GetClientByIdAsync(item.Key.ToString());
                if (Client != null)
                {

               
                var model = new VisitedViewModel
        {
          ClientId = item.Key.ToString(),
          Id = Helper.Helper.FormatClientId(Client.ClientId),
          imageUrl = _image.GenerateImageUrl(Client.ImageFolderName, Client.ClientProfileImageFileName),
          Name = Client.ClientName,
          VisitDate =  _clientDA.LastVisitOfAClient(item.Key)
        };
        VipClients.Add(model);
                }
      }
      return VipClients.OrderByDescending(x => x.VisitDate).ToList();
    }

    public async Task<List<VisitedViewModel>> GetAllVisitsOrRecentCheckins(string id, DateTime? date = null, bool onlyCheckIns = false, bool onlyCount = false, int? numberOfItems = null)
    {
      List<VisitedViewModel> VipClients = new List<VisitedViewModel>();

      var Representative = await _userManager.FindByIdAsync(id);

      var outlet = await _outletDa.GetOutletByIdAsync((Guid)Representative.OutletId);

      var allVisitedPersons = await _clientDA.GetVisitHistoryByOutletIdAsync(outlet.Id);

      if (onlyCheckIns)
      {
        allVisitedPersons = allVisitedPersons.Where(x => x.Checkin == true).ToList();
      }

      if (onlyCount)
      {
        int count = allVisitedPersons.Count;
        return new List<VisitedViewModel>
        {
            new VisitedViewModel
            {
                TotalCheckins = onlyCheckIns ? count : 0,
                TotalVisited = !onlyCheckIns ? count : 0
            }
        };
      }

      if (date.HasValue)
      {
        var filterDate = date.Value.Date;
        allVisitedPersons = onlyCheckIns
            ? allVisitedPersons.Where(x => x.CheckinDate.HasValue && x.CheckinDate.Value.Date == filterDate).ToList()
            : allVisitedPersons.Where(x => x.VisitedDate.Date == filterDate).ToList();
      }

      if (numberOfItems != null)
      {
        allVisitedPersons = allVisitedPersons.Take((int)numberOfItems).ToList();
      }

      foreach (var item in allVisitedPersons)
      {
        Client? client = await _clientDA?.GetClientByIdAsync(item.ClientId.ToString());
        var model = new VisitedViewModel
        {
          ClientId = item.ClientId.ToString(),
          Id = client.ClientId.ToString("D6"),
          imageUrl = _image.GenerateImageUrl(client.ImageFolderName, client.ClientProfileImageFileName),
          Name = client?.ClientName,
          VisitDate = item.VisitedDate,
          CheckInDate = item.CheckinDate
        };
        VipClients.Add(model);
      }

      return VipClients.OrderByDescending(x => x.VisitDate).ToList();
    }

    public async Task<List<VisitedViewModel>> GetAllCheckInsOfAClient(string representativeId, string id, bool onlyCheckIns = false, bool onlyCount = false)
    {
      List<VisitedViewModel> VipClients = new List<VisitedViewModel>();

      var outlet = await _userManager.FindByIdAsync(representativeId);
      var allVisitedPersons = await _clientDA.GetVisitHistoryByClientIdAsync(Guid.Parse(id));
      allVisitedPersons = allVisitedPersons.Where(x=>x.OutletId == outlet.OutletId).OrderBy(x => x.VisitedDate).ToList();

      if (onlyCheckIns)
      {
        allVisitedPersons = allVisitedPersons.Where(x => x.Checkin == true).ToList();
      }

      if (onlyCount)
      {
        var client = await _clientDA.GetClientByIdAsync(id);

        int count = allVisitedPersons.Count;
        return new List<VisitedViewModel>
            {
            new VisitedViewModel
            {
                TotalCheckins = onlyCheckIns ? count : 0,
                TotalVisited = !onlyCheckIns ? count : 0,
                Id = client.Id.ToString(),
                Name = client.Name,
                imageUrl = _image.GenerateImageUrl(client.ImageFolderName, client.ClientProfileImageFileName)
            }
             };
      }

      foreach (var item in allVisitedPersons)
      {
        var client = await _clientDA.GetClientByIdAsync(item.ClientId.ToString());
        var Outlets = await _outletDa?.GetAllOutletsAsync();

                if (client != null)
                {
                    var model = new VisitedViewModel
                    {
                        ClientId = item.ClientId.ToString(),
                        Id = client.ClientId.ToString("D6"),
                        imageUrl = _image.GenerateImageUrl(client.ImageFolderName, client.ClientProfileImageFileName),
                        Name = client.ClientName,
                        VisitDate = item.VisitedDate,
                        CheckInDate = item.CheckinDate,
                        Comments = item.Comments,
                        OutletName = Outlets?.FirstOrDefault(k => k.Id == item.OutletId)?.Name,
                        Discount = item.Discount.ToString()
                    };
                    VipClients.Add(model);
                }
          }

      return VipClients.OrderByDescending(x => x.VisitDate).ToList();
    }

    public async Task<VisitAnalytics> Analytics(string id, int i = 5)
    {
      var Representative = await _userManager.FindByIdAsync(id);

      var outlet = await _outletDa.GetOutletByIdAsync((Guid)Representative.OutletId);

      var visit = await _clientDA.GetVisitHistoryByOutletIdAsync(outlet.Id);

      var startDate = DateTime.Today.AddDays(-i);
      var endDate = DateTime.Today;

      var allDates = Enumerable.Range(0, (endDate - startDate).Days + 1)
                                .Select(offset => startDate.AddDays(offset))
                                .ToList();

      var groupedVisits = visit
          .Where(v => v.Visited && v.VisitedDate.Date >= startDate && v.VisitedDate.Date <= endDate)
          .GroupBy(v => v.VisitedDate.Date)
          .Select(g => new Visit
          {
            Date = g.Key,
            CheckinCount = g.Count(x => x.Checkin == true),
            VisitCount = g.Count()
          })
          .ToList();

      var result = allDates
          .GroupJoin(
              groupedVisits,
              date => date,
              visit => visit.Date,
              (date, visits) => new Visit
              {
                Date = date,
                CheckinCount = visits.FirstOrDefault()?.CheckinCount ?? 0,
                VisitCount = visits.FirstOrDefault()?.VisitCount ?? 0
              })
          .ToList();

      var totalVisits = result.Sum(v => v.VisitCount);
      var totalCheckins = result.Sum(v => v.CheckinCount);

      double percentageDiscount = ((double)totalCheckins / totalVisits) * 100;

      return new VisitAnalytics
      {
        Visits = result,
        TotalVisit = totalVisits,
        PercentageDiscount = Math.Round(percentageDiscount, 1)
      };
    }

    // code by rashi
    //public async Task<List<ClientViewModel>> GetAllClients()
    //{
    //    List<Client> clients = await _clientDA.GetAllClients();

    //    List<ClientViewModel> ListClientViewModel = clients.Select(client => new ClientViewModel
    //    {
    //        Id = client.Id,
    //        ClientId = client.ClientId,
    //        ClientName = client.ClientName,
    //        Status = client.AciveStatus,
    //        ClientProfileImageLink = client.ClientProfileImageFileName

    //    }).ToList();

    //    return ListClientViewModel;
    //}
    public async Task<List<ClientViewModel>> GetAllClients()
    {
      List<Client> clients = await _clientDA.GetAllClients();

      List<ClientViewModel> ListClientViewModel = clients.Select(client => new ClientViewModel
      {
        Id = client.Id,
        ClientId = client.ClientId,
        ClientName = client.ClientName,
        PhoneNumber = client.Phone,
        LastVisit = _clientDA.LastVisitOfAClient(client.Id),
        PropertyCount = _clientDA.GetPropertyCountByIdAsync(client.Id),
        Status = client.AciveStatus,
        ClientProfileImageLink = _image.GenerateImageUrl(client.ImageFolderName, client.ClientProfileImageFileName)

      }) // Sort by ClientId
    .ToList();

      return ListClientViewModel.OrderByDescending(clientViewModel => clientViewModel.ClientId).ToList();
    }
    public async Task<bool> ClientStatusAsync(string id, bool activate)
    {
      var client = await GetClientByIdAsynctry(id);

      return await _clientDA.ClientStatusAsync(Guid.Parse(id), activate);
    }
    public async Task<ClientViewModel> GetClientsById(string id)
    {
      Client client = await _clientDA.GetClientByIdAsync(id)
;

      if (client == null)
      {
        return null;
      }

      ClientViewModel clientViewModel = new ClientViewModel
      {
        Id = client.Id,
        ClientId = client.ClientId,
        ClientName = client.ClientName,
        Status = client.AciveStatus,
        ClientProfileImageLink = client.ClientProfileImageFileName,
        EmailAddress = client.EmailAddress,
        Country = client.Country,
        City = client.City,
        State = client.State,
        Postcode = client.Postcode,
        Address = client.Address
      };

      return clientViewModel;
    }

    public async Task<List<PropertyViewModel>> GetPropertiesByClientid(Guid id)
    {
      List<Property> properties = await _clientDA.GetPropertyByIdAsync(id);

      List<PropertyViewModel> ListPropertyViewModel = properties.Select(property => new PropertyViewModel
      {
        Id = property.Id,

        TypeOfProperty = property.TypeOfProperty,
        Country = property.Country,
        State = property.State,
        Address = property.Address,
        PropertyPrice = property.PropertyPrice.ToString(),
        ImageLink = _image.GenerateImageUrl(property.UploadedImageFolderName, property.UploadedImageName)


      })
    .ToList();

      return ListPropertyViewModel;
    }
    public async Task<PropertyViewModel> GetPropertyByPropertyIdAsync(Guid id)
    {

      Property property = await _clientDA.GetPropertyByPropertyIdAsync(id);
      var model = new PropertyViewModel
      {
        Id = property.Id,
        TypeOfProperty = property.TypeOfProperty,
        Country = property.Country,
        State = property.State,
        Address = property.Address,
        PropertyPrice = property.PropertyPrice.ToString(),
        ImageLink = _image.GenerateImageUrl(property.UploadedImageFolderName, property.UploadedImageName),
        ClientUserId= (Guid)property.ClientUserId,       

      };
      return model;
    }
  }
  public class Visit
    {
        public DateTime Date { get; set; }
        public int CheckinCount { get; set; }
        public int VisitCount { get; set; }
    }
    public class VisitAnalytics
    {
        public List<Visit> Visits { get; set; }
        public int TotalVisit { get; set; }
        public double PercentageDiscount { get; set; }
    }

}