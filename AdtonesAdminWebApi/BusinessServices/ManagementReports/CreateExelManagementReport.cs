using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.ManagementReports
{
    public interface ICreateExelManagementReport
    {
        Task<byte[]> GenerateExcelManagementReport(ManagementReportsSearch search);
    }

    public abstract class CreateExelManagementReport : ICreateExelManagementReport
    {
        private readonly IManagementReportDAL _reportDAL;
        private readonly ISetDefaultParameters _defaults;
        private readonly IManagementReportService _reportService;

        public CreateExelManagementReport(IManagementReportDAL reportDAL, ISetDefaultParameters defaults, IManagementReportService reportService)
        {
            _reportDAL = reportDAL;
            _defaults = defaults;
            _reportService = reportService;
        }


        public async Task<byte[]> GenerateExcelManagementReport(ManagementReportsSearch search)
        {
            var model = await _reportService.GetCachedOrNewReportData(search);
            var defaultSearch = _defaults.SetDefaults(search);
            var wb = Create(model, defaultSearch);
            byte[] filebyte;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    filebyte = memoryStream.ToArray();
                    //string s = Convert.ToBase64String(filebyte);
                    //var FileName = ContentDispositionHeaderValue.Parse(fileName.Trim('"'));

                    //var result = new HttpResponseMessage(HttpStatusCode.OK)
                    //{
                    //    Content = new ByteArrayContent(memoryStream.ToArray())
                    //};
                    //result.Content.Headers.ContentDisposition =
                    //    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    //    {
                    //        FileName = fileName
                    //    };
                    //result.Content.Headers.ContentType =
                    //    new MediaTypeHeaderValue("application/vnd.ms-excel");

                    return filebyte;
                }
            }
            catch (Exception ex)
            {
                //var _logging = new ErrorLogging()
                //{
                //    ErrorMessage = ex.Message.ToString(),
                //    StackTrace = ex.StackTrace.ToString(),
                //    PageName = "CampaignAuditController",
                //    ProcedureName = "GenerateManReport"
                //};
                //_logging.LogError();
                return null;
            }
        }


        private XLWorkbook Create(ManagementReportModel model, ManagementReportsSearch search)
        {

            var wb = new XLWorkbook();

            List<ManagementReportModel> mappingResult = new List<ManagementReportModel>();
            mappingResult.Add(model);

            string[] operatorArray = _reportDAL.GetOperatorNames(search).Result.ToList().ToArray();

            string operatorName = string.Join(", ", operatorArray);

            string fromDate = "", toDate = "";
            fromDate = search.DateFrom.ToString();
            toDate = search.DateTo.ToString();


            var ws = wb.Worksheets.Add("Management Report");
            ws.Style.Font.FontSize = 9;
            ws.Range("A1" + ":" + "N1").Merge().Value = "Management Report Data";
            ws.Range("A1" + ":" + "N1").Style.Font.FontSize = 14;
            ws.Range("A1" + ":" + "N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("A1" + ":" + "N1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("A1" + ":" + "N1").Style.Font.Bold = true;
            ws.Columns("A:M").Width = 25;

            ws.Range("A2" + ":" + "B2").Merge().Value = "Operator";
            ws.Range("A2" + ":" + "B2").Style.Font.FontSize = 12;
            ws.Range("A2" + ":" + "B2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("A2" + ":" + "B2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("A2" + ":" + "B2").Style.Font.Bold = true;
            ws.Columns("A:B").Width = 10;

            ws.Range("C2" + ":" + "D2").Merge().Value = operatorName.ToString();
            ws.Range("C2" + ":" + "D2").Style.Font.FontSize = 10;
            ws.Range("C2" + ":" + "D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("C2" + ":" + "D2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Columns("C:D").Width = 10;

            ws.Range("A3" + ":" + "B3").Merge().Value = "Date";
            ws.Range("A3" + ":" + "B3").Style.Font.FontSize = 12;
            ws.Range("A3" + ":" + "B3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("A3" + ":" + "B3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("A3" + ":" + "B3").Style.Font.Bold = true;
            ws.Columns("A:B").Width = 10;

            ws.Range("C3" + ":" + "D3").Merge().Value = fromDate + " - " + toDate;
            ws.Range("C3" + ":" + "D3").Style.Font.FontSize = 10;
            ws.Range("C3" + ":" + "C3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("C3" + ":" + "C3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Columns("C:D").Width = 10;

            ws.Range("A4" + ":" + "A5").Merge().Value = "Total Users";
            ws.Range("A4" + ":" + "A5").Style.Font.FontSize = 12;
            ws.Range("A4" + ":" + "A5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("A4" + ":" + "A5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("A4" + ":" + "A5").Style.Font.Bold = true;
            ws.Column("A").Width = 15;

            ws.Range("B4" + ":" + "B5").Merge().Value = "Removed Users";
            ws.Range("B4" + ":" + "B5").Style.Font.FontSize = 12;
            ws.Range("B4" + ":" + "B5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("B4" + ":" + "B5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("B4" + ":" + "B5").Style.Font.Bold = true;
            ws.Column("B").Width = 18;

            ws.Range("C4" + ":" + "C5").Merge().Value = "Plays";
            ws.Range("C4" + ":" + "C5").Style.Font.FontSize = 12;
            ws.Range("C4" + ":" + "C5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("C4" + ":" + "C5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("C4" + ":" + "C5").Style.Font.Bold = true;
            ws.Column("C").Width = 10;

            ws.Range("D4" + ":" + "D5").Merge().Value = "Plays (Under 6sec)";
            ws.Range("D4" + ":" + "D5").Style.Font.FontSize = 12;
            ws.Range("D4" + ":" + "D5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("D4" + ":" + "D5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("D4" + ":" + "D5").Style.Font.Bold = true;
            ws.Column("D").Width = 20;

            ws.Range("E4" + ":" + "E5").Merge().Value = "SMS";
            ws.Range("E4" + ":" + "E5").Style.Font.FontSize = 12;
            ws.Range("E4" + ":" + "E5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("E4" + ":" + "E5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("E4" + ":" + "E5").Style.Font.Bold = true;
            ws.Column("E").Width = 10;

            ws.Range("F4" + ":" + "F5").Merge().Value = "Email";
            ws.Range("F4" + ":" + "F5").Style.Font.FontSize = 12;
            ws.Range("F4" + ":" + "F5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("F4" + ":" + "F5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("F4" + ":" + "F5").Style.Font.Bold = true;
            ws.Column("F").Width = 10;

            ws.Range("G4" + ":" + "G5").Merge().Value = "Live Campaign";
            ws.Range("G4" + ":" + "G5").Style.Font.FontSize = 12;
            ws.Range("G4" + ":" + "G5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("G4" + ":" + "G5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("G4" + ":" + "G5").Style.Font.Bold = true;
            ws.Column("G").Width = 18;

            ws.Range("H4" + ":" + "H5").Merge().Value = "Ads provisioned";
            ws.Range("H4" + ":" + "H5").Style.Font.FontSize = 12;
            ws.Range("H4" + ":" + "H5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("H4" + ":" + "H5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("H4" + ":" + "H5").Style.Font.Bold = true;
            ws.Column("H").Width = 20;

            ws.Range("I4" + ":" + "I5").Merge().Value = $"Total Spend (in {model.CurrencyCode})";
            ws.Range("I4" + ":" + "I5").Style.Font.FontSize = 12;
            ws.Range("I4" + ":" + "I5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("I4" + ":" + "I5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("I4" + ":" + "I5").Style.Font.Bold = true;
            ws.Column("I").Width = 25;

            ws.Range("J4" + ":" + "J5").Merge().Value = $"Total Credit (in {model.CurrencyCode})";
            ws.Range("J4" + ":" + "J5").Style.Font.FontSize = 12;
            ws.Range("J4" + ":" + "J5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("J4" + ":" + "J5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("J4" + ":" + "J5").Style.Font.Bold = true;
            ws.Column("J").Width = 25;

            ws.Range("K4" + ":" + "K5").Merge().Value = "Total Cancel";
            ws.Range("K4" + ":" + "K5").Style.Font.FontSize = 12;
            ws.Range("K4" + ":" + "K5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("K4" + ":" + "K5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("K4" + ":" + "K5").Style.Font.Bold = true;
            ws.Column("K").Width = 15;

            ws.Range("L4" + ":" + "L5").Merge().Value = "Average Plays Per User";
            ws.Range("L4" + ":" + "L5").Style.Font.FontSize = 12;
            ws.Range("L4" + ":" + "L5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("L4" + ":" + "L5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("L4" + ":" + "L5").Style.Font.Bold = true;
            ws.Column("L").Width = 25;

            ws.Range("M4" + ":" + "M5").Merge().Value = "Text Files Processed";
            ws.Range("M4" + ":" + "M5").Style.Font.FontSize = 12;
            ws.Range("M4" + ":" + "M5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("M4" + ":" + "M5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("M4" + ":" + "M5").Style.Font.Bold = true;
            ws.Column("M").Width = 25;

            ws.Range("N4" + ":" + "N5").Merge().Value = "Text Lines Processed";
            ws.Range("N4" + ":" + "N5").Style.Font.FontSize = 12;
            ws.Range("N4" + ":" + "N5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("N4" + ":" + "N5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("N4" + ":" + "N5").Style.Font.Bold = true;
            ws.Column("N").Width = 25;



            int first = 5;
            int last = first;
            int excelrowno = first;
            if (mappingResult.Count() > 0)
            {
                for (int i = 0; i < mappingResult.Count(); i++)
                {
                    excelrowno += 1;
                    int j = excelrowno;


                    //ws.Cell("A" + j.ToString()).Value = mappingResult[i].NumOfTotalUser.ToString();
                    //            ws.Cell("B" + j.ToString()).Value = mappingResult[i].NumOfRemovedUser.ToString();
                    //            ws.Cell("C" + j.ToString()).Value = mappingResult[i].NumOfPlay.ToString();
                    //            ws.Cell("D" + j.ToString()).Value = mappingResult[i].NumOfPlayUnder6secs.ToString();
                    //            ws.Cell("E" + j.ToString()).Value = mappingResult[i].NumOfSMS.ToString();
                    //            ws.Cell("F" + j.ToString()).Value = mappingResult[i].NumOfEmail.ToString();
                    //            ws.Cell("G" + j.ToString()).Value = mappingResult[i].NumOfLiveCampaign.ToString();
                    //            ws.Cell("H" + j.ToString()).Value = mappingResult[i].NumberOfAdsProvisioned.ToString();
                    //            ws.Cell("I" + j.ToString()).Value = mappingResult[i].TotalSpend.ToString("N");
                    //            ws.Cell("J" + j.ToString()).Value = mappingResult[i].TotalCredit.ToString("N");
                    //            ws.Cell("K" + j.ToString()).Value = mappingResult[i].NumOfCancel.ToString();
                    //            ws.Cell("L" + j.ToString()).Value = mappingResult[i].AveragePlaysPerUser.ToString();
                    //            ws.Cell("M" + j.ToString()).Value = mappingResult[i].NumOfTextFile.ToString();
                    //            ws.Cell("N" + j.ToString()).Value = mappingResult[i].NumOfUpdateToAudit.ToString();
                }
            }

            return wb;
        }
    }
}