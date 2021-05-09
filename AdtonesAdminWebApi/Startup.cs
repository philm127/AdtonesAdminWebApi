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
using AdtonesAdminWebApi.DAL;
using AdtonesAdminWebApi.UserMatchServices;
using AdtonesAdminWebApi.Services.Mailer;
using System.Collections.Generic;
using AutoMapper;
//using AdtonesAdminWebApi.CRM.Repositories;
//using AdtonesAdminWebApi.CRM.Business;
//using AdtonesAdminWebApi.CRM.Models.Subscriber;
//using AdtonesAdminWebApi.CrmApp.Application;
//using AdtonesAdminWebApi.CrmApp.Data;
//using AdtonesAdminWebApi.CrmApp.Data.Users;

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
            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddMemoryCache();
            //services.AddApplication();
            //services.AddData();
            //services.AddDataUsers();

            services.AddControllers().AddNewtonsoftJson();
            services.AddHttpContextAccessor();
            //// Auto Mapper Configurations
            //var mapperConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new SubscriberProfile());
            //});

            //IMapper mapper = mapperConfig.CreateMapper();
            //services.AddSingleton(mapper);
            services.AddAutoMapper(typeof(Startup));
            // services.AddAutoMapper(typeof(SubscriberProfile));

            #region Business Services

            // Business Services
            services.AddScoped<ISharedSelectListsService, SharedSelectListsService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            // services.AddScoped<IAdvertiserCreditService, AdvertiserCreditService>();
            services.AddScoped<IPromotionalCampaignService, PromotionalCampaignService>();
            services.AddScoped<ISalesManagementService, SalesManagementService>();
            services.AddScoped<IProfileMatchInfoService, ProfileMatchInfoService>();
            services.AddScoped<ICountryAreaService, CountryAreaService>();
            services.AddScoped<IFinanceTablesService, FinanceTablesService>();
            services.AddScoped<IAdvertiserFinancialService, AdvertiserFinancialService>();
            services.AddScoped<IUserDashboardService, UserDashboardService>();
            services.AddScoped<IOperatorConfigService, OperatorConfigService>();
            services.AddScoped<IOperatorService, OperatorService>();
            services.AddScoped<ISystemConfigService, SystemConfigService>();
            services.AddScoped<IRewardsService, RewardsService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IAdvertService, AdvertService>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<ICampaignAuditService, CampaignAuditService>();
            services.AddScoped<IPermissionManagementService, PermisionManagementService>();
            services.AddScoped<IManagementReportService, ManagementReportService>();
            services.AddScoped<ICreateUpdateCampaignService, CreateUpdateCampaignService>();
            services.AddScoped<ICreateCheckSaveProfileModels, CreateCheckSaveProfileModels>();
            services.AddScoped<IBillingService, BillingService>();
            //services.AddScoped<ISubscriberService, SubscriberService>();

            #endregion

            #region DAL Access

            // Use DAL. 
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
            services.AddScoped<IProfileMatchInfoDAL,ProfileMatchInfoDAL>();
            services.AddScoped<IOperatorDAL, OperatorDAL>();
            services.AddScoped<ISalesManagementDAL, SalesManagementDAL>();
            services.AddScoped<IBillingDAL, BillingDAL>();
            services.AddScoped<ICreateUpdateCampaignDAL, CreateUpdateCampaignDAL>();
            //services.AddScoped<ISubscriberRepositorry, SubscriberRepositorry>();

            #endregion


            // DAL Query Execution.
            services.AddTransient<IExecutionCommand, ExecutionCommand>();

            #region Special Services

            services.AddScoped<ISaveGetFiles, SaveGetFiles>();
            services.AddTransient<IUserMatchInterface, UserMatchTableProcess>();
            services.AddTransient<IAdTransferService, AdTransferService>();
            services.AddTransient<IGenerateTicketService, GenerateTicketService>();
            services.AddTransient<ILiveAgentService, LiveAgentService>();
            services.AddTransient<ISoapApiService, SoapApiService>();
            services.AddTransient<ISendEmailMailer, SendEmailMailer>();
            services.AddTransient<IPrematchProcess, PrematchProcess>();
            services.AddTransient<IConvertSaveMediaFile, ConvertSaveMediaFile>();
            services.AddScoped<ICurrencyConversion, CurrencyConversion> ();
            services.AddTransient<IAdvertEmail, AdvertEmail>();
            services.AddTransient<ILoggingService, LoggingService>();
            services.AddScoped<ICreateInvoicePDF, CreateInvoicePDF>();

            #endregion

            // Client Specific Services
            services.AddScoped<IExpressoProcessPromoUser, ExpressoProcessPromoUser>();
            services.AddScoped<ISafaricomProcessPromoUser, SafaricomProcessPromoUser>();
            services.AddScoped<ISavePUToDatabase, SavePUToDatabase>();


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

            // REST Headers
            app.UseCors("CorsPolicy");
            // app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
