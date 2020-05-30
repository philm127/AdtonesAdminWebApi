using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ITicketDAL
    {
        Task<IEnumerable<HelpAdminResult>> GetTicketList(string command);
        Task<HelpAdminResult> GetTicketDetails(string command, int id = 0);
        Task<int> UpdateTicketStatus(string command, HelpAdminResult model);
        Task<IEnumerable<HelpAdminResult>> GetOperatorTicketList(string command, int operatorId);
    }
}
