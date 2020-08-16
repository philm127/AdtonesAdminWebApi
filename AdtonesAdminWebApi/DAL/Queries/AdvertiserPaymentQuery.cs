using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class AdvertiserPaymentQuery
    {
        


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



        public static string UpdateUserCredit => @"UPDATE UsersCredit SET AvailableCredit=AvailableCredit +  @AssignCredit,UpdatedDate=GETDATE()
                                                            WHERE UserId = @Id";


    }

}
