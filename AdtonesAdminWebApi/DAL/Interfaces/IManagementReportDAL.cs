using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IManagementReportDAL
    {
        Task<int> GetreportInts(ManagementReportsSearch search, string query);
        Task<IEnumerable<SpendCredit>> GetTotalCreditCost(ManagementReportsSearch search, string query);
    }
}
