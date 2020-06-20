﻿

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface IUserManagementQuery
    {
        string UpdateUserStatus { get; }
        string GetUserDetails { get; }
        string UpdateCorpUser { get; }
        string getContactByUserId { get; }
        string GetOperatorAdmin { get; }
        string GetCompanyDetails { get; }
        string UpdateContacts { get; }
        string UpdateUser { get; }
    }


    public class UserManagementQuery : IUserManagementQuery
    {
        public string UpdateUserStatus => @"UPDATE Users SET Activated=@Activated WHERE ";
                                                                  
        public string GetUserDetails => @"SELECT UserId,u.OperatorId,Email,FirstName,LastName,DateCreated,Organisation,op.CountryId,
                                        Activated,RoleId,OrganisationTypeId,Outstandingdays,AdtoneServerUserId,VerificationStatus,PasswordHash 
                                        FROM Users AS u LEFT JOIN Operators AS op ON op.OperatorId=u.OperatorId 
                                        WHERE ";


        public string getContactByUserId => @"SELECT Id, UserId,MobileNumber,FixedLine,Email, PhoneNumber,Address,CountryId,CurrencyId 
                                                                                                FROM Contacts WHERE UserId = @userid ";

        // Comes from SoapApiService
        public string UpdateCorpUser => "UPDATE Users SET Activated=3, IsMsisdnMatch=false WHERE UserId=@Id;";


        public string GetCompanyDetails => @"SELECT Id, UserId,CompanyName,Address,AdditionalAddress,
                                            Town,PostCode,CountryId FROM CompanyDetails WHERE UserId = @userid ";


        public string GetOperatorAdmin => @"SELECT u.UserId,FirstName,LastName,Email,Organisation,u.OperatorId,o.CountryId,
                                            c.Name AS CountryName,o.OperatorName,u.Activated,u.DateCreated,con.MobileNumber,
                                            con.PhoneNumber, con.Address,con.Id
                                            FROM Users AS u LEFT JOIN Operators AS o ON u.OperatorId=o.OperatorId
                                            LEFT JOIN Country AS c ON o.CountryId=c.Id
                                            LEFT JOIN Contacts AS con ON con.UserId=u.UserId
                                            WHERE u.UserId=@userId";

        public string UpdateContacts => @"UPDATE Contacts SET MobileNumber=@MobileNumber,FixedLine=@FixedLine,Email=@Email,
                                            PhoneNumber=@PhoneNumber,Address=@Address,CountryId=@CountryId
                                            WHERE Id = @Id";


        public string UpdateUser => @"UPDATE Users SET FirstName=@FirstName, LastName = @LastName, Organisation = @Organisation
                                        ,Outstandingdays=@Outstandingdays, RoleId=@RoleId ";
            


    }
}
