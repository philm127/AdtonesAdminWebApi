using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public class BillingQuery
    {
         
        public static string InsertIntoBilling => @"INSERT INTO Billing(UserId, CampaignProfileId,PaymentMethodId,InvoiceNumber,
                                                    PONumber,FundAmount,TaxPercantage,TotalAmount,PaymentDate,SettledDate,Status,CurrencyCode,AdtoneServerBillingId)
                                                    VALUES(@UserId, @CampaignProfileId,@PaymentMethodId,@InvoiceNumber,
                                                    @PONumber, @Fundamount, @TaxPercantage, @TotalAmount, GETDATE(),@SettledDate,@Status,@CurrencyCode,@AdtoneServerBillingId);
                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";



        public static string GetInvoiceDetailsForPDF => @"SELECT bil.InvoiceNumber, camp.CampaignName,pay.Description AS MethodOfPayment,
                                                        CONCAT(usr.FirstName,' ',usr.LastName) AS FullName,
                                                        bil.PaymentMethodId,bil.CampaignProfileId,comp.CompanyName,comp.Address AS AddressLine1,comp.AdditionalAddress AS AddressLine2,
                                                        comp.Town AS City,comp.PostCode,ct.Name AS InvoiceCountry,ct.ShortName, bil.FundAmount,bil.PONumber,ccred.CreditPeriod, con.PhoneNumber,
                                                        usr.Email,tx.TaxPercantage AS InvoiceTax
                                                        FROM Billing AS bil
                                                        INNER JOIN Users AS usr ON usr.UserId=bil.UserId
                                                        LEFT JOIN CompanyDetails AS comp ON comp.UserId=usr.UserId
                                                        LEFT JOIN Contacts AS con ON con.UserId=usr.UserId
                                                        LEFT JOIN CampaignProfile AS camp ON camp.CampaignProfileId=bil.CampaignProfileId
                                                        LEFT JOIN CampaignCreditPeriods AS ccred ON ccred.CampaignProfileId=camp.CampaignProfileId
                                                        LEFT JOIN PaymentMethod AS pay ON pay.Id=bil.PaymentMethodId
                                                        LEFT JOIN Country AS ct ON ct.Id=comp.CountryId
                                                        LEFT JOIN CountryTax AS tx ON tx.CountryId=ct.Id
                                                        WHERE bil.Id=@Id";

        public static string GetCreditPeriod => @"SELECT CreditPeriod FROM CampaignCreditPeriods WHERE CampaignProfileId=@Id";

    }
}
