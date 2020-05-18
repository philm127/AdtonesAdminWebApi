using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    public static class HttpContextAccessor
    {
        public static int GetUserIdFromJWT(this Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
        {
            try
            {
                return int.Parse(accessor.HttpContext.User.FindFirst("userId")?.Value);
            }
            catch { }

            return 0;
        }


        public static int GetRoleIdFromJWT(this Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
        {
            try
            {
                return int.Parse(accessor.HttpContext.User.FindFirst("roleId")?.Value);
            }
            catch { }

            return 0;
        }


        public static int GetOperatorFromJWT(this Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
        {
            try
            {
                return int.Parse(accessor.HttpContext.User.FindFirst("operator")?.Value);
            }
            catch { }

            return 0;
        }


        public static string GetRoleFromJWT(this Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
        {
            try
            {
                return accessor.HttpContext.User.Identities.FirstOrDefault().Claims.Where(x => x.Type == "Role").FirstOrDefault().Value;

            }
            catch { }

            return string.Empty;
        }
    }
}
