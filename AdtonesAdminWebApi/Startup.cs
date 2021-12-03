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
using AdtonesAdminWebApi.DAL;
using AdtonesAdminWebApi.UserMatchServices;
using AdtonesAdminWebApi.Services.Mailer;

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
            
            services.AddControllers().AddNewtonsoftJson();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(Startup));
            services.AddBusinessServices();
            services.AddDAL();

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
            services.AddTransient<ISageCreditCardPaymentService, SageCreditCardPaymentService>();
            services.AddTransient<ILoggingService, LoggingService>();
            services.AddScoped<ICreateInvoicePDF, CreateInvoicePDF>();
            //services.AddTransient<IStatsProvider, StatsProvider>();

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
