using Microsoft.AspNetCore.Http;

public interface IImageService
{
  string GenerateImageUrl(string? folderName = null, string? fileName = null);
    Task<string> SaveImageAsync(IFormFile imageFile, string folderName);
    bool DeleteImage(string folderName, string fileName);
    Task<string> UpdateImageAsync(IFormFile newImageFile, string folderName, string OldFileName);
    string GenerateQRCodeLocationPath(string fileName);
}