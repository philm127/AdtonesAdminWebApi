using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{

    public interface ISharedListQuery
    {
        string GetCurrencyList { get; }
        string GetCountryList { get; }
        string GetOperators { get; }
        string GetCreditUsers { get; }
        string AddCreditUserList { get; }
        string GetCampaignList { get; }
        string GetUserPaymentList { get; }
        string GetInvoiceList { get; }
    }


    public class SharedListQuery : ISharedListQuery
    {
        public string GetCurrencyList => "SELECT CurrencyId AS Value,CurrencyCode AS Text FROM Currencies";
        
        
        public string GetCountryList => @"SELECT Id AS Value,Name AS Text FROM Country";


        public string GetOperators => @"SELECT OperatorId AS Value,OperatorName AS Text FROM Operators WHERE IsActive=1";


        public string GetCreditUsers => @"SELECT u.UserId AS Value, CONCAT(FirstName,' ',LastName,'(',Email,')') AS Text FROM Users AS u
                                                INNER JOIN UsersCredit AS uc ON uc.UserId=u.UserId";


        public string AddCreditUserList => @"SELECT UserId AS Value, CONCAT(FirstName,' ',LastName,'(',Email,')') AS Text FROM Users 
                                                WHERE VerificationStatus=1
                                                AND Activated=1 AND RoleId=3 
                                                AND UserId NOT IN(SELECT UserId FROM UsersCredit)";


        public string GetCampaignList => @"SELECT Id AS Value,CampaignNameName AS Text FROM CampaignProfile";

        public string GetUserPaymentList => @"SELECT UserId AS Value,CONCAT(FirstName,'',LastName,'(',Email,')') AS Text FROM Users
                                                WHERE Activated=1 AND RoleId=3 AND Verification=1";


        public string GetInvoiceList => @"SELECT Id AS Value,InvoiceNumber AS Text FROM Billing WHERE PaymentMethod=1
                                            AND CampaignProfileId=@Id ORDER BY Id DEC";

    }
}
