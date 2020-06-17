using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ICampaignAuditDAL
    {
        Task<CampaignDashboardChartPREResult> GetCampaignDashboardSummariesOperators(int id = 0);
        Task<List<CampaignDashboardChartPREResult>> GetCampaignDashboardSummariesAdvertisers(int campid = 0, int userId = 0);

        Task<CampaignDashboardChartPREResult> GetCampaignDashboardSummariesForOperator(int campaignId);

        Task<CampaignDashboardChartPREResult> GetDashboardSummariesForOperator(int operatorId);
        Task<IEnumerable<CampaignAuditOperatorTable>> GetPlayDetailsByCampaign(PagingSearchClass paging);
        Task<IEnumerable<CampaignAuditOperatorTable>> GetPlayDetailsByCampaignCount(PagingSearchClass param);
    }
}
