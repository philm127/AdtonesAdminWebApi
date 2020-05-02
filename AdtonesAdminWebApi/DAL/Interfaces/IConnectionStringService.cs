using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IConnectionStringService
    {
        Task<string> GetSingleConnectionString(int CountryID = 0, int OperatorId = 0);
        Task<IEnumerable<string>> GetConnectionStrings(int CountryID = 0, int OperatorId = 0);
    }
}
