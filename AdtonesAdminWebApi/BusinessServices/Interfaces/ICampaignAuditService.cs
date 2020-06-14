using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ICampaignAuditService
    {
        Task<ReturnResult> GetCampaignDashboardSummariesOperators(int campaignId);
        Task<ReturnResult> GetPlayDetailsForOperatorByCampaign(PagingSearchClass paging);
    }
}
