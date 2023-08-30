using Microsoft.AspNetCore.Mvc;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AdtonesAdminWebApi.Services;
using Microsoft.AspNetCore.Http;
using AdtonesAdminWebApi.DAL.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ILoggingService _logServ;
        private readonly ITicketDAL _ticketDAL;
        IHttpContextAccessor _httpAccessor;
        ReturnResult result = new ReturnResult();
        const string PageName = "SharedListController";

        public TicketController(IHttpContextAccessor httpAccessor, ITicketService ticketService, ITicketDAL ticketDal, ILoggingService logServ)
        {
            _ticketService = ticketService;
            _ticketDAL = ticketDal;
            _logServ = logServ;
            _httpAccessor = httpAccessor;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List HelpAdminResult</returns>
        [HttpGet("v1/GetTicketList/{id}")]
        public async Task<ReturnResult> GetTicketList(int id=0)
        {
            try
            {
                result.body = await _ticketDAL.GetTicketList(id);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetTicketList");
                result.result = 0;
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List HelpAdminResult</returns>
        [HttpPut("v1/GetTicketListAsync")]
        public async Task<ReturnResult> GetTicketListAsync(PagingSearchClass paging)
        {
            var roleName = _httpAccessor.GetRoleFromJWT();
            if (roleName.ToLower().Contains("operator"))
                return await _ticketService.GetOperatorTicketList(paging);

            try
            {
                var res = await _ticketDAL.GetTicketListAsync(paging);
                result.recordcount = res.Count();
                result.body = res.Skip(paging.page * paging.pageSize).Take(paging.pageSize);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetTicketListAsync");
                result.result = 0;
            }

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List HelpAdminResult</returns>
        [HttpGet("v1/GetTicketListForSales/{id}")]
        public async Task<ReturnResult> GetTicketListForSales(int id = 0)
        {
            try
            {
                result.body = await _ticketDAL.GetTicketListForSales(id);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetTicketListSales");
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List HelpAdminResult</returns>
        [HttpGet("v1/GetTicketListForAdvertiser/{id}")]
        public async Task<ReturnResult> GetTicketListForAdvertiser(int id)
        {
            try
            {
                result.body = await _ticketDAL.GetTicketListForAdvertiser(id);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetTicketListForAdvertiser");
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains HelpAdminResult</returns>
        [HttpGet("v1/GetTicketDetails/{id}")]
        public async Task<ReturnResult> GetTicketDetails(int id)
        {
            var ticketList = new TicketListModel();
            try
            {
                ticketList = await _ticketDAL.GetTicketDetails(id);
                var commentList = await _ticketDAL.GetTicketcomments(id);

                if (ticketList == null)
                {
                    result.result = 0;
                    result.error = "There is no matching record";
                    return result;
                }

                ticketList.comments = (IEnumerable<TicketComments>)commentList;
                result.body = ticketList;
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetTicketDetails");
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains int No. records Updated</returns>
        [HttpPut("v1/UpdateTicketStatus")]
        public async Task<ReturnResult> UpdateTicketStatus(IdCollectionViewModel model)
        {
            return await _ticketService.UpdateTicketStatus(model.id,model.status);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains message success</returns>
        [HttpPost("v1/AddTicketComment")]
        public async Task<ReturnResult> AddTicketComment([FromForm] TicketComments model)
        {
            return await _ticketService.AddComment(model);
        }

    }
}
