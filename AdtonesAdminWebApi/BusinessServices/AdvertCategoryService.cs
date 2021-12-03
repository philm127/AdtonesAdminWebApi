using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class AdvertCategoryService : IAdvertCategoryService
    {
        private readonly ILoggingService _logServ;
        private readonly IAdvertCategoryDAL _advertDAL;
        ReturnResult result = new ReturnResult();
        const string PageName = "AdvertCategoryService";

        public AdvertCategoryService(ILoggingService logServ, IAdvertCategoryDAL advertDAL)
        {
            _logServ = logServ;
            _advertDAL = advertDAL;
        }

        public async Task<ReturnResult> GetAdvertCategoryDataTable()
        {
            try
            {
                result.body = await _advertDAL.GetAdvertCategoryList();
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertCategoryDataTable";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }


        public async Task<ReturnResult> DeleteAdvertCategory(IdCollectionViewModel model)
        {
            try
            {
                result.body = await _advertDAL.RemoveAdvertCategory(model);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "DeleteAdvertCategory";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }


        public async Task<ReturnResult> UpdateAdvertCategory(AdvertCategoryResult model)
        {
            try
            {
                result.body = await _advertDAL.UpdateAdvertCategory(model);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateAdvertCategory";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }


        public async Task<ReturnResult> GetAdvertCategoryDetails(int id)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertCategoryDetails(id);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertCategoryDetails";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }


        public async Task<ReturnResult> AddAdvertCategory(AdvertCategoryResult model)
        {
            try
            {
                result.body = await _advertDAL.InsertAdvertCategory(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddAdvertCategory";
                await _logServ.LogError();

            }
            return result;
        }

    }
}
