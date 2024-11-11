using Bahrin.Harbour.Helper;
using QRCoder;
using System.IO;
using System.Threading.Tasks;

namespace Bahrin.Harbour.Service.QRCodeService
{
    public class QRCodeService : IQRCodeService
    {
        public async Task<byte[]> GenerateQrCodeAsync(Guid userId)
        {
            try
            {
                var strUserId = userId.ToString();

                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(strUserId, QRCodeGenerator.ECCLevel.Q))
                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                {
                    return qrCode.GetGraphic(20, System.Drawing.Color.White, System.Drawing.Color.Transparent);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while generating the QR code.", ex);
            }
        }

    }

}
