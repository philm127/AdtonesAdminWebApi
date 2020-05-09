
using AdtonesAdminWebApi.BusinessServices.Interfaces;
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
    public class CampaignService : ICampaignService
    {
        private readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();


        public CampaignService(IConfiguration configuration)

        {
            _configuration = configuration;
        }


        /// <summary>
        /// Populate the datatable
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> LoadCampaignDataTable()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<CampaignAdminResult>(GetCampaignResultSet());

                }
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
        

#region Longer SQL Query


        private string GetCampaignResultSet()
        {
            return @"SELECT u.Email,CONCAT(u.FirstName, ' ',u.LastName) AS FullName,
                    ISNULL(cl.Name,'-') AS ClientName,camp.CampaignName,
                    (SELECT TOP 1 ad.AdvertName FROM Advert AS ad WHERE ad.CampProfileId=camp.CampaignProfileId) AS AdvertName,
                    cp.finaltotalplays,
                    (SELECT SUM(ISNULL(TotalBudget,0)) FROM CampaignProfile WHERE CampaignProfileId=camp.CampaignProfileId 
                        GROUP BY CampaignProfileId) AS TotalBudget,
                    cp.TotalSpend,
                        (
	                    (SELECT SUM(ISNULL(TotalBudget,0)) 
		                    FROM CampaignProfile 
		                    WHERE CampaignProfileId=camp.CampaignProfileId 
		                    GROUP BY CampaignProfileId
                     /* For hard of vision is a minus sign below */
	                    ) - cp.TotalSpend) AS FundsAvailable, cp.ABidValue, camp.CreatedDateTime,
                    (SELECT TOP 1 AdvertId FROM Advert AS ad WHERE ad.CampProfileId=camp.CampaignProfileId) AS AdvertId,
                    (SELECT TOP 1 OperatorId FROM Advert WHERE CampProfileId=camp.CampaignProfileId) AS OperatorId,
                    (SELECT COUNT(Id) FROM Question WHERE CampaignProfileId=camp.CampaignProfileId GROUP BY CampaignProfileId) AS TicketCount,
                    (CASE 
	                    WHEN 
		                    ISNULL((SELECT COUNT(CampaignProfileId) FROM Billing WHERE CampaignProfileId=camp.CampaignProfileId 
                            GROUP BY CampaignProfileId ),0)=0 
	                    THEN 8 
                        ELSE camp.Status END) AS Status,
	                camp.CampaignProfileId,camp.UserId,camp.IsAdminApproval,camp.CountryId
                    FROM CampaignProfile AS camp INNER JOIN Users AS u ON camp.UserId=u.UserId
                    LEFT JOIN Client AS cl ON camp.ClientId=cl.Id
                    LEFT JOIN
                        (SELECT CampaignProfileId,COUNT(CampaignProfileId) AS finaltotalplays,
	                    CAST(AVG(ISNULL(BidValue,0)) AS decimal(18,4)) AS ABidValue,
	                    CAST((Sum(ISNULL(BidValue,0)) + Sum(ISNULL(SMSCost,0)) + Sum(ISNULL(EmailCost,0))) AS decimal(18,4)) AS TotalSpend,
	                    SUM(ISNULL(SMSCost,0)) AS SMSCost,Sum(ISNULL(EmailCost,0)) AS EmailCost
	                    FROM CampaignAudit
	                    WHERE LOWER(Status)='played' AND PlayLengthTicks > 6000 
	                    GROUP BY CampaignProfileId
	                    HAVING COUNT(CampaignProfileId)>=1) AS cp
                    ON cp.CampaignProfileId=camp.CampaignProfileId
                    ORDER BY camp.CreatedDateTime DESC;";
        }


        


        #endregion
    }
}
