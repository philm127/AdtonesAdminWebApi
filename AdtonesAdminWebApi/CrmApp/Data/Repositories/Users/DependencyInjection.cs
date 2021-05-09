using AdtonesAdminWebApi.CrmApp.Application.Interfaces.Users;
using AdtonesAdminWebApi.CrmApp.Data.Repositories.Users;
using Microsoft.Extensions.DependencyInjection;

namespace AdtonesAdminWebApi.CrmApp.Data.Users
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataUsers(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ISubscriberRepository, SubscriberRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}