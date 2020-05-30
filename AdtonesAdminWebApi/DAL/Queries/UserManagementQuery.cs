

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface IUserManagementQuery
    {
        string UpdateUserStatus { get; }

    }


    public class UserManagementQuery : IUserManagementQuery
    {
        public string UpdateUserStatus => @"UPDATE Users SET Activated=@Activated WHERE ";
                                                                  
                

    }
}
