using AdtonesAdminWebApi.CrmApp.Application.Interfaces;
using AdtonesAdminWebApi.CrmApp.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AdtonesAdminWebApi.CrmApp.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}