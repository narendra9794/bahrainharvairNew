using Bahrin.Harbour.Data.ClientDA;
using Bahrin.Harbour.Data.DBCollections;
using Bahrin.Harbour.Data.LoyalityCardDA;
using Bahrin.Harbour.Helper;
using Bahrin.Harbour.Model.AccountModel;
using Bahrin.Harbour.Model.ClientModel;
using Bahrin.Harbour.Service.QRCodeService;
using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.Extensions.Logging;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Bahrin.Harbour.Service.LoyalityCard
{
    public class LoyalityCardService : ILoyalityCardService
    {
        private readonly ILoyalityCardDA _loyalityCard;
        private readonly IClientDA _clientDA;
        private readonly ILogger<LoyalityCardDA> _logger;
        private readonly IQRCodeService _qRCode;

        private readonly string _cardPath = @"wwwroot/pdf/BahrinHarbourCard.pdf";

        public LoyalityCardService(ILoyalityCardDA loyalityCard, ILogger<LoyalityCardDA> logger, IClientDA clientDA, IQRCodeService qRCode)
        {
            _loyalityCard = loyalityCard;
            _logger = logger;
            _clientDA = clientDA;
            _qRCode = qRCode;
        }

        public async Task<StatusModel> CreateOrUpdateAsync(string ClientId, bool active)
        {
            StatusModel model = new StatusModel();
            var client = await _clientDA.GetClientByIdAsync(ClientId);

            /*if (client.isLoyalityCardGenerated)
            {
                model.status = Constants.False;
                model.message = "Card id Already Generated for this client.";
                return model;
            }*/

            client.isLoyalityCardGenerated = Constants.True;


            LoyaltyCard card = new LoyaltyCard
            {
                ClientId = client.ClientId,
                ClientGuid = client.Id,
                ExpireDate = null,
                AciveStatus = (bool)(active == null ? Constants.True : active),
                ContactNumber = client.Phone,
                Email = client.EmailAddress,
                ClientName = client.ClientName
            };

            var status = await _loyalityCard.SaveOrUpdateLoyalityCardAsync(card);
      if (status)
      {
        await _clientDA.UpdateClientLoyalityCardStatusAsync(client);
      }

      if (status == Constants.True)
            {
                model.status = Constants.True;
                model.message = "Card is Generated successfully";
                return model;
            }

            model.status = Constants.False;
            model.message = "There are some error occured while generating the card.";
            return model;
        }

        public async Task<List<LoyaltyCardViewModel>> GetAllCard()
        {
            List<LoyaltyCard> allCards = await _loyalityCard.GetAllCardsAsync();

            var allClient = await _clientDA.GetAllClients();

            List<LoyaltyCardViewModel> cards = allCards.Select(card => new LoyaltyCardViewModel
            {
                ClientGuid = allClient.FirstOrDefault(x=>x.Id == card.ClientGuid)?.Id,//1
                ClientId = allClient.FirstOrDefault(x => x.Id == card.ClientGuid)?.ClientId,//2
                ClientName = allClient.FirstOrDefault(x => x.Id == card.ClientGuid)?.Name,//3
                Email = allClient.FirstOrDefault(x => x.Id == card.ClientGuid)?.EmailAddress,//4
                ContactNumber = allClient.FirstOrDefault(x => x.Id == card.ClientGuid)?.Phone,//5
                ExpireDate = card.ExpireDate,//6
                Active = card.AciveStatus//7
            }).ToList();

            return cards;
        }

        public async Task<LoyaltyCardViewModel> GetCardAsync(string ClientGuidId)
        {
            LoyaltyCard card = await _loyalityCard.GetCardsByIdAsync(ClientGuidId);

            LoyaltyCardViewModel cardViewModel = new LoyaltyCardViewModel
            {
                ClientGuid = card.ClientGuid,
                ClientId = card.ClientId,
                ClientName = card.ClientName,
                Email = card.Email,
                ContactNumber = card.ContactNumber,
                ExpireDate = card.ExpireDate,
                Active = card.AciveStatus
            };

            return cardViewModel;
        }

        public async Task<(byte[] PdfData, string QrCodeString)> CreatePdfWithQrCode( string ClientGuidId)
        {
            try
            {
                LoyaltyCard card = await _loyalityCard.GetCardsByIdAsync(ClientGuidId);

                if (card == null)
                {
                    return (null, null);
                }

                string fontPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", "larabiefontrg.ttf");
                PdfFont customFont = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);

                string fontPathPlusJakartaSansRegular = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", "PlusJakartaSans-Regular.ttf");
                PdfFont PlusJakartaSansRegular = PdfFontFactory.CreateFont(fontPathPlusJakartaSansRegular, PdfEncodings.IDENTITY_H);

                string fontPathPlusJakartaSansBold = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", "PlusJakartaSans-Bold.ttf");
                PdfFont PlusJakartaSansBold = PdfFontFactory.CreateFont(fontPathPlusJakartaSansBold, PdfEncodings.IDENTITY_H);

                var Client = await _clientDA.GetClientByIdAsync(ClientGuidId);

                var qrCodeBytes = await _qRCode.GenerateQrCodeAsync(Client.Id);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (PdfReader reader = new PdfReader(_cardPath))
                    {
                        PdfWriter writer = new PdfWriter(ms)
;

                        using (PdfDocument pdfDoc = new PdfDocument(reader, writer))
                        {
                            using (iText.Layout.Document document = new iText.Layout.Document(pdfDoc))
                            {
                                PdfPage page = pdfDoc.GetFirstPage();

                                ImageData imageData = ImageDataFactory.Create(qrCodeBytes);
                                Image qrCodeImg = new Image(imageData);

                                qrCodeImg.SetWidth(155);
                                qrCodeImg.SetHeight(155);
                                qrCodeImg.SetFixedPosition(115, 195);
                                document.Add(qrCodeImg);


                                Paragraph paragraphClientName = new Paragraph(Helper.Helper.SplitLongLines(Client.ClientName.ToUpper(), 35))
                                    .SetFontSize(10.25f)
                                    .SetFont(PlusJakartaSansRegular)
                                    .SetFixedPosition(84.95f, 407, 400)
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetFontColor(new DeviceRgb(255, 255, 255));

                                document.Add(paragraphClientName);

              /*                  Paragraph ValidFromText = new Paragraph("Valid From".ToUpper())
                                    .SetFontSize(6f)
                                    .SetFont(PlusJakartaSansBold)
                                    .SetFixedPosition(254f, 55f, 400)
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetFontColor(new DeviceRgb(255, 255, 255));

                                document.Add(ValidFromText);

                                Paragraph ValidFromDate = new Paragraph("09/24")
                                    .SetFontSize(15.17f)
                                    .SetFont(customFont)
                                    .SetFixedPosition(250.5f, 39f, 400)
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetFontColor(new DeviceRgb(255, 255, 255));

                                document.Add(ValidFromDate);

                                Paragraph paragraphClientId = new Paragraph(Helper.Helper.FormatClientId(Client.ClientId))
                                    .SetFontSize(24.17f)
                                    .SetFont(customFont)
                                    .SetBold()
                                    .SetFixedPosition(24.95f, 36.4f, 400)
                                    .SetTextAlignment(TextAlignment.LEFT)
                                    .SetFontColor(new DeviceRgb(255, 255, 255));

                                document.Add(paragraphClientId);*/
                                document.Close();
                            }
                        }
                    }

                    return (ms.ToArray(), Client.ClientName.ToUpper() + Helper.Helper.FormatClientId(Client.ClientId));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error generating PDF with QR code", ex);
            }
        }
       

    }
}
