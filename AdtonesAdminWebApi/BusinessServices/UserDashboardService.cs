using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class UserDashboardService : IUserDashboardService
    {
        private readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();


        public UserDashboardService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task<ReturnResult> LoadAdvertiserDataTable()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<AdvertiserDashboardResult>(AdvertiserResultQuery());
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserDashboardService",
                    ProcedureName = "LoadAdvertiserDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadOperatorDataTable()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<OperatorDashboardResult>(OperatorResultQuery());
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserDashboardService",
                    ProcedureName = "LoadOperatorDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        #region Long SQL Queries


        private string AdvertiserResultQuery()
        {
            return @"SELECT item.UserId,item.RoleId,item.Email,item.FirstName,item.LastName,
                          ISNULL(camp.NoOfactivecampaign, 0) AS NoOfactivecampaign,
                           ISNULL(ad.NoOfunapprovedadverts, 0) AS NoOfunapprovedadverts,
                           ISNULL(cred.AssignCredit, 0) AS creditlimit,ISNULL(billit.outStandingInvoice, 0) AS outStandingInvoice,
                           item.Activated,item.DateCreated,ISNULL(tkt.TicketCount, 0) AS TicketCount
                           FROM
                                (SELECT item.UserId, item.RoleId, item.Email, item.DateCreated, item.Activated,
                                item.FirstName,item.LastName
                                FROM Users item Where item.VerificationStatus = 1 AND item.RoleId = 3) item
                            LEFT JOIN
                                (SELECT a.[UserId], b.[AssignCredit], a.[Id] FROM
                                (SELECT[UserId], MIN(Id) AS Id FROM UsersCredit GROUP BY[UserId]) a
                                INNER JOIN UsersCredit b ON a.[UserId] = b.[UserId] AND a.Id = b.Id) cred
                            ON item.UserId = cred.UserId
                            LEFT JOIN
                                (SELECT COUNT(UserId)as TicketCount, UserId FROM Question WHERE Status IN (1, 2) GROUP BY UserId) tkt
                            ON item.UserId = tkt.UserId
                            LEFT JOIN
                                (SELECT COUNT(CampaignProfileId) AS NoOfactivecampaign,UserId FROM Campaignprofile WHERE Status IN (4, 3, 2, 1) GROUP BY UserId) camp
                            ON item.UserId = camp.UserId
                            LEFT JOIN
                                (SELECT COUNT(AdvertId) AS NoOfunapprovedadverts,UserId FROM Advert WHERE Status = 4 GROUP BY UserId) ad
                            ON item.UserId = ad.UserId
                            LEFT JOIN
                                (SELECT COUNT(bill3.UserId) AS outStandingInvoice,bill3.UserId
                                FROM
                                    (SELECT SUM(FundAmount) AS totalAmount, CampaignProfileId, UserId
                                    FROM Billing WHERE PaymentMethodId = 1 GROUP BY CampaignProfileId, UserId) bill3
                                LEFT JOIN
                                    (SELECT sum(Amount) AS paidAmount, UserId, CampaignProfileId
                                    FROM UsersCreditPayment GROUP BY CampaignProfileId, UserId) uc
                                ON bill3.UserId = uc.UserId AND bill3.CampaignProfileId = uc.CampaignProfileId
                                WHERE (ISNULL(bill3.totalAmount, 0) - ISNULL(uc.paidAmount, 0)) > 0
                                GROUP BY bill3.UserId) billit
                            ON item.UserId = billit.UserId;";
        }


        private string OperatorResultQuery()
        {
            return @"SELECT u.UserId,FirstName,LastName,Email,ISNULL(Organisation,'-') AS Organisation,u.OperatorId,o.CountryId,
                        c.Name AS CountryName,o.OperatorName,u.Activated,u.DateCreated
                        FROM Users AS u LEFT JOIN Operators AS o ON u.OperatorId=o.OperatorId
                        LEFT JOIN Country AS c ON o.CountryId=c.Id
                        WHERE RoleId=6 ORDER BY u.DateCreated DESC";
        }


        #endregion


    }
}
