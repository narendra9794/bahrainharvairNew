
namespace Bahrin.Harbour.Service.QRCodeService
{
    public interface IQRCodeService
    {
        Task<byte[]> GenerateQrCodeAsync(Guid userId);
    }
}