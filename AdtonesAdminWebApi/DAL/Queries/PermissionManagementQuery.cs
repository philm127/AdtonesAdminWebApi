using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class PermissionManagementQuery
    {

        public static string GetPermissionById => "SELECT Permissions FROM Users WHERE UserId=@UserId";

        public static string GetUsersPermissionByRole => "SELECT UserId,Permissions AS permissions FROM Users WHERE RoleId !=2 AND Permissions IS NOT NULL ";


        public static string UpdateUserPermissions => "UPDATE Users SET Permissions=@perms WHERE UserId=@Id";

        public static string PermissionsForSelectList => "SELECT TOP 1 Permissions AS permissions FROM Users WHERE RoleId=@RoleId AND Activated=1 AND Permissions IS NOT NULL ORDER BY UserId ASC";



    }
}
