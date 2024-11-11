using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Data.OutletDA;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.AppUserAuth;
using Bahrin.Harbour.Model.ClientModel;
using Bahrin.Harbour.Model.OutletModel;
using Microsoft.AspNetCore.Identity;

public class OutletService : IOutletService
{
  private readonly IOutletDA _outletDa;
  private readonly IImageService _image;
  private readonly UserManager<ApplicationUser> _user;

  public OutletService(IOutletDA outlet, IImageService image, UserManager<ApplicationUser> user)
  {
    _outletDa = outlet;
    _image = image;
    _user = user;
  }
  public async Task<List<OutletViewModel>> GetAllOutletsAsync()
    {
        var outlets = await _outletDa.GetAllOutletsAsync();
    var users = await _user.GetUsersInRoleAsync(Constants.AppUser);
    var userModel = outlets.OrderByDescending(x=>x.CreatedDate).Select(o => new OutletViewModel        

        {
            Id = o.Id,
            Name = o.Name,
            Country = o.Country,
            State = o.State,
            City = o.City,
            Address = o.Address,
            DiscountPercentage = o.DiscountPercentage,
            ContactPersonName = o.ContactPersonName,
            ContactPersonEmail = o.ContactPersonEmail,
            ContactPersonPhoneNumber = o.ContactPersonPhoneNumber,
            OutletImageLink = o.OutletImageName,
            AciveStatus = o.AciveStatus,
            RepresentativeId = users.FirstOrDefault(x => x.OutletId == o.Id)?.Id,
            RepresentativeName = users.FirstOrDefault(x => x.OutletId == o.Id)?.FirstName + " " + users.FirstOrDefault(x =>       x.OutletId == o.Id)?.LastName,
      ProfileImageLink = _image.GenerateImageUrl(users.FirstOrDefault(x => x.OutletId == o.Id)?.ProfileImagePathfolder, users.FirstOrDefault(x => x.OutletId == o.Id)?.ProfileImageFileName
      )
    }).ToList();
    return userModel;
    }

    public async Task<OutletViewModel> CreateOrUpdateOutletAsync(OutletViewModel model)
    {
        if (model == null)
            throw new KeyNotFoundException("Outlet View Model is null");

        Outlet outlet = model.Id == Guid.Empty ? new Outlet() : await _outletDa.GetOutletByIdAsync(model.Id);

        if (outlet == null)
            throw new KeyNotFoundException("Outlet not found");

        outlet.Name = model.Name;
        outlet.Country = model.Country;
        outlet.State = model.State;
        outlet.City = model.City;
        outlet.Address = model.Address;
        outlet.DiscountPercentage = model.DiscountPercentage;
        outlet.ContactPersonName = model.ContactPersonName;
        outlet.ContactPersonEmail = model.ContactPersonEmail;
        outlet.ContactPersonPhoneNumber = model.ContactPersonPhoneNumber;
        outlet.AciveStatus = model.AciveStatus;

        if (model.ImageFile != null)
        {
      outlet.OutletImageName = await _image.UpdateImageAsync(model.ImageFile, Constants.OutletProfileImages, outlet.OutletImageName);
            outlet.OutletImageFolderName = Constants.OutletProfileImages;
        }

        if (model.Id == Guid.Empty)
        {
            outlet.CreatedDate = DateTime.Now;
            await _outletDa.AddOutletAsync(outlet);
        }
        else
        {
            outlet.DateModified = DateTime.Now;
            await _outletDa.UpdateOutletAsync(outlet);
        }

        model.Id = outlet.Id;
        model.OutletImageLink = outlet.OutletImageName;

        return model;
    }

    public async Task DeleteOutletByIdAsync(Guid id)
    {
        await _outletDa.DeleteOutletAsync(id);
    }

    public async Task<OutletViewModel> GetOutletByIdAsync(Guid id)
    {
        var outlet = await _outletDa.GetOutletByIdAsync(id);

        if (outlet == null)
            throw new KeyNotFoundException("Outlet not found");

        var outletViewModel = new OutletViewModel
        {
            Id = outlet.Id,
            Name = outlet.Name,
            Country = outlet.Country,
            State = outlet.State,
            City = outlet.City,
            Address = outlet.Address,
            DiscountPercentage = outlet.DiscountPercentage,
            ContactPersonName = outlet.ContactPersonName,
            ContactPersonEmail = outlet.ContactPersonEmail,
            ContactPersonPhoneNumber = outlet.ContactPersonPhoneNumber,
            OutletImageLink = _image.GenerateImageUrl(outlet.OutletImageFolderName, outlet.OutletImageName),
          
            AciveStatus = outlet.AciveStatus
        };

        return outletViewModel;
    }
  public async Task<bool> OutletStatusAsync(string id, bool activate)
  {
    var outlet = await GetOutletByIdAsync(Guid.Parse(id));

    return await _outletDa.OutletStatusAsync(Guid.Parse(id), activate);
  }
}
