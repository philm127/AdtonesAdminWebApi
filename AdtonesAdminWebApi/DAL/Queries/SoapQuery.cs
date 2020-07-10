using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class SoapQuery
    {
        public static string GetSoapApiResponseCodes => @"SELECT Id,ReturnCode,Description FROM SoapApiResponseCodes
                                                        WHERE ReturnCode=@returnCode;";

    }

}
