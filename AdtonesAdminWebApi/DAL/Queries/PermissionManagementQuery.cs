using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class PermissionManagementQuery
    {

        public static string GetPermissionById => "SELECT Permissions FROM Users WHERE UserId=@UserId";

    }
}
