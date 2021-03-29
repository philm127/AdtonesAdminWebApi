using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CrmApp.Application.Interfaces
{
    public interface IGenericSearchRepository<T> where T : class
    {
        // Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll(PagingSearchClass entity);
        //Task<int> Add(T entity);
        //Task<int> Delete(int id);
        //Task<int> Update(T entity);
    }
}