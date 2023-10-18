using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using Newtonsoft.Json;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class UserDashboardService : IUserDashboardService
    {
        private readonly IConfiguration _configuration;
        IHttpContextAccessor _httpAccessor;
        private readonly IUserDashboardDAL _dashboardDal;
        private readonly ILoggingService _logServ;
        private readonly IConnectionStringService _conService;
        const string PageName = "UserDashboardService";


        ReturnResult result = new ReturnResult();


        public UserDashboardService(IConfiguration configuration, IHttpContextAccessor httpAccessor, IUserDashboardDAL dashboardDal,
                                    ILoggingService logServ, IConnectionStringService conService)
        {
            _configuration = configuration;
            _httpAccessor = httpAccessor;
            _dashboardDal = dashboardDal;
            _logServ = logServ;
            _conService = conService;
        }


        public async Task<ReturnResult> LoadSubscriberDataTable(PagingSearchClass paging)
        {
            var subData = new List<SubscriberDashboardResult>();
            PageSearchModel searchList = null;

            if (paging.search != null && paging.search.Length > 3)
            {
                searchList = JsonConvert.DeserializeObject<PageSearchModel>(paging.search);
            }
            try
            {
                if(searchList != null && searchList.Operator != null)
                {
                    var conn = await _conService.GetConnectionStringByOperator(int.Parse(searchList.Operator));
                    var dataRes = await _dashboardDal.GetSubscriberDashboard(paging, conn);
                    subData = dataRes.ToList();
                }
                else if(searchList != null && searchList.country != null)
                {
                    var conn = await _conService.GetConnectionStringsByCountryId(int.Parse(searchList.country));
                    var dataRes = await _dashboardDal.GetSubscriberDashboard(paging, conn.FirstOrDefault());
                    subData = dataRes.ToList();
                }
                else
                {
                    List<string> conns = await _conService.GetConnectionStrings();
                    foreach (var conn in conns)
                    {
                        var dataRes = await _dashboardDal.GetSubscriberDashboard(paging, conn);
                        subData.AddRange(dataRes);
                    }
                }
                result.recordcount = subData.Count();
                var res = SortSubscriberData(subData, paging);
                result.body = res.Skip(paging.page * paging.pageSize).Take(paging.pageSize);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetTicketListAsync";
                await _logServ.LogError();

                result.result = 0;
            }

            return result;
        }


        private List<SubscriberDashboardResult> SortSubscriberData(List<SubscriberDashboardResult> subData, PagingSearchClass param)
        {
            switch (param.sort)
            {
                case "fullName":
                    if (param.direction.ToLower() == "asc")
                        subData.Sort((u1, u2) => u1.FullName.CompareTo(u2.FullName));
                    else
                        subData.Sort((u1, u2) => u2.FullName.CompareTo(u1.FullName));
                    break;
                case "email":
                    if (param.direction.ToLower() == "asc")
                        subData.Sort((u1, u2) => u1.Email.CompareTo(u2.Email));
                    else
                        subData.Sort((u1, u2) => u2.Email.CompareTo(u1.Email));
                    break;
                case "country":
                    if (param.direction.ToLower() == "asc")
                        subData.Sort((u1, u2) => u1.CountryName.CompareTo(u2.CountryName));
                    else
                        subData.Sort((u1, u2) => u2.CountryName.CompareTo(u1.CountryName));
                    break;
                case "rStatus":
                    if (param.direction.ToLower() == "asc")
                        subData.Sort((u1, u2) => u1.rStatus.CompareTo(u2.rStatus));
                    else
                        subData.Sort((u1, u2) => u2.rStatus.CompareTo(u1.rStatus));
                    break;
                case "createdDate":
                    if (param.direction.ToLower() == "asc")
                        subData.Sort((u1, u2) => u1.DateCreated.CompareTo(u2.DateCreated));
                    else
                        subData.Sort((u1, u2) => u2.DateCreated.CompareTo(u1.DateCreated));
                    break;
                default:
                    subData.Sort((u1, u2) => u2.DateCreated.CompareTo(u1.DateCreated));
                    break;
            }
            return subData;
        }

        

    }
}
