using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class AdvertiserFinancialDAL : BaseDAL, IAdvertiserFinancialDAL
    {

        public AdvertiserFinancialDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, 
                                        IHttpContextAccessor httpAccessor)
                                        : base(configuration, executers, connService, httpAccessor)
        {}


        


    }
}
