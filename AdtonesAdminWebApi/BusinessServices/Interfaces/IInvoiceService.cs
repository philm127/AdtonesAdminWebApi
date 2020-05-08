using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IInvoiceService
    {
        Task<ReturnResult> LoadInvoiceDataTable();
        Task<ReturnResult> SendInvoice(IdCollectionViewModel model);
    }
}
