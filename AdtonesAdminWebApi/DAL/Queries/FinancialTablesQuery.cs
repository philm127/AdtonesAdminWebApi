using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class FinancialTablesQuery
    {
        public static string GetInvoiceResultSet => @"SELECT bil.Id AS BillingId,bil.InvoiceNumber,bil.PONumber,ISNULL(cl.Name,'-') AS ClientName,
                                                        camp.CampaignName,bil.PaymentDate AS CreatedDate, camp.CampaignprofileId,
                                                        ucp.Amount AS InvoiceTotal,(Case WHEN bil.Status=3 THEN 'Fail' ELSE 'Paid' END) AS rStatus,
                                                        bil.SettledDate,(CASE WHEN bil.PaymentMethodId=1 THEN 'Cheque' ELSE pay.Description END)
                                                        AS PaymentMethod,bil.CurrencyCode,
                                                        bil.Status AS Status,ucp.UserId,
                                                        CONCAT(usr.FirstName,' ',usr.LastName) AS FullName,
                                                        CONCAT(@siteAddress,'/Invoice/Adtones_invoice_',bil.InvoiceNumber,'.pdf') AS InvoicePath,
                                                        usr.Email,ISNULL(usr.Organisation, '-') AS Organisation
                                                        FROM ( SELECT ucpO.UserId,ucpO.BillingId,SUM(ISNULL(ucpO.Amount,0)) AS Amount,ucpO.CampaignProfileId,
                                                                ucpD.Description 
                                                                FROM UsersCreditPayment AS ucpO
                                                                INNER JOIN
                                                                    ( SELECT BillingId,ISNULL(Description, '-') AS Description FROM UsersCreditPayment 
                                                                        WHERE Id IN
                                                                            (SELECT MAX(Id) FROM UsersCreditPayment GROUP BY BillingId )
                                                                        ) AS ucpD
                                                                ON ucpD.BillingId=ucpO.BillingId
                                                                GROUP BY ucpO.UserId,ucpO.BillingId,ucpO.CampaignProfileId,ucpD.Description
                                                            ) AS ucp 
                                                        LEFT JOIN Billing AS bil ON ucp.BillingId=bil.Id 
                                                        LEFT JOIN Client AS cl ON bil.ClientId=cl.Id
                                                        LEFT JOIN CampaignProfile camp ON camp.CampaignProfileId=ucp.CampaignProfileId
                                                        LEFT JOIN PaymentMethod AS pay ON bil.PaymentMethodId=pay.Id
                                                        LEFT JOIN Users AS usr ON ucp.UserId=usr.UserId
                                                        WHERE ucp.Amount >= bil.TotalAmount ";


        public static string GetOutstandingInvoiceResultSet => @"SELECT bil.Id AS BillingId,bil.UserId,bil.CampaignProfileId,bil.InvoiceNumber,
                                                                bil.TotalAmount AS CreditAmount,ISNULL(ucp.Amount,0) AS PaidAmount,ucp.Description,
                                                                bil.TotalAmount,bil.PaymentDate AS CreatedDate,ISNULL(cl.Name, '-') AS ClientName,
                                                                ISNULL(u.Organisation, '-') AS Organisation,u.Email,
                                                                CONCAT(u.FirstName,'',u.LastName) AS FullName,(bil.TotalAmount - ISNULL(ucp.Amount,0)) AS OutStandingAmount,
                                                                CONCAT(@siteAddress,'/Invoice/Adtones_invoice_',bil.InvoiceNumber,'.pdf') AS InvoicePath,
                                                                CASE WHEN ISNULL(ucp.Amount,0)>0 THEN 1 ELSE 0 END AS Status,cp.CampaignName,bil.CurrencyCode
                                                                FROM Billing AS bil LEFT JOIN Client AS cl ON cl.Id=bil.ClientId
                                                                INNER JOIN Users AS u ON bil.UserId=u.UserId
                                                                LEFT JOIN
                                                                    ( SELECT ucpO.UserId,ucpO.BillingId,SUM(ISNULL(ucpO.Amount,0)) AS Amount,ucpO.CampaignProfileId,
                                                                        ucpD.Description 
                                                                        FROM UsersCreditPayment AS ucpO
                                                                        INNER JOIN
                                                                            ( SELECT BillingId,ISNULL(Description, '-') AS Description FROM UsersCreditPayment 
                                                                                WHERE Id IN
                                                                                    (SELECT MAX(Id) FROM UsersCreditPayment GROUP BY BillingId )
                                                                             ) AS ucpD
                                                                        ON ucpD.BillingId=ucpO.BillingId
                                                                        GROUP BY ucpO.UserId,ucpO.BillingId,ucpO.CampaignProfileId,ucpD.Description
                                                                    ) AS ucp
                                                                ON ucp.BillingId=bil.Id
                                                                INNER JOIN CampaignProfile AS cp ON cp.CampaignProfileId=bil.CampaignProfileId
                                                                LEFT JOIN Contacts AS con ON u.UserId=con.UserId
                                                                WHERE PaymentMethodId=1
                                                                AND bil.TotalAmount > ISNULL(Amount,0)";


        public static string GetOutstandingInvoiceForSalesResultSet => @"SELECT bil.Id AS BillingId,bil.UserId,bil.CampaignProfileId,bil.InvoiceNumber,
                                                                bil.TotalAmount AS CreditAmount,ISNULL(ucp.Amount,0) AS PaidAmount,ucp.Description,
                                                                bil.PaymentDate AS CreatedDate,ISNULL(cl.Name, '-') AS ClientName,
                                                                ISNULL(u.Organisation, '-') AS Organisation,u.Email,sexcs.UserId AS SUserId,
                                                                CASE WHEN sexcs.FirstName IS NULL THEN 'UnAllocated' 
                                                                    ELSE CONCAT(sexcs.FirstName,' ',sexcs.LastName) END AS SalesExec,
                                                                CONCAT(u.FirstName,'',u.LastName) AS FullName,(bil.TotalAmount - ISNULL(ucp.Amount,0)) AS OutStandingAmount,
                                                                CONCAT(@siteAddress,'/Invoice/Adtones_invoice_',bil.InvoiceNumber,'.pdf') AS InvoicePath,
                                                                CASE WHEN ISNULL(ucp.Amount,0)>0 THEN 1 ELSE 0 END AS Status,cp.CampaignName,bil.CurrencyCode
                                                                FROM Billing AS bil LEFT JOIN Client AS cl ON cl.Id=bil.ClientId
                                                                INNER JOIN Users AS u ON bil.UserId=u.UserId
                                                                LEFT JOIN
                                                                    ( SELECT ucpO.UserId,ucpO.BillingId,SUM(ISNULL(ucpO.Amount,0)) AS Amount,ucpO.CampaignProfileId,
                                                                        ucpD.Description 
                                                                        FROM UsersCreditPayment AS ucpO
                                                                        INNER JOIN
                                                                            ( SELECT BillingId,ISNULL(Description, '-') AS Description FROM UsersCreditPayment 
                                                                                WHERE Id IN
                                                                                    (SELECT MAX(Id) FROM UsersCreditPayment GROUP BY BillingId )
                                                                             ) AS ucpD
                                                                        ON ucpD.BillingId=ucpO.BillingId
                                                                        GROUP BY ucpO.UserId,ucpO.BillingId,ucpO.CampaignProfileId,ucpD.Description
                                                                    ) AS ucp
                                                                ON ucp.BillingId=bil.Id
                                                                INNER JOIN CampaignProfile AS cp ON cp.CampaignProfileId=bil.CampaignProfileId
                                                               LEFT JOIN Advertisers_SalesTeam AS sales ON u.UserId=sales.AdvertiserId 
                                                                LEFT JOIN Users AS sexcs ON sexcs.UserId=sales.SalesExecId
                                                                LEFT JOIN Contacts AS con ON u.UserId=con.UserId
                                                                WHERE PaymentMethodId=1
                                                                AND bil.TotalAmount > ISNULL(Amount,0) ";



        public static string GetInvoiceToPDF => @"SELECT pay.Description,bil.InvoiceNumber,bil.SettledDate,co.CountryId,ucp.Amount,usr.Email,
                                                    usr.FirstName, usr.LastName
                                                    FROM Billing AS bil 
                                                    INNER JOIN PaymentMethod AS pay ON bil.PaymentMethodId=pay.Id
                                                    INNER JOIN Users AS usr ON bil.UserId=usr.UserId
                                                    LEFT JOIN CompanyDetails AS co ON usr.UserId=co.UserId
                                                    LEFT JOIN UsersCreditPayment AS ucp ON ucp.UserId=bil.UserId
                                                    WHERE bil.Id=@billingId AND ucp.Id=@ucpId;";


        public static string UpdateUserCreditPayment => @"INSERT INTO UsersCreditPayment(UserId,BillingId,Amount,Description,Status,CreatedDate,UpdatedDate,
                                                            CampaignprofileId)
                                                          VALUES(@UserId,@BillingId,@Amount,@Description,@Status,GETDATE(),GETDATE(),
                                                            @CampaignprofileId);";


        public static string GetCampaignCreditPeriodData => @"SELECT ccp.CampaignCreditPeriodId,ccp.UserId,ccp.CampaignProfileId,ccp.CreditPeriod,
                                                                ccp.CreatedDate,CONCAT(u.FirstName,' ',u.LastName) AS UserName, cp.CampaignName
                                                                FROM CampaignCreditPeriods AS ccp INNER JOIN Users AS u ON u.UserId=ccp.UserId
                                                                INNER JOIN CampaignProfile AS cp ON cp.CampaignProfileId=ccp.CampaignProfileId
                                                                LEFT JOIN Contacts AS con ON con.UserId=ccp.UserId
                                                                LEFT JOIN Operators AS op ON op.CountryId=con.CountryId";


    }

}
