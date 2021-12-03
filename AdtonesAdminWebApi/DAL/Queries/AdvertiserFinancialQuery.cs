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


        public static string GetPaymentHistory => @"SELECT ucp.Id, ucp.Amount, ucp.CreatedDate, bil.InvoiceNumber 
                                                    FROM UsersCreditPayment AS ucp  LEFT JOIN Billing AS bil ON bil.Id=ucp.BillingId 
                                                    WHERE ucp.UserId=@userid ORDER BY CreatedDate Desc";


    }

}
