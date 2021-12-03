using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.Services.Mailer;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.Command;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class AdvertiserFinancialService : IAdvertiserFinancialService
    {
        private readonly IConfiguration _configuration;
        
        private readonly ISharedSelectListsDAL _sharedDal;
        private readonly IAdvertiserFinancialDAL _invDAL;
        private IWebHostEnvironment _env;
        private readonly ISendEmailMailer _mailer;
        private readonly ISaveGetFiles _getFiles;
        private readonly ILoggingService _logServ;
        IHttpContextAccessor _httpAccessor;
        ReturnResult result = new ReturnResult();
        const string PageName = "AdvertiserPaymentService";

        public AdvertiserFinancialService(IConfiguration configuration, ISharedSelectListsDAL sharedDal, IAdvertiserFinancialDAL invDAL,
                                           IWebHostEnvironment env, ISaveGetFiles getFiles, ISendEmailMailer mailer, ILoggingService logServ,
                                           IHttpContextAccessor httpAccessor)

        {
            _configuration = configuration;
            _sharedDal = sharedDal;
            _invDAL = invDAL;
            _env = env;
            _mailer = mailer;
            _getFiles = getFiles;
            _logServ = logServ;
            _httpAccessor = httpAccessor;
        }


    }
}
