using AdtonesAdminWebApi.BusinessServices.ManagementReports;
using AdtonesAdminWebApi.BusinessServices.ManagementReports.ManagementReportParts;
using Microsoft.Extensions.DependencyInjection;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {

            services.AddTransient<ISharedSelectListsService, SharedSelectListsService>();
            services.AddTransient<IUserManagementService, UserManagementService>();
            services.AddTransient<IPromotionalCampaignService, PromotionalCampaignService>();
            services.AddTransient<ISalesManagementService, SalesManagementService>();
            services.AddTransient<IProfileMatchInfoService, ProfileMatchInfoService>();
            services.AddTransient<ICountryAreaService, CountryAreaService>();
            services.AddTransient<IUserDashboardService, UserDashboardService>();
            services.AddTransient<IOperatorConfigService, OperatorConfigService>();
            services.AddTransient<IOperatorService, OperatorService>();
            services.AddTransient<ISystemConfigService, SystemConfigService>();
            services.AddTransient<IRewardsService, RewardsService>();
            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<IAdvertService, AdvertService>();
            services.AddTransient<IAdvertCategoryService, AdvertCategoryService>();
            services.AddTransient<ICampaignService, CampaignService>();
            services.AddTransient<IUserProfileService, UserProfileService>();
            services.AddTransient<ICampaignAuditService, CampaignAuditService>();
            services.AddTransient<IPermissionManagementService, PermisionManagementService>();
            services.AddTransient<IUserCreditService, UserCreditService>();
            
            services.AddTransient<ICreateUpdateCampaignService, CreateUpdateCampaignService>();
            services.AddTransient<ICreateCheckSaveProfileModels, CreateCheckSaveProfileModels>();
            services.AddTransient<IBillingService, BillingService>();

            // ManagementReport
            services.AddTransient<IManagementReportService, ManagementReportService>();
            services.AddTransient<IGetConvertedCurrency, GetConvertedCurrency>();
            services.AddTransient<ICalculateConvertedSpendCredit, CalculateConvertedSpendCredit>();
            services.AddTransient<IGenerateReportDataByOperator, GenerateReportDataByOperator>();
            services.AddTransient<ISetDefaultParameters, SetDefaultParameters>();
            // services.AddTransient<ICreateExelManagementReport, CreateExelManagementReport>();
            

            services.AddTransient<ITotalSubscribers, TotalSubscribers>();
            services.AddTransient<ITotalPlays, TotalPlays>();
            services.AddTransient<ITotalListened, TotalListened>();
            services.AddTransient<ITotalCampaigns, TotalCampaigns>();
            services.AddTransient<ITotalAdverts, TotalAdverts>();
            services.AddTransient<IRewards, Rewards>();
            services.AddTransient<IAmountSpentInPeriod, AmountSpentInPeriod>();
            services.AddTransient<IAmountOfCredit, AmountOfCredit>();
            services.AddTransient<IAmountPaid, AmountPaid>();
            services.AddTransient<ITotalSpent, TotalSpent>();
            

            return services;
        }
    }
}
