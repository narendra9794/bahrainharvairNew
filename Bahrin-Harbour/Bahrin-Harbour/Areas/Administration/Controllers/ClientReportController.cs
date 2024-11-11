using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Bahrin.Harbour.Model.ReportModel;
using Bahrin.Harbour.Service.Reportservices;
using Bahrin.Harbour.Data.ClientDA;
using Bahrin.Harbour.Data.DataContext;


namespace Bahrin_Harbour.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("[area]/[controller]/[action]")]
    public class ClientReportController : Controller
    {
        private readonly BahrinHarbourContext _context;

        private readonly IClientReports _clientReports;
        public ClientReportController(IClientReports clientservices, BahrinHarbourContext context)
        {
            _clientReports = clientservices;
            _context = context;
        }

        public IActionResult Clientreport()
        {
            return View();
        }

        /// <summary>
        /// Client report
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="download"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ClientReportD(string StartDate, string EndDate, bool download = false)
        {
            DateTime startdate = Convert.ToDateTime(StartDate);
            DateTime enddate = Convert.ToDateTime(EndDate);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                var clientData = _clientReports.Getclientdetails(startdate, enddate);

                if (download)
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Client Report");
                        worksheet.Cells[2, 1].Value = "StartDate";
                        worksheet.Cells[2, 1].Style.Font.Bold = true;
                        worksheet.Cells[2, 2].Style.Font.Bold = true;
                        worksheet.Cells[2, 1].Style.Font.Size = 12;
                        worksheet.Cells[2, 2].Value = startdate;
                        worksheet.Cells[2, 2].Style.Numberformat.Format = "dd/MM/yyyy";
                        worksheet.Cells[2, 3].Value = "EndDate";
                        worksheet.Cells[2, 4].Style.Font.Bold = true;
                        worksheet.Cells[2, 3].Style.Font.Bold = true;
                        worksheet.Cells[2, 3].Style.Font.Size = 12;
                        worksheet.Cells[2, 4].Value = enddate;
                        worksheet.Cells[2, 4].Style.Numberformat.Format = "dd/MM/yyyy";
                        int row = 4;
                        worksheet.Cells[1, 2].Value = "Client Report";
                        worksheet.Cells[1, 2].Style.Font.Bold = true;
                        worksheet.Cells[1, 2].Style.Font.Size = 25;
                        worksheet.Cells[row, 1].Value = "Client ID";
                        worksheet.Cells[row, 2].Value = "Client Name";
                        worksheet.Cells[row, 3].Value = "Properties";
                        worksheet.Cells[row, 4].Value = "No.Of Visit";
                        worksheet.Cells[row, 5].Value = "No.Of Checkin";
                        worksheet.Cells[row, 6].Value = "Last Visit";
                        worksheet.Cells[row, 6].Style.Font.Bold = true;
                        worksheet.Cells[row, 6].Style.Font.Size = 15;
                        worksheet.Cells[row, 1].Style.Font.Size = 15;
                        worksheet.Cells[row, 2].Style.Font.Size = 15;
                        worksheet.Cells[row, 3].Style.Font.Size = 15;
                        worksheet.Cells[row, 4].Style.Font.Size = 15;
                        worksheet.Cells[row, 5].Style.Font.Size = 15;
                        worksheet.Column(1).Width = 15;
                        worksheet.Column(2).Width = 20;
                        worksheet.Column(3).Width = 20;
                        worksheet.Column(4).Width = 20;
                        worksheet.Column(5).Width = 20;
                        worksheet.Cells[row, 1].Style.Font.Bold = true;
                        worksheet.Cells[row, 2].Style.Font.Bold = true;
                        worksheet.Cells[row, 3].Style.Font.Bold = true;
                        worksheet.Cells[row, 4].Style.Font.Bold = true;
                        worksheet.Cells[row, 5].Style.Font.Bold = true;
                        for (int i = 0; i < clientData.Count; i++)
                        {
                            worksheet.Cells[i + 5, 1].Value = clientData[i].ClientId;
                            worksheet.Cells[i + 5, 2].Value = clientData[i].Name;
                            worksheet.Cells[i + 5, 3].Value = clientData[i].Properties;
                            worksheet.Cells[i + 5, 4].Value = clientData[i].NoOfVisit;
                            worksheet.Cells[i + 5, 5].Value = clientData[i].NoCheckIn;
                            worksheet.Cells[i + 5, 6].Value = clientData[i].LastVisit;
                            worksheet.Cells[i + 5, 6].Style.Numberformat.Format = "dd/MM/yyyy";
                        }
                        var stream = new MemoryStream();
                        package.SaveAs(stream);
                        var fileName = $"ClientReport_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                        stream.Position = 0;
                        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
                else
                {
                    return Json(clientData);
                }
            }
            catch (Exception ex)
            {

                return BadRequest("An error occurred while processing your request.");
            }
        }
        /// <summary>
        /// dropdownoutlet
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Outletdropdowndata()
        {
            try
            {
                var loyaltyCardData = _clientReports.GetDropdowndetails();
                return Json(loyaltyCardData);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while processing your request.");
            }
        }

        public IActionResult LoyaltyCardreport()
        {
            return View();
        }
        /// <summary>
        /// LoyaltyCard Generated
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="download"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LoyaltyCardrepordata(string StartDate, string EndDate, bool download = false)
        {
            DateTime startdate = Convert.ToDateTime(StartDate);
            DateTime enddate = Convert.ToDateTime(EndDate);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                var loyaltyCardData = _clientReports.GetLoyaltyCarddetails(startdate, enddate);
                if (download)
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("OutletVist Report");
                        worksheet.Cells[2, 1].Value = "StartDate";
                        worksheet.Cells[2, 1].Style.Font.Bold = true;
                        worksheet.Cells[2, 2].Style.Font.Bold = true;
                        worksheet.Cells[2, 1].Style.Font.Size = 12;
                        worksheet.Cells[2, 2].Value = startdate;
                        worksheet.Cells[2, 2].Style.Numberformat.Format = "dd/MM/yyyy";
                        worksheet.Cells[2, 3].Value = "EndDate";
                        worksheet.Cells[2, 4].Style.Font.Bold = true;
                        worksheet.Cells[2, 3].Style.Font.Bold = true;
                        worksheet.Cells[2, 3].Style.Font.Size = 12;
                        worksheet.Cells[2, 4].Value = enddate;
                        worksheet.Column(4).Width = 15;
                        worksheet.Cells[2, 4].Style.Numberformat.Format = "dd/MM/yyyy";
                        int row = 3;
                        worksheet.Cells[1, 2].Value = "Loyalty Card Report";
                        worksheet.Cells[1, 2].Style.Font.Bold = true;
                        worksheet.Cells[1, 2].Style.Font.Size = 30;
                        worksheet.Cells[row, 1].Value = "User ID";
                        worksheet.Cells[row, 1].Style.Font.Size = 15;
                        worksheet.Cells[row, 2].Value = "Name";
                        worksheet.Cells[row, 2].Style.Font.Size = 15;
                        worksheet.Cells[row, 3].Value = "Generated On";
                        worksheet.Cells[row, 3].Style.Font.Size = 15;
                        worksheet.Column(1).Width = 37;
                        worksheet.Column(2).Width = 30;
                        worksheet.Column(3).Width = 20;
                        worksheet.Cells[row, 1].Style.Font.Bold = true;
                        worksheet.Cells[row, 2].Style.Font.Bold = true;
                        worksheet.Cells[row, 3].Style.Font.Bold = true;
                        int row1 = 4;
                        var groupedData = loyaltyCardData.GroupBy(x => x.CreatedDatecard.Date);
                        foreach (var group in groupedData)
                        {
                            worksheet.Cells[row1, 1].Value = "Date: " + group.Key.ToString("dd/MM/yyyy");
                            worksheet.Cells[row1, 1].Style.Font.Bold = true;
                            worksheet.Cells[row, 1].Style.Font.Size = 13;
                            worksheet.Cells[row1, 2].Value = "";
                            worksheet.Cells[row1, 3].Value = "";
                            row1++;
                            int cardCounter = 0;
                            foreach (var card in group)
                            {
                                worksheet.Cells[row1, 1].Value = card.UserId;
                                worksheet.Cells[row1, 2].Value = card.Name;
                                worksheet.Cells[row1, 3].Value = card.CreatedDatecard.ToString("dd/MM/yyyy");
                                row1++;
                                cardCounter++;
                            }
                            worksheet.Cells[row1, 3].Value = $"{cardCounter} Cards";
                            worksheet.Cells[row1, 3].Style.Font.Bold = true;
                            worksheet.Cells[row1, 3].Style.Font.Size = 13;
                            row1++;
                            worksheet.Cells[row1, 1].Value = "";
                            row1++;
                        }
                        var stream = new MemoryStream();
                        package.SaveAs(stream);
                        var fileName = $"LoyaltyCardReport_{DateTime.Now:yyyyMMdd}.xlsx";
                        stream.Position = 0;
                        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
                else
                {
                    return Json(loyaltyCardData);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while processing your request.");
            }
        }
        public IActionResult Outletreport()
        {
            return View();
        }
        /// <summary>
        /// OutletVisitrepordata
        /// </summary>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="Id"></param>
        /// <param name="download"></param>
        /// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> OutletVisitrepordata(string StartDate, string EndDate, string Id, bool download = false)
        //{
        //    DateTime startdate = Convert.ToDateTime(StartDate);
        //    DateTime enddate = Convert.ToDateTime(EndDate);
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        //    try
        //    {
        //        List<Guid> AlloutletId = new List<Guid>();
        //        if (Id == "0")
        //        {
        //            AlloutletId = _context.Outlets.Where(x => x.AciveStatus == true).Select(y => y.Id).ToList();


        //        }
        //        else
        //        {
        //            AlloutletId.Add(Guid.Parse(Id));
        //        }


        //        //var outletportNAME = _context.Outlets.Where(X=>X.Id== Id).Select(X=>X.Name).FirstOrDefault();

        //        var outletvistData = _clientReports.GetOutletvisitdetails(startdate, enddate, AlloutletId);

        //        if (download)
        //        {
        //            using (var package = new ExcelPackage())
        //            {
        //                var worksheet = package.Workbook.Worksheets.Add("outlet Report");
        //                worksheet.Cells[2, 1].Value = "StartDate";
        //                worksheet.Cells[2, 1].Style.Font.Bold = true;
        //                worksheet.Cells[2, 1].Style.Font.Size = 12;
        //                worksheet.Cells[2, 2].Style.Font.Bold = true;
        //                worksheet.Cells[2, 2].Value = startdate;
        //                worksheet.Cells[2, 2].Style.Numberformat.Format = "dd/MM/yyyy";
        //                worksheet.Cells[2, 3].Value = "EndDate";
        //                worksheet.Cells[2, 3].Style.Font.Bold = true;
        //                worksheet.Cells[2, 3].Style.Font.Size = 12;
        //                worksheet.Cells[2, 4].Value = enddate;
        //                worksheet.Cells[2, 4].Style.Font.Bold = true;
        //                worksheet.Column(4).Width = 25;
        //                worksheet.Cells[2, 4].Style.Numberformat.Format = "dd/MM/yyyy";
        //                int row = 4;
        //                worksheet.Cells[1, 2].Value = "Outlet Visit Report";
        //                worksheet.Cells[1, 2].Style.Font.Bold = true;
        //                worksheet.Cells[1, 2].Style.Font.Size = 30;
        //                worksheet.Cells[row, 1].Value = "Client ID";
        //                worksheet.Column(1).Width = 25;
        //                worksheet.Cells[row, 2].Value = "Client Name";
        //                worksheet.Column(2).Width = 25;
        //                worksheet.Cells[row, 3].Value = "Representative Name";
        //                worksheet.Cells[row, 3].Style.Font.Size = 18;
        //                worksheet.Cells[row, 3].Style.Font.Bold = true;
        //                worksheet.Column(3).Width = 40;
        //                worksheet.Cells[row, 4].Value = "Representative Discount";
        //                worksheet.Cells[row, 4].Style.Font.Size = 18;
        //                worksheet.Cells[row, 4].Style.Font.Bold = true;
        //                worksheet.Column(4).Width = 40;
        //                worksheet.Cells[row, 5].Value = "Checkin";
        //                worksheet.Cells[row, 5].Style.Font.Size = 18;
        //                worksheet.Cells[row, 5].Style.Font.Bold = true;
        //                worksheet.Column(5).Width = 40;
        //                worksheet.Cells[row, 6].Value = "Visit";
        //                worksheet.Cells[row, 6].Style.Font.Size = 18;
        //                worksheet.Cells[row, 6].Style.Font.Bold = true;
        //                worksheet.Column(6).Width = 40;
        //                worksheet.Cells[3, 1].Value = "Outlet Name:";
        //                worksheet.Cells[3, 1].Style.Font.Size = 12;
        //                worksheet.Cells[3, 1].Style.Font.Bold = true;
        //                //worksheet.Cells[3, 2].Value = outletportNAME;
        //                worksheet.Cells[3, 2].Style.Font.Bold = true;
        //                worksheet.Cells[row, 1].Style.Font.Size = 15;
        //                worksheet.Cells[row, 2].Style.Font.Size = 15;
        //                worksheet.Column(1).Width = 10;
        //                worksheet.Column(2).Width = 25;
        //                worksheet.Cells[row, 1].Style.Font.Bold = true;
        //                worksheet.Cells[row, 2].Style.Font.Bold = true;
        //                for (int i = 0; i < outletvistData.Count; i++)
        //                {
        //                    worksheet.Cells[i + 5, 1].Value = outletvistData[i].Clientid;
        //                    worksheet.Cells[i + 5, 2].Value = outletvistData[i].ClientName;
        //                    worksheet.Cells[i + 5, 3].Value = outletvistData[i].RepresentativeName;
        //                    worksheet.Cells[i + 5, 4].Value = outletvistData[i].SalesRefersDIS;
        //                    worksheet.Cells[i + 5, 5].Value = outletvistData[i].checkinDate;
        //                    worksheet.Cells[i + 5, 6].Value = outletvistData[i].VisitDate;
        //                    worksheet.Cells[i + 5, 5].Style.Numberformat.Format = "dd/MM/yyyy";
        //                    worksheet.Cells[i + 5, 6].Style.Numberformat.Format = "dd/MM/yyyy";

        //                }
        //                var stream = new MemoryStream();
        //                package.SaveAs(stream);
        //                var fileName = $"OutletvisitReport_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        //                stream.Position = 0;
        //                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //            }
        //        }
        //        else
        //        {
        //            return Json(outletvistData);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest("An error occurred while processing your request.");
        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> OutletVisitrepordata(string StartDate, string EndDate, string Id, bool download = false)
        {
            DateTime startdate = Convert.ToDateTime(StartDate);
            DateTime enddate = Convert.ToDateTime(EndDate);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            try
            {
                List<Guid> AlloutletId = new List<Guid>();
                if (Id == "0")
                {
                    AlloutletId = _context.Outlets.Where(x => x.AciveStatus == true).Select(y => y.Id).ToList();
                }
                else
                {
                    AlloutletId.Add(Guid.Parse(Id));
                }
                var outletvistData = _clientReports.GetOutletvisitdetails(startdate, enddate, AlloutletId);

                if (download)
                {
                    using (var package = new ExcelPackage())
                    {
                        var worksheet = package.Workbook.Worksheets.Add("Outlet Report");

                        
                        worksheet.Cells[1, 2].Value = "Outlet Visit Report";
                        worksheet.Cells[1, 2].Style.Font.Bold = true;
                        worksheet.Cells[1, 2].Style.Font.Size = 30;
                        

                        worksheet.Cells[2, 1].Value = "StartDate";
                        worksheet.Cells[2, 1].Style.Font.Bold = true;
                        worksheet.Cells[2, 1].Style.Font.Size = 12;
                        worksheet.Cells[2, 2].Value = startdate;
                        worksheet.Cells[2, 2].Style.Numberformat.Format = "dd/MM/yyyy";
                        worksheet.Column(1).Width = 20;

                        worksheet.Cells[2, 3].Value = "EndDate";
                        worksheet.Cells[2, 3].Style.Font.Bold = true;
                        worksheet.Cells[2, 3].Style.Font.Size = 12;
                        worksheet.Cells[2, 4].Value = enddate;
                        worksheet.Cells[2, 4].Style.Numberformat.Format = "dd/MM/yyyy";
                        worksheet.Column(4).Width = 20;
                        int row = 4;
                        worksheet.Cells[row, 1].Value = "Client ID";
                        worksheet.Column(1).Width = 20;
                        worksheet.Column(2).Width = 20;
                        worksheet.Column(3).Width = 38;
                        worksheet.Column(4).Width = 40;
                        worksheet.Column(5).Width = 20;
                        worksheet.Column(6).Width = 20;
                        worksheet.Cells[row, 2].Value = "Client Name";
                        worksheet.Cells[row, 3].Value = "Representative Name";
                        worksheet.Cells[row, 4].Value = "Representative Discount";
                        worksheet.Cells[row, 5].Value = "Checkin";
                        worksheet.Cells[row, 6].Value = "Visit";
                        for (int i = 1; i <= 6; i++)
                        {
                            worksheet.Cells[row, i].Style.Font.Bold = true;
                            worksheet.Cells[row, i].Style.Font.Size = 15;
                        }

                        row += 1; 

                        var groupedData = outletvistData.GroupBy(o => o.OutletName);
                        foreach (var outletGroup in groupedData)
                        {
                            worksheet.Cells[row, 1].Value = "Outlet Name:";
                            worksheet.Cells[row, 2].Value = outletGroup.Key;
                            worksheet.Cells[row, 1].Style.Font.Bold = true;
                            worksheet.Cells[row, 2].Style.Font.Bold = true;
                            worksheet.Cells[row, 1].Style.Font.Size = 13;
                            row++;

                            
                            foreach (var data in outletGroup)
                            {
                                worksheet.Cells[row, 1].Value = data.Clientid;
                                worksheet.Cells[row, 2].Value = data.ClientName;
                                worksheet.Cells[row, 3].Value = data.RepresentativeName;
                                worksheet.Cells[row, 4].Value = data.SalesRefersDIS;
                                worksheet.Cells[row, 5].Value = data.checkinDate;
                                worksheet.Cells[row, 6].Value = data.VisitDate;

                                
                                worksheet.Cells[row, 5].Style.Numberformat.Format = "dd/MM/yyyy";
                                worksheet.Cells[row, 6].Style.Numberformat.Format = "dd/MM/yyyy";

                                row++;
                            }

                            row++; 
                        }
                        var stream = new MemoryStream();
                        package.SaveAs(stream);
                        var fileName = $"OutletvisitReport_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                        stream.Position = 0;
                        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
                else
                {
                    return Json(outletvistData);
                }
            }
            catch (Exception ex)
            {
                // Detailed error message
                return BadRequest("An error occurred while processing your request: " + ex.Message);
            }
        }

    }
}
