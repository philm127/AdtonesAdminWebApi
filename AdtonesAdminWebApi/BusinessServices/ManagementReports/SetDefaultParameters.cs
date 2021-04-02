using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.ManagementReports
{
    public interface ISetDefaultParameters
    {
        ManagementReportsSearch SetDefaults(ManagementReportsSearch search);
    }


    public class SetDefaultParameters : ISetDefaultParameters
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly ICurrencyDAL _curDAL;

        public SetDefaultParameters(IHttpContextAccessor httpAccessor, ICurrencyDAL curDAL)
        {
            _httpAccessor = httpAccessor;
            _curDAL = curDAL;
        }


        public ManagementReportsSearch SetDefaults(ManagementReportsSearch search)
        {
            // Currently hardcoded in SetDefaultParameters to the 2 available
            List<int> ops = new int[] { 1, 2 }.ToList(); // _reportDAL.GetAllOperators().Result.ToList();

            var currency = _curDAL.GetDisplayCurrencyCodeForUserAsync(_httpAccessor.GetUserIdFromJWT()).Result;
            System.Globalization.CultureInfo enGB = new System.Globalization.CultureInfo("en-GB");
            var today = DateTime.Today;
            var tomorrow = today.AddDays(2);
            var old = today.AddDays(-5000);
            TimeSpan ts = new TimeSpan(0, 0, 0);


            if (search.DateTo == null || search.DateTo < old || search.DateTo < search.DateFrom)

                search.ToDate = tomorrow.Date.ToString("yyyy-MM-dd HH:mm:ss");

            else
            {
                DateTime iniDate = search.DateTo.Value;// DateTime.ParseExact(search.DateTo, format, CultureInfo.InvariantCulture);
                var toDate = iniDate.AddDays(1);
                search.ToDate = toDate.ToString("yyyy-MM-dd 00:00:00");
            }

            if (search.DateFrom == null || search.DateFrom < old || search.DateFrom > search.DateTo)
                search.FromDate = old.Date.ToString("yyyy-MM-dd HH:mm:ss");

            else
                search.FromDate = search.DateFrom.Value.ToString("yyyy-MM-dd 00:00:00");

            // Only included known operators for now
            if (search.operators == null || search.operators.Length == 0)
            {
                search.operators = ops.ToArray();
            }

            if (search.currency == null || search.currency == 0)
            {
                search.currency = currency.CurrencyId;
            }

            return search;
        }

    }
}
