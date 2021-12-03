using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository;
{
    public interface IManagementReportDAL
    {
        Task<int> GetreportInts(ManagementReportsSearch search, string query, int ops, string conn);
        Task<CampaignTableManReport> GetReportPlayLengths(ManagementReportsSearch search, string query, int ops, string conn);
        Task<IEnumerable<SpendCredit>> GetTotalCreditCost(ManagementReportsSearch search, string query, int op, string conn, int exp);
        Task<IEnumerable<SpendCredit>> GetTotalCreditCostProv(ManagementReportsSearch search, string query, int ops, string conn, int exp);
        // Task<IEnumerable<SpendCredit>> GetTotalCost(ManagementReportsSearch search, string query);
        // Task<IEnumerable<SpendCredit>> GetTotalCredit(ManagementReportsSearch search, string query);
        Task<ManRepUsers> GetManReportsForUsers(ManagementReportsSearch search, string query, int ops, string conn);
        Task<IEnumerable<int>> GetAllOperators();
        Task<IEnumerable<string>> GetOperatorNames(ManagementReportsSearch search);
        Task<TwoDigitsManRep> GetreportDoubleInts(ManagementReportsSearch search, string query, int ops, string conn, bool useOpId = false);
        Task<RewardsManModel> GetReportRewards(ManagementReportsSearch search, string query, int ops, string conn);
    }
}
