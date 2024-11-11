
namespace Bahrin.Harbour.Data.DBCollections
{
    public class Property : BaseEntity
    {
        public string? UploadedImageName { get; set; }
        public string? UploadedImageFolderName { get; set; }
        public Guid? ClientUserId { get; set; }
        public Guid? ClientId { get; set; }
        public string? TypeOfProperty { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public long? PropertyPrice { get; set; }
  }
}
