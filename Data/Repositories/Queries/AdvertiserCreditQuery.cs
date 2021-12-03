using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories.Queries
{
    
    public static class AdvertiserCreditQuery
    {

        public static string CheckIfUserExists => @"SELECT COUNT(1) FROM UsersCredit WHERE UserId=@userId";

        public static string UpdateUserCredit => @"UPDATE UsersCredit SET AssignCredit=@AssignCredit,AvailableCredit=@AvailableCredit,
                                                UpdatedDate=GETDATE()
                                                WHERE Id = @Id";


        public static string AddUserCredit => @"INSERT INTO UsersCredit(UserId,AssignCredit,AvailableCredit,UpdatedDate,CreatedDate,CurrencyId) 
                                            VALUES(@UserId,@AssignCredit,@AssignCredit,GETDATE(),GETDATE(),@CurrencyId)";


        public static string UserCreditDetails => @"SELECT uc.Id,uc.UserId,AssignCredit,AvailableCredit,uc.CreatedDate,uc.CurrencyId,c.CountryId 
                                                FROM  UsersCredit AS uc LEFT JOIN Currencies AS c ON c.CurrencyId=uc.CurrencyId
                                                WHERE uc.UserId=@Id";

        public static string GetPaymentHistory => @"SELECT ucp.Id, ucp.Amount, ucp.CreatedDate, bil.InvoiceNumber 
                                                    FROM UsersCreditPayment AS ucp  LEFT JOIN Billing AS bil ON bil.Id=ucp.BillingId 
                                                    WHERE ucp.UserId=@userid ORDER BY CreatedDate Desc";


        public static string GetTotalPaymentsByUser => @"SELECT sum(Amount) FROM UsersCreditPayment WHERE UserId=@UserId GROUP BY UserId";


        public static string GetTotalBilledByUser => @"SELECT sum(FundAmount) FROM Billing WHERE PaymentMethodId=1 AND UserId=@UserId GROUP BY UserId";


        public static string UpdateCampaignCredit => @"UPDATE CampaignCreditPeriods SET CreditPeriod=@CreditPeriod WHERE ";


        public static string InsertCampaignCredit => @"INSERT INTO CampaignCreditPeriods(CreditPeriod,UserId,CampaignProfileId,UpdatedDate,CreatedDate,AdtoneServerCampaignCreditPeriodId)
                                                                VALUES(@CreditPeriod,@UserId,@CampaignProfileId, GETDATE(),GETDATE(),@AdtoneServerCampaignCreditPeriodId);
                                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

    }

}
