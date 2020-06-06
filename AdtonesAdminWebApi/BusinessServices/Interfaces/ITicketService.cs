using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ITicketService
    {
        Task<ReturnResult> GetTicketDetails(int id = 0);
        Task<ReturnResult> UpdateTicketStatus(int id,int status);
        Task<ReturnResult> GetTicketList(int id = 0);
    }
}
