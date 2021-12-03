using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface ICampaignAuditService
    {
        Task<ReturnResult> GetCampaignDashboardSummariesOperatorByCampaign(int campaignId);
        Task<ReturnResult> GetPlayDetailsForOperatorByCampaign(PagingSearchClass paging);

        Task<ReturnResult> GetDashboardSummariesByOperator(int operatorId);
        Task<ReturnResult> GetPromoCampaignDashboardSummary(int campaignId);
        Task<ReturnResult> GetPromoPlayDetails(PagingSearchClass paging);
        Task<ReturnResult> GetDashboardSummaryForSalesManager();
        Task<ReturnResult> GetDashboardSummaryForSalesExec(int userId);
    }
}
