using QRCoder;
using Bahrin.Harbour.Helper;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public class ImageService : IImageService
{
    private readonly string _path = @"wwwroot/Images";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ImageService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> SaveImageAsync(IFormFile imageFile, string folderName)
    {
        if (imageFile == null || imageFile.Length == 0)
            throw new ArgumentException("No file provided");

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

        var fullPath = Path.Combine(_path, folderName, fileName);

        if (!Directory.Exists(Path.Combine(_path, folderName)))
        {
            Directory.CreateDirectory(Path.Combine(_path, folderName));
        }

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }
        return fileName;
    }

  public string GenerateImageUrl(string? folderName = null, string? fileName = null)
  {
    if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
    {
      return string.Empty;
    }
    var request = _httpContextAccessor.HttpContext.Request;
        var scheme = request.Scheme;
        var host = request.Host.Value; 
        var imageUrl = $"{scheme}://{host}/api/image/getimage?folderName={folderName}&fileName={fileName}";

        return imageUrl;
   }
    public string GenerateQRCodeLocationPath( string fileName)
    { 
        var imagePath = $"{_path}/{Constants.QrCodeImages}/{fileName}";

        return imagePath;
    }

    public bool DeleteImage(string folderName, string fileName)
    {
        var fullPath = Path.Combine(_path, folderName, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            return true;
        }

        return false;
    }

    public async Task<string> UpdateImageAsync(IFormFile newImageFile, string folderName , string oldFileName = "")
    {
        if (!string.IsNullOrEmpty(oldFileName))
        { 
            DeleteImage(folderName, oldFileName);
        }
        return await SaveImageAsync(newImageFile, folderName);
    }
    public async Task<string> GetImage(string folderName, string fileName)
    {
        var request = _httpContextAccessor.HttpContext.Request;
        var scheme = request.Scheme;
        var host = request.Host.Value;
        var imageUrl = $"{scheme}://{host}/{_path}/{folderName}/{fileName}";

        return imageUrl;
    }

  
}
