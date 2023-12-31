﻿using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class AdvertiserCreditService : IAdvertiserCreditService
    {

        private readonly IConfiguration _configuration;
        private readonly ISharedSelectListsDAL _sharedDal;
        private readonly IAdvertiserCreditDAL _userDAL;

        ReturnResult result = new ReturnResult();

        public AdvertiserCreditService(IConfiguration configuration, ISharedSelectListsDAL sharedDal,IAdvertiserCreditDAL userDAL)

        {
            _configuration = configuration;
            _sharedDal = sharedDal;
            _userDAL = userDAL;
        }



        /// <summary>
        /// Used for both Adding New Credit and Updating Existing Credit
        /// </summary>
        /// <param name="_usercredit"></param>
        /// <returns></returns>
        public async Task<ReturnResult> AddCredit(AdvertiserCreditFormModel _usercredit)
        {
            try
            {
                int x = 0;
                var creditDetails = await _userDAL.GetUserCreditDetail(_usercredit.UserId);

                if (creditDetails != null)
                {
                    decimal newCredCalc = await CalculateNewCredit(_usercredit.UserId);
                    _usercredit.Id = creditDetails.Id;
                    _usercredit.AssignCredit = _usercredit.AssignCredit;
                    _usercredit.AvailableCredit = _usercredit.AssignCredit + newCredCalc;
                    x = await _userDAL.UpdateUserCredit(_usercredit);
                }
                else
                {
                    x = await _userDAL.AddUserCredit(_usercredit);
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertisersCreditService",
                    ProcedureName = "AddCredit"
                };
                _logging.LogError();
                result.result = 0;
                result.error = "Credit was not added successfully";
                return result;
            }
            result.body = "Assigned credit successfully";
            return result;
        }


        /// <summary>
        /// Populates the User Credit Details Screen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReturnResult> GetCreditDetails(int id)
        {
            string select_query = string.Empty;
            int Id = 0;

            try
            {
                var creddet = await _userDAL.GetUserCreditDetail(id);
                var credhist = await _userDAL.GetUserCreditPaymentHistory(id);
                creddet.PaymentHistory = credhist.ToList();

                result.body = creddet;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertisersCreditService",
                    ProcedureName = "CreditDetails"
                };
                _logging.LogError();
                result.result = 0;
                return result;
            }
            return result;
        }


        ///// <summary>
        ///// Updates FROM the User Credit Details screen
        ///// </summary>
        ///// <param name="_creditmodel"></param>
        ///// <returns></returns>
        //public async Task<ReturnResult> UpdateCredit(UsersCreditFormModel _creditmodel)
        //{
        //    try
        //    {
        //        _creditmodel.AvailableCredit = await CalculateNewCredit(_creditmodel.UserId);

        //        var update_query = @"UPDATE UsersCredit SET UserId = @UserId,AssignCredit = @AssignCredit,AvailableCredit = @AvailableCredit,
        //                                                                                 UpdatedDate = GETDATE(),CurrencyId = @CurrencyId
        //                                                                                 WHERE Id = @Id";


        //        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //        {
        //            await connection.OpenAsync();
        //            result.body = await connection.ExecuteAsync(update_query, _creditmodel);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var _logging = new ErrorLogging()
        //        {
        //            ErrorMessage = ex.Message.ToString(),
        //            StackTrace = ex.StackTrace.ToString(),
        //            PageName = "UserCreditService",
        //            ProcedureName = "UpdateCredit"
        //        };
        //        _logging.LogError();
        //        result.result = 0;
        //    }
        //    return result;

        //}


        public async Task<ReturnResult> UpdateCampaignCredit(CampaignCreditResult model)
        {
            try
            {
                // Need to do this to get OperatorId
                result.body = await _userDAL.UpdateCampaignCredit(model);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertiserPaymentService",
                    ProcedureName = "UpdateCampaignCredit"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddCampaignCredit(CampaignCreditResult model)
        {
            try
            {
                // Need to do this to get OperatorId
                result.body = await _userDAL.InsertCampaignCredit(model);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertiserPaymentService",
                    ProcedureName = "UpdateCampaignCredit"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }



        /// <summary>
        /// Calculates the available credit from the User Credit Details screen
        /// </summary>
        /// <param name="id">Passed the UserId</param>
        /// <returns>The available credit</returns>
        private async Task<decimal> CalculateNewCredit(int userId)
        {
            decimal available = 0.0000M;
            
            try
            {
                available = await _userDAL.GetCreditBalance(userId);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertisersCreditService",
                    ProcedureName = "CalculateNewCredit"
                };
                _logging.LogError();
                throw;
            }
            
            return available;
        }



        #region Not sure about wait on design decisiones TODO:


        //public async Task<ReturnResult> UserCreditpayment(IdCollectionViewModel model)
        //{
        //    string select_query = @"SELECT Id,UserId,BillingId,Amount,Description,Status,CreatedDate,CampaignProfileId
        //                            FROM UsersCreditPayment WHERE UserTd=@UserId";
        //    try
        //    {
        //        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //        {
        //            await connection.OpenAsync();
        //            result.body = await connection.QueryAsync(select_query, new { UserId = model.userId });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var _logging = new ErrorLogging()
        //        {
        //            ErrorMessage = ex.Message.ToString(),
        //            StackTrace = ex.StackTrace.ToString(),
        //            PageName = "AdvertisersCreditService",
        //            ProcedureName = "UserCreditPayment"
        //        };
        //        _logging.LogError();
        //        result.result = 0;
        //        return result;
        //    }
        //    return result;
        //}



        ///// <summary>
        ///// This populates the initial user list on datatable page,
        ///// don't think will need.
        ///// </summary>
        ///// <returns></returns>
        //public async Task<ReturnResult> GetCreditDetailsUsersList()
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //        {
        //            connection.Open();
        //            result.body = await connection.QueryAsync<SharedSelectListViewModel>(@"SELECT u.UserId AS Value,
        //                                                                  CONCAT(u.FirstName,' ',u.LastName) AS Text FROM Users AS u
        //                                                                  INNER JOIN UsersCredit AS c ON c.UserId=u.UserId");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var _logging = new ErrorLogging()
        //        {
        //            ErrorMessage = ex.Message.ToString(),
        //            StackTrace = ex.StackTrace.ToString(),
        //            PageName = "AdvertisersCreditService",
        //            ProcedureName = "GetCreditDetailsUsersList"
        //        };
        //        _logging.LogError();
        //        result.result = 0;
        //    }
        //    return result;
        //}


        #endregion

    }
}
