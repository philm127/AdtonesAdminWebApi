using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace AdtonesAdminWebApi.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDAL(this IServiceCollection services)
        {
            services.AddTransient<IConnectionStringService, ConnectionStringService>();
            services.AddTransient<ISharedSelectListsDAL, SharedSelectListsDAL>();
            services.AddTransient<ITicketDAL, TicketDAL>();
            services.AddTransient<IAdvertDAL, AdvertDAL>();
            services.AddTransient<IAdvertCategoryDAL, AdvertCategoryDAL>();
            services.AddTransient<ICountryAreaDAL, CountryAreaDAL>();
            services.AddTransient<ICampaignDAL, CampaignDAL>();
            services.AddTransient<ICampaignAdvertDAL, CampaignAdvertDAL>();
            services.AddTransient<IUserDashboardDAL, UserDashboardDAL>();
            services.AddTransient<IPromotionalCampaignDAL, PromotionalCampaignDAL>();
            services.AddTransient<ILoginDAL, LoginDAL>();
            services.AddTransient<IUserManagementDAL, UserManagementDAL>();
            services.AddTransient<IUserManagementAddUserDAL, UserManagementAddUserDAL>();
            services.AddTransient<IUserMatchDAL, UserMatchDAL>();
            services.AddTransient<IUserCreditDAL, UserCreditDAL>();
            services.AddTransient<ICampaignMatchDAL, CampaignMatchDAL>();
            services.AddTransient<IUserProfileDAL, UserProfileDAL>();
            services.AddTransient<ISoapDAL, SoapDAL>();
            services.AddTransient<ICampaignAuditDAL, CampaignAuditDAL>();
            services.AddTransient<ICurrencyDAL, CurrencyDAL>();
            
            services.AddTransient<IPermissionManagementDAL, PermissionManagementDAL>();
            services.AddTransient<IManagementReportDAL, ManagementReportDAL>();
            services.AddTransient<IFinanceTablesDAL, FinanceTablesDAL>();
            services.AddTransient<IRewardDAL, RewardDAL>();
            services.AddTransient<IProfileMatchInfoDAL, ProfileMatchInfoDAL>();
            services.AddTransient<IOperatorDAL, OperatorDAL>();
            services.AddTransient<ISalesManagementDAL, SalesManagementDAL>();
            services.AddTransient<IBillingDAL, BillingDAL>();
            services.AddTransient<ICreateUpdateCampaignDAL, CreateUpdateCampaignDAL>();

            // DAL Query Execution.
            services.AddTransient<IExecutionCommand, ExecutionCommand>();
            return services;
        }
    }
}
