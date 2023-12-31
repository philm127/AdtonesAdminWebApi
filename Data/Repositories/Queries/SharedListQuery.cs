﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories.Queries
{

    public static class SharedListQuery
    {
        public static string GetCurrencyList => "SELECT c.CurrencyId AS Value,c.CurrencyCode AS Text FROM Currencies AS c";
        
        
        public static string GetCountryList => @"SELECT Id AS Value,Name AS Text FROM Country ";


        public static string GetOperators => @"SELECT c.OperatorId AS Value,c.OperatorName AS Text FROM Operators AS c WHERE IsActive=1";

        public static string GetAdvertCategory => @"SELECT AdvertCategoryId AS Value, Name AS Text FROM AdvertCategories WHERE CountryId=@Id;";

        public static string GetCampaignCategory => @"SELECT CampaignCategoryId AS Value, CategoryName AS Text FROM CampaignCategory WHERE Active=1 ";

        public static string GetOrganisationTypes => @"SELECT OrganisationTypeId AS Value, Type AS Text FROM OrganisationTypes;";


        public static string GetCreditUsers => @"SELECT u.UserId AS Value, CONCAT(FirstName,' ',LastName,'(',u.Email,')') AS Text FROM Users AS u
                                                INNER JOIN UsersCredit AS uc ON uc.UserId=u.UserId INNER JOIN Contacts as c ON c.UserId=u.UserId";


        public static string GetUserwRole => @"SELECT u.UserId AS Value,RoleId, CONCAT(FirstName,' ',LastName) AS Text 
                                                FROM Users AS u WHERE RoleId !=2 ORDER BY RoleId";


        public static string GetUserPermissionwRole => @"SELECT u.UserId AS Value,RoleId, CONCAT(FirstName,' ',LastName) AS Text 
                                                FROM Users AS u WHERE RoleId !=2 AND ((Permissions IS NOT NULL) OR (Permissions <> '0')) ";


        public static string AddCreditUserList => @"SELECT u.UserId AS Value, CONCAT(FirstName,' ',LastName,'(',u.Email,')') AS Text 
                                                FROM Users AS u LEFT JOIN Contacts AS c ON c.UserId=u.UserId
                                                WHERE VerificationStatus=1
                                                AND u.Activated=1 AND u.RoleId=3 
                                                AND u.UserId NOT IN(SELECT UserId FROM UsersCredit)";


        public static string GetCampaignList => @"SELECT c.CampaignProfileId AS Value,c.CampaignName AS Text FROM CampaignProfile AS c";


        public static string GetUserPaymentList => @"SELECT u.UserId AS Value,CONCAT(u.FirstName,'',u.LastName,'(',u.Email,')') AS Text 
                                                    FROM Users AS u
                                                    INNER JOIN Contacts as c ON c.UserId=u.UserId
                                                    WHERE u.Activated=1 AND u.RoleId=3 AND u.VerificationStatus=1";


        public static string GetInvoiceNumberList => @"SELECT Id AS Value,InvoiceNumber AS Text FROM Billing WHERE PaymentMethodId=1
                                                    AND CampaignProfileId=@Id ORDER BY Id DESC";


        public static string GetClientList => @"SELECT Id AS Value,Name AS Text FROM Client WHERE UserId=@Id ";


        public static string GetPaymentTypeList => @"SELECT Id AS Value, Name AS Text FROM PaymentMethod";


        public static string GetQuestionSubject => @"SELECT SubjectId AS Value, Name AS Text FROM QuestionSubject";

    }
}
