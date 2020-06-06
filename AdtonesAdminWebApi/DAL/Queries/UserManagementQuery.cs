

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface IUserManagementQuery
    {
        string UpdateUserStatus { get; }
        string GetUserById { get; }

    }


    public class UserManagementQuery : IUserManagementQuery
    {
        public string UpdateUserStatus => @"UPDATE Users SET Activated=@Activated WHERE ";
                                                                  
        public string GetUserById => @"SELECT UserId,OperatorId,Email,FirstName,LastName,DateCreated,Organisation,
                                        Activated,RoleId,OrganisationTypeId,AdtoneServerUserId 
                                        FROM Users WHERE UserId=@UserId";

    }
}
