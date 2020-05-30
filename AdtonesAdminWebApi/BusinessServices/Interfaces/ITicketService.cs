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
        Task<ReturnResult> CloseTicket(int id);
        Task<ReturnResult> ArchiveTicket(int id);
        // Task<ReturnResult> GetOperatorTicketList();
    }
}
