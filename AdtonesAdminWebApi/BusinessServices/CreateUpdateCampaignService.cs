using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class CreateUpdateCampaignService
    {
        private readonly IHttpContextAccessor _httpAccessor;
        ReturnResult result = new ReturnResult();


        public CreateUpdateCampaignService(IHttpContextAccessor httpAccessor)
        {
            _httpAccessor = httpAccessor;
        }


        /// <summary>
        /// Gets initial data to populate the create campaign stepper.
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> GetInitialData(int advertiserId = 0)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertResultSet(id);

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CreateUpdateCampaignService",
                    ProcedureName = "GetInitialData"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }
    }
}
