using AdtonesAdminWebApi.ViewModels;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IManagementReportService
    {
        Task<ReturnResult> GetReportData(ManagementReportsSearch search);
        // Task<XLWorkbook> GenerateExcelReport(ManagementReportsSearch search);
        Task<ManagementReportModel> GetCachedOrNewReportData(ManagementReportsSearch search);
    }
}
