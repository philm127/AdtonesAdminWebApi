using System;
using AdtonesAdminWebApi.BusinessServices;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AdtonesAdminWebApi.Services;

using AdtonesAdminWebApi.OperatorSpecific;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Shared;
using AdtonesAdminWebApi.DAL;

namespace AdtonesAdminWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddControllers().AddNewtonsoftJson();

            // Business Services
            services.AddScoped<ISharedSelectListsService, SharedSelectListsService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IUsersCreditService, UsersCreditService>();
            services.AddScoped<IPromotionalUsersService, PromotionalUsersService>();
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<IProfileMatchInfoService, ProfileMatchInfoService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IUserPaymentService, UserPaymentService>();
            services.AddScoped<IUserDashboardService, UserDashboardService>();
            services.AddScoped<IOperatorConfigService, OperatorConfigService>();

            // Use DAL 
            services.AddScoped<ISharedSelectListsDAL, SharedSelectListsDAL>();
            services.AddScoped<IConnectionStringService, ConnectionStringService>();

            // Special Services
            services.AddScoped<ISaveFiles, SaveFiles>();

            // Client Specific Services
            services.AddScoped<IExpresso, Expresso>();
            services.AddScoped<ISafaricom, Safaricom>();

            /////
            ///// Authentication
            /////
            services.AddScoped<ILogonService, LogonService>();
            services.AddScoped<IAuthService, AuthService>();

            byte[] secretKey = Convert.FromBase64String(Configuration["JwtConfig:secret"]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
