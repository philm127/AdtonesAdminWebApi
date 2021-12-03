using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ICampaignAuditDAL
    {
        Task<CampaignDashboardChartPREResult> GetCampaignDashboardSummariesAdvertisers(int userId, int campaignId=0);

        // Task<CampaignDashboardChartPREResult> GetCampaignDashboardSummariesForOperator(int campaignId);

        // Task<CampaignDashboardChartPREResult> GetDashboardSummariesForOperator(int operatorId);
        Task<IEnumerable<CampaignAuditOperatorTable>> GetPlayDetailsByCampaign(PagingSearchClass paging);
        Task<IEnumerable<CampaignAuditOperatorTable>> GetPlayDetailsByCampaignCount(PagingSearchClass param);
        Task<IEnumerable<PromoCampaignPlaylist>> GetPromoPlayDetails(PagingSearchClass paging);
        // Task<CampaignDashboardChartPREResult> GetDashboardSummariesForSalesManager();
        // Task<CampaignDashboardChartPREResult> GetDashboardSummariesForSalesExec(int sid);

        Task<CampaignDashboardChartPREResult> CampaignDashboardSummariesOperators(int campaignId);
        Task<CampaignDashboardChartPREResult> DashboardSummariesOperators(int operatorId);
        Task<CampaignDashboardChartPREResult> DashboardSummariesSalesManager(int salesmanId);
        Task<CampaignDashboardChartPREResult> DashboardSummariesSalesExec(int salesexecId);
        Task<CampaignDashboardChartResult> CampaignPromoDashboardSummaries(int campaignId);
    }
}
