using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ISharedSelectListsDAL
    {
        Task<IEnumerable<SharedSelectListViewModel>> GetCurrency(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetCountry(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetOperators(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetCreditUsers(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> AddCreditUsers(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetCamapignList(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetUserPaymentList(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetInvoiceList(int id = 0);
    }
}
