using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class UsersCreditService : IUsersCreditService
    {

        private readonly IConfiguration _configuration;
        private readonly ISharedSelectListsDAL _sharedDal;

        ReturnResult result = new ReturnResult();

        public UsersCreditService(IConfiguration configuration, ISharedSelectListsDAL sharedDal)

        {
            _configuration = configuration;
            _sharedDal = sharedDal;
        }


        /// <summary>
        /// Populates the datatable
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> LoadDataTable()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<UserCreditResult>(UserResultQuery());
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserCreditService",
                    ProcedureName = "GetUserResult"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// When Add Credit selected this populates dropdown with credit users
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> GetAddCreditUsersList()
        {
            try
            {
                var select_query = @"SELECT UserId AS Value, CONCAT(FirstName,' ',LastName,'(',Email,')') AS Text FROM Users 
                                    WHERE VerificationStatus=1
                                    AND Activated=1 AND RoleId=3 
                                    AND UserId NOT IN(SELECT UserId FROM UsersCredit)";

                result.body = await _sharedDal.GetSelectList(select_query);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UsersCreditService",
                    ProcedureName = "GetAddCreditUsersList"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddCredit(UserCreditFormModel _creditmodel)
        {
            try
            {
                UsersCreditFormModel _usercredit = new UsersCreditFormModel();
                _usercredit.UserId = _creditmodel.UserId;
                _usercredit.AssignCredit = _creditmodel.AssignCredit;
                _usercredit.AvailableCredit = _creditmodel.AssignCredit;
                _usercredit.UpdatedDate = DateTime.Now;
                _usercredit.CurrencyId = _creditmodel.CurrencyId;

                var query = string.Empty;
                if (_creditmodel.Id == 0)
                {
                    query = @"UPDATE UsersCredit SET UserId=@UserId,AssignCredit=@AssignCredit,AvailableCredit=@AssignCredit,
                                UpdatedDate=GETDATE(),CurrencyId=@CurrencyId
                                WHERE Id = @Id";
                }
                else
                {
                    query = @"INSERT INTO UsersCredit(UserId,AssignCredit,AvailableCredit,UpdatedDate,CreatedDate,CurrencyId) 
                        VALUES(@UserId,@AssignCredit,@AssignCredit,GETDATE(),GETDATE(),@CurrencyId)";
                }

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    int x = await connection.ExecuteAsync(query, _usercredit);

                    if (x != 1)
                    {
                        result.result = 0;
                        result.error = "Credit was not added successfully";
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UsersCreditService",
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
        public async Task<ReturnResult> GetCreditDetails(IdCollectionViewModel model)
        {
            string select_query = string.Empty;
            int Id = 0;

            try
            {
                if (model.userId != 0)
                {
                    Id = model.userId;
                    select_query = @"SELECT Id,UserId,AssignCredit,AvailableCredit,CreatedDate,CurrencyId FROM  UsersCredit
                                            WHERE UserId=@Id";
                }
                else
                {
                    Id = model.id;
                    select_query = @"SELECT Id,UserId,AssignCredit,AvailableCredit,CreatedDate,CurrencyId FROM  UsersCredit
                                             WHERE Id=@Id";
                }

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryFirstOrDefaultAsync<UsersCreditFormModel>(select_query, new { Id = Id });
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UsersCreditService",
                    ProcedureName = "CreditDetails"
                };
                _logging.LogError();
                result.result = 0;
                return result;
            }
            return result;
        }


        /// <summary>
        /// Updates FROM the User Credit Details screen
        /// </summary>
        /// <param name="_creditmodel"></param>
        /// <returns></returns>
        public async Task<ReturnResult> UpdateCredit(UsersCreditFormModel _creditmodel)
        {
            try
            {
                _creditmodel.AvailableCredit = await CalculateNewCredit(_creditmodel);

                var update_query = @"UPDATE UsersCredit SET UserId = @UserId,AssignCredit = @AssignCredit,AvailableCredit = @AvailableCredit,
                                                                                         UpdatedDate = GETDATE(),CurrencyId = @CurrencyId
                                                                                         WHERE Id = @Id";


                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.ExecuteAsync(update_query, _creditmodel);
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserCreditService",
                    ProcedureName = "UpdateCredit"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;

        }



        /// <summary>
        /// Calculates the available credit from the User Credit Details screen
        /// </summary>
        /// <param name="model">Passed the UserCreditFormModel from UpdateCredit</param>
        /// <returns>The available credit</returns>
        private async Task<decimal> CalculateNewCredit(UserCreditFormModel model)
        {
            string select_query1 = @"SELECT sum(Amount) FROM UsersCreditPayment WHERE UserId=@UserId GROUP BY UserId";

            string select_query2 = @"SELECT sum(FundAmount) FROM Billing WHERE PaymentMethodId=1 AND UserId=@UserId GROUP BY UserId";
            decimal cred = 0.0000M;
            decimal bil = 0.0000M;
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    cred = await connection.QueryFirstOrDefaultAsync<decimal>(select_query1, new { UserId = model.UserId });
                }

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    bil = await connection.QueryFirstOrDefaultAsync<decimal>(select_query2, new { UserId = model.UserId });
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UsersCreditService",
                    ProcedureName = "CalculateNewCredit"
                };
                _logging.LogError();
                throw;
            }
            var res = (cred + model.AssignCredit) - bil;
            return res;
        }


        #region Long SQL Queries
        private string UserResultQuery()
        {
            return @"SELECT u.Id,u.UserId,usrs.Email,usrs.Name,usrs.Organisation,u.CreatedDate AS CreatedDateSort,
             u.AssignCredit AS Credit,u.AvailableCredit,ISNULL(bil.FundAmount,0) AS TotalUsed,ISNULL(pay.Amount,0) AS TotalPaid,
             (ISNULL(bil.FundAmount,0) - ISNULL(pay.Amount,0)) AS RemainingAmount
             FROM UsersCredit AS u
             LEFT JOIN 
             (SELECT UserId,SUM(FundAmount) AS FundAmount FROM Billing WHERE PaymentMethodId=1 GROUP BY UserId) bil
             ON u.UserId=bil.UserId
             LEFT JOIN
             (SELECT UserId,SUM(Amount) AS Amount from UsersCreditPayment GROUP BY UserId) pay
             ON u.UserId=pay.UserId
             LEFT JOIN
             (SELECT UserId,Email,CONCAT(FirstName,' ',LastName) AS Name,Organisation FROM Users) usrs
             ON usrs.UserId=u.UserId
             ORDER BY u.Id DESC;";
        }

        #endregion

        #region Not sure about wait on design decisiones


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
        //            PageName = "UsersCreditService",
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
        //            PageName = "UsersCreditService",
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
