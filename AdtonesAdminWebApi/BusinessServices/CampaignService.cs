
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;


namespace AdtonesAdminWebApi.BusinessServices
{
    public class CampaignService : ICampaignService
    {
        private readonly IConfiguration _configuration;

        public IConnectionStringService _connService { get; }
        // private readonly ISaveFiles _saveFile;
        ReturnResult result = new ReturnResult();

        IHttpContextAccessor _httpAccessor;
        private readonly ICampaignDAL _campDAL;

        public CampaignService(IConfiguration configuration, IConnectionStringService connService, IHttpContextAccessor httpAccessor,
                                ICampaignDAL campDAL) //ISaveFiles saveFile)

        {
            _configuration = configuration;
            _connService = connService;
           // _saveFile = saveFile;
            _httpAccessor = httpAccessor;
            _campDAL = campDAL;
        }


        /// <summary>
        /// Populate the datatable
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> LoadCampaignDataTable(int id=0)                  
        {
            try
            {
                    result.body = await _campDAL.GetCampaignResultSet(id);
                //    var provModel = await _campDAL.GetCampaignResultSetProv(id);

                //PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(CampaignAdminResult));
                //DataTable dt = new DataTable();
                //foreach (PropertyDescriptor p in props)
                //    dt.Columns.Add(p.Name, p.PropertyType);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignService",
                    ProcedureName = "LoadCampaignDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadCampaignDataTableSalesExec(int id = 0)
        {
            try
            {
                result.body = await _campDAL.GetCampaignResultSetBySalesExec(id);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignService",
                    ProcedureName = "LoadCampaignDataTableSalesExec"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadCampaignDataTableById(int id)
        {
            try
            {
                result.body = await _campDAL.GetCampaignResultSetById(id);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignService",
                    ProcedureName = "LoadCampaignDataTableById"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        

        /// <summary>
        /// Changed for the actual campaign directly
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReturnResult> UpdateCampaignStatus(IdCollectionViewModel model)
        {
            try
            {
                // Need to do this to get OperatorId
                CampaignProfile _campProfile = await _campDAL.GetCampaignProfileDetail(model.id);
                bool exists = false;

                exists = await _campDAL.CheckCampaignBillingExists(model.id);

                if (!exists)
                    _campProfile.Status = (int)Enums.CampaignStatus.InsufficientFunds;
                else
                    _campProfile.Status = model.status;

                result.body = await _campDAL.ChangeCampaignProfileStatus(_campProfile);
                var x = await _campDAL.ChangeCampaignProfileStatusOperator(_campProfile);
                var y = await _campDAL.UpdateCampaignMatch(_campProfile);


            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignService",
                    ProcedureName = "UpdateStatus"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Changed when advert status changed, called by Adverts changed status
        /// </summary>
        /// <param name="campaignId"></param>
        /// <returns>True or False</returns>
        public async Task<bool> ChangeCampaignStatus(int campaignId)
        {
            try
            {
                CampaignProfile _campProfile = await _campDAL.GetCampaignProfileDetail(campaignId);

                if (_campProfile != null)
                {
                    bool exists = false;

                    exists = await _campDAL.CheckCampaignBillingExists(campaignId);
                    var roleId = _httpAccessor.GetRoleIdFromJWT();
                    if (exists || roleId == (int)Enums.UserRole.ProfileAdmin)
                    {

                        if (_campProfile.StartDate == null && _campProfile.EndDate == null)
                        {
                            _campProfile.Status = (int)Enums.CampaignStatus.Play;
                        }
                        else
                        {
                            if (_campProfile.StartDate != null)
                            {
                                if (_campProfile.StartDate == DateTime.Now.Date)
                                {
                                    _campProfile.Status = (int)Enums.CampaignStatus.Play;
                                }
                                else
                                {
                                    _campProfile.Status = (int)Enums.CampaignStatus.Planned;
                                }
                            }
                            else
                            {
                                _campProfile.Status = (int)Enums.CampaignStatus.Planned;
                            }
                        }
                    }
                    else
                    {
                        _campProfile.Status = (int)Enums.CampaignStatus.InsufficientFunds;
                    }

                    var y = await _campDAL.ChangeCampaignProfileStatus( _campProfile);
                    var x = await _campDAL.ChangeCampaignProfileStatusOperator( _campProfile);

                    return true;
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignService",
                    ProcedureName = "ChangeCampaignStatus"
                };
                _logging.LogError();
                return false;
            }
            return true;
        }


        public async Task<ReturnResult> AddCampaignCategory(CampaignCategoryResult model)
        {
            try
            {
                result.body = await _campDAL.InsertCampaignCategory(model);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertService",
                    ProcedureName = "AddAdvertCategory"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }



    }
}
