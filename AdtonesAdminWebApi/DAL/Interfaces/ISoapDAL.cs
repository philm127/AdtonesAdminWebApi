using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ISoapDAL
    {
        Task<SoapApiResponseCodes> GetSoapApiResponse(string id);
    }
}
