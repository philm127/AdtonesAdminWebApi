using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public interface IUserManagementDAL
    {
        public List<CountryModel> GetList();
    }
}
