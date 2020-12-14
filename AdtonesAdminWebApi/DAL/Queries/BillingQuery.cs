using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public class BillingQuery
    {
         
        public static string InsertIntoBilling => @"INSERT INTO Billing(UserId, ClientId ,CampaignProfileId,PaymentMethodId,InvoiceNumber,
                                                    PONumber,FundAmount,TaxPercantage,TotalAmount,PaymentDate,SettledDate,Status,CurrencyCode,AdtoneServerBillingId)
                                                    VALUES(@UserId, @ClientId ,@CampaignProfileId,@PaymentMethodId,@InvoiceNumber,
                                                    @PONumber, @Fundamount, @TaxPercantage, @TotalAmount, GETDATE(),@SettledDate,@Status,@CurrencyCode);
                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
    }
}
