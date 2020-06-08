using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface ISoapQuery
    {
        string GetSoapApiResponseCodes { get; }
    }


    public class SoapQuery : ISoapQuery
    {
        public string GetSoapApiResponseCodes => @"SELECT Id,ReturnCode,Description FROM SoapApiResponseCodes
                                                        WHERE ReturnCode=@returnCode;";

    }

}
