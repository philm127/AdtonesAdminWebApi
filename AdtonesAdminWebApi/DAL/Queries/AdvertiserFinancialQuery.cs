using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class AdvertiserFinancialQuery
    {


        public static string GetPaymentHistory => @"SELECT ucp.Id, ucp.Amount, ucp.CreatedDate, bil.InvoiceNumber 
                                                    FROM UsersCreditPayment AS ucp  LEFT JOIN Billing AS bil ON bil.Id=ucp.BillingId 
                                                    WHERE ucp.UserId=@userid ORDER BY CreatedDate Desc";


    }

}
