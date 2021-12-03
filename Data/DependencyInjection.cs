using BusinessServices.Interfaces.Repository;
using Data.Repositories;
//using Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataService(this IServiceCollection services)
        {
            services.AddTransient<ICampaignMatchesRepository, CampaignMatchesRepository>();
            services.AddTransient<IConnectionStringService, ConnectionStringService>();

            services.AddScoped<IConnectionStringService, ConnectionStringService>();
            services.AddScoped<ISharedSelectListsDAL, SharedSelectListsDAL>();
            services.AddScoped<ITicketDAL, TicketDAL>();
            services.AddScoped<IAdvertDAL, AdvertDAL>();
            services.AddScoped<ICountryAreaDAL, CountryAreaDAL>();
            services.AddScoped<ICampaignDAL, CampaignDAL>();
            services.AddScoped<IUserDashboardDAL, UserDashboardDAL>();
            services.AddScoped<IPromotionalCampaignDAL, PromotionalCampaignDAL>();
            services.AddScoped<ILoginDAL, LoginDAL>();
            services.AddScoped<IUserManagementDAL, UserManagementDAL>();
            services.AddScoped<IUserMatchDAL, UserMatchDAL>();
            // services.AddScoped<IUserProfileDAL, UserProfileDAL>();
            services.AddScoped<ISoapDAL, SoapDAL>();
            services.AddScoped<ICampaignAuditDAL, CampaignAuditDAL>();
            services.AddScoped<ICurrencyDAL, CurrencyDAL>();
            // services.AddScoped<IAdvertiserCreditDAL, AdvertiserCreditDAL>();
            services.AddScoped<IAdvertiserFinancialDAL, AdvertiserFinancialDAL>();
            services.AddScoped<IPermissionManagementDAL, PermissionManagementDAL>();
            services.AddScoped<IManagementReportDAL, ManagementReportDAL>();
            services.AddScoped<IFinanceTablesDAL, FinanceTablesDAL>();
            services.AddScoped<IRewardDAL, RewardDAL>();
            services.AddScoped<IProfileMatchInfoDAL, ProfileMatchInfoDAL>();
            services.AddScoped<IOperatorDAL, OperatorDAL>();
            services.AddScoped<ISalesManagementDAL, SalesManagementDAL>();
            services.AddScoped<IBillingDAL, BillingDAL>();
            services.AddScoped<ICreateUpdateCampaignDAL, CreateUpdateCampaignDAL>();

            return services;
        }
    }
}
