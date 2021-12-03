using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IAdvertCategoryDAL
    {
        Task<IEnumerable<AdvertCategoryResult>> GetAdvertCategoryList();
        Task<AdvertCategoryResult> GetAdvertCategoryDetails(int id);
        Task<int> UpdateAdvertCategory(AdvertCategoryResult model);
        Task<int> InsertAdvertCategory(AdvertCategoryResult model);
        Task<int> RemoveAdvertCategory(IdCollectionViewModel model);
    }
}
