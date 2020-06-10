

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface IUserManagementQuery
    {
        string UpdateUserStatus { get; }
        string GetUserById { get; }
        string UpdateCorpUser { get; }
        string getContactByUserId { get; }

    }


    public class UserManagementQuery : IUserManagementQuery
    {
        public string UpdateUserStatus => @"UPDATE Users SET Activated=@Activated WHERE ";
                                                                  
        public string GetUserById => @"SELECT UserId,u.OperatorId,Email,FirstName,LastName,DateCreated,Organisation,op.CountryId,
                                        Activated,RoleId,OrganisationTypeId,AdtoneServerUserId 
                                        FROM Users AS u LEFT JOIN Operators AS op ON op.OperatorId=u.OperatorId 
                                        WHERE UserId=@UserId";

        public string getContactByUserId => @"SELECT Id, UserId,MobileNumber,FixedLine,Email, PhoneNumber,Address,CountryId,CurrencyId 
                                                                                                FROM Contacts WHERE UserId = @userid ";

        // Comes from SoapApiService
        public string UpdateCorpUser => "UPDATE Users SET Activated=3, IsMsisdnMatch=false WHERE UserId=@Id;";

    }
}
