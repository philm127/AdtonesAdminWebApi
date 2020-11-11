using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class AdvertiserFinancialQuery
    {
        


        public static string GetInvoiceToPDF => @"SELECT pay.Description,bil.InvoiceNumber,bil.SettledDate,co.CountryId,ucp.Amount,usr.Email,
                                                    usr.FirstName, usr.LastName
                                                    FROM Billing AS bil 
                                                    INNER JOIN PaymentMethod AS pay ON bil.PaymentMethodId=pay.Id
                                                    INNER JOIN Users AS usr ON bil.UserId=usr.UserId
                                                    LEFT JOIN CompanyDetails AS co ON usr.UserId=co.UserId
                                                    LEFT JOIN UsersCreditPayment AS ucp ON ucp.UserId=bil.UserId
                                                    WHERE bil.Id=@billingId AND ucp.Id=@ucpId;";


        public static string GetPrePaymentDetails => @"SELECT bil.Id AS billingId,bil.InvoiceNumber,camp.CampaignName,bil.UserId,
                                                        CONCAT(usr.FirstName,' ',usr.LastName) AS FullName,bil.CampaignProfileId
                                                        FROM Billing AS bil 
                                                        INNER JOIN Users AS usr ON bil.UserId=usr.UserId
                                                        INNER JOIN CampaignProfile AS camp ON bil.CampaignProfileId=camp.CampaignProfileId
                                                        LEFT JOIN UsersCreditPayment AS ucp ON ucp.UserId=bil.UserId
                                                        WHERE bil.Id=@billingId;";


        public static string UpdateUserCreditPayment => @"INSERT INTO UsersCreditPayment(UserId,BillingId,Amount,Description,Status,CreatedDate,UpdatedDate,
                                                            CampaignprofileId)
                                                          VALUES(@UserId,@BillingId,@Amount,@Description,@Status,GETDATE(),GETDATE(),
                                                            @CampaignprofileId);";

        public static string UpdateInvoiceSettledDate => @"UPDATE Billing SET SettledDate=GETDATE() WHERE Id=@Id";


        public static string GetOutstandingBalanceCampaign => @"SELECT ISNULL((bilit.TotalAmount - ISNULL(CAST(payit.Amount AS decimal(18,2)),0)),0) AS OutstandingAmount 
                                                        FROM
                                                            (SELECT SUM(ISNULL(bil.TotalAmount,0)) AS TotalAmount,bil.Id
                                                            FROM Billing AS bil
                                                            WHERE PaymentMethodId=1 AND Id=@Id
                                                            GROUP BY bil.CampaignProfileId) bilit
                                                        LEFT JOIN
                                                            (SELECT SUM(ISNULL(ucp.Amount,0)) AS Amount,ucp.BillingId
                                                            FROM UsersCreditPayment ucp
                                                            WHERE BillingId=@Id
                                                            GROUP BY ucp.BillingId) payit
                                                        ON payit.CampaignProfileId=bilit.Id";


        public static string GetOutstandingBalanceInvoice => @"SELECT ISNULL((bilit.TotalAmount - ISNULL(CAST(payit.Amount AS decimal(18,2)),0)),0) AS OutstandingAmount 
                                                                FROM
                                                                    (SELECT ISNULL(CAST(bil.TotalAmount AS decimal(18,2)),0) AS TotalAmount,Id  
                                                                    FROM Billing AS bil
                                                                    WHERE bil.Id=@Id) bilit
                                                                LEFT JOIN
                                                                    (SELECT SUM(ISNULL(CAST(ucp.Amount AS decimal(18,2)),0)) AS Amount,BillingId  
                                                                    FROM UsersCreditPayment ucp
                                                                    WHERE BillingId=@Id
                                                                    GROUP BY BillingId) payit
                                                                ON payit.BillingId=bilit.Id";



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


        public static string GetTotalBilledByUser => @"SELECT sum(TotalAmount) FROM Billing WHERE PaymentMethodId=1 AND UserId=@UserId GROUP BY UserId";


        public static string UpdateCampaignCredit => @"UPDATE CampaignCreditPeriods SET CreditPeriod=@CreditPeriod WHERE ";


        public static string InsertCampaignCredit => @"INSERT INTO CampaignCreditPeriods(CreditPeriod,UserId,CampaignProfileId,UpdatedDate,CreatedDate,AdtoneServerCampaignCreditPeriodId)
                                                                VALUES(@CreditPeriod,@UserId,@CampaignProfileId, GETDATE(),GETDATE(),@AdtoneServerCampaignCreditPeriodId);
                                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";


    }

}
