using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories.Queries
{
    public static class SoapQuery
    {
        public static string GetSoapApiResponseCodes => @"SELECT Id,ReturnCode,Description FROM SoapApiResponseCodes
                                                        WHERE ReturnCode=@returnCode;";

    }

}
