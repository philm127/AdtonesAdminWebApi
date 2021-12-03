using Domain.Model;
using Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository;
{
    public interface ISoapDAL
    {
        Task<SoapApiResponseCodes> GetSoapApiResponse(string id);
    }
}
