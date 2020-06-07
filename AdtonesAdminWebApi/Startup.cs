using System;
using AdtonesAdminWebApi.BusinessServices;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AdtonesAdminWebApi.Services;

using AdtonesAdminWebApi.OperatorSpecific;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Shared;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.DAL;
using AdtonesAdminWebApi.UserMatchServices;

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
            services.AddHttpContextAccessor();

            #region Business Services

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
            services.AddScoped<IOperatorService, OperatorService>();
            services.AddScoped<ISystemConfigService, SystemConfigService>();
            services.AddScoped<IRewardsService, RewardsService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IAdvertService, AdvertService>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IUserProfileService, UserProfileService>();

            #endregion

            #region DAL Access

            // Use DAL. 
            services.AddScoped<IConnectionStringService, ConnectionStringService>();
            services.AddScoped<ISharedSelectListsDAL, SharedSelectListsDAL>();
            services.AddScoped<ITicketDAL, TicketDAL>();
            services.AddScoped<IAdvertDAL, AdvertDAL>();
            services.AddScoped<IAreaDAL, AreaDAL>();
            services.AddScoped<ICampaignDAL, CampaignDAL>();
            services.AddScoped<ICheckExistsDAL, CheckExistsDAL>();
            services.AddScoped<IUserDashboardDAL, UserDashboardDAL>();
            services.AddScoped<IProvisionServerDAL, ProvisionServerDAL>();
            services.AddScoped<ILoginDAL, LoginDAL>();
            services.AddScoped<IUserManagementDAL, UserManagementDAL>();
            services.AddScoped<IUserMatchDAL, UserMatchDAL>();
            services.AddScoped<IUserProfileDAL, UserProfileDAL>();

            #endregion


            // DAL Query Execution.
            services.AddTransient<IExecutionCommand, ExecutionCommand>();

            #region SQL Queries

            // DAL Queries.
            services.AddTransient<ISharedListQuery, SharedListQuery>();
            services.AddTransient<ITicketQuery, TicketQuery>();
            services.AddTransient<IUserDashboardQuery, UserDashboardQuery>();
            services.AddTransient<IAdvertQuery, AdvertQuery>();
            services.AddTransient<ICampaignQuery, CampaignQuery>();
            services.AddTransient<IAreaQuery, AreaQuery>();
            services.AddTransient<ICheckExistsQuery, CheckExistsQuery>();
            services.AddTransient<IProvisionServerQuery, ProvisionServerQuery>();
            services.AddTransient<ILoginQuery, LoginQuery>();
            services.AddTransient<IUserManagementQuery, UserManagementQuery>();
            services.AddTransient<IUserMatchQuery, UserMatchQuery>();
            services.AddTransient<IUserProfileQuery, UserProfileQuery>();

            #endregion


            // Special Services
            services.AddScoped<ISaveGetFiles, SaveGetFiles>();
            services.AddTransient<IUserMatchInterface, UserMatchTableProcess>();
            services.AddTransient<IAdTransferService, AdTransferService>();
            services.AddTransient<IGenerateTicketService, GenerateTicketService>();
            services.AddTransient<ILiveAgentService, LiveAgentService>();
            services.AddTransient<ISoapApiService, SoapApiService>();

            // Client Specific Services
            services.AddScoped<IExpressoProcessPromoUser, ExpressoProcessPromoUser>();
            services.AddScoped<ISafaricomProcessPromoUser, SafaricomProcessPromoUser>();


            #region Authentication

            ///// Authentication

            services.AddScoped<ILogonService, LogonService>();
            services.AddScoped<IAuthService, AuthService>();

            byte[] secretKey = Convert.FromBase64String(Configuration["JwtConfig:secret"]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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

            #endregion
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();


            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            // REST Headers
            app.UseCors(
                options => options.SetIsOriginAllowed(x => _ = true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
