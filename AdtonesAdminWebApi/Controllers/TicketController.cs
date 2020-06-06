using Microsoft.AspNetCore.Mvc;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    // [Authorize]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List HelpAdminResult</returns>
        [HttpGet("v1/GetTicketList/{id}")]
        public async Task<ReturnResult> GetTicketList(int id=0)
        {
            return await _ticketService.GetTicketList(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains HelpAdminResult</returns>
        [HttpGet("v1/GetTicketDetails/{id}")]
        public async Task<ReturnResult> GetTicketDetails(int id)
        {
            return  await _ticketService.GetTicketDetails(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains int No. records Updated</returns>
        [HttpPut("v1/CloseTicket/{id}")]
        public async Task<ReturnResult> CloseTicket(int id)
        {
            var status = (int)Enums.TicketStatus.Closed;
            return await _ticketService.UpdateTicketStatus(id,status);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains int No. records Updated</returns>
        [HttpPut("v1/ArchiveTicket/{id}")]
        public async Task<ReturnResult> ArchiveTicket(int id)
        {
            var status = (int)Enums.TicketStatus.Archived;
            return await _ticketService.UpdateTicketStatus(id, status);
        }

    }
}
