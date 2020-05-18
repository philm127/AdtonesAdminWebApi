using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class TicketService : ITicketService
    {
        ReturnResult result = new ReturnResult();
        
        IHttpContextAccessor _httpAccessor;
        private readonly ITicketDAL _ticketDAL;
        private readonly ITicketQuery _commandText;


        public TicketService(IHttpContextAccessor httpAccessor, ITicketDAL ticketDAL, ITicketQuery commandText)
        {
            _httpAccessor = httpAccessor;
            _commandText = commandText;
            _ticketDAL = ticketDAL;
        }


        public ReturnResult FillQuestionStatus(int? userId)
        {
            IEnumerable<Enums.TicketStatus> questionstatusTypes = Enum.GetValues(typeof(Enums.TicketStatus))
                                                     .Cast<Enums.TicketStatus>();
            result.body = (from action in questionstatusTypes
                                  select new SharedSelectListViewModel
                                  {
                                      Text = action.ToString(),
                                      Value = ((int)action).ToString()
                                  }).ToList();
            return result;
        }


        public async Task<ReturnResult> CloseTicket(int id)
        {
            var question = new HelpAdminResult
            {
                Id = id,
                Status = (int)Enums.TicketStatus.Closed,
                UpdatedBy = _httpAccessor.GetUserIdFromJWT()
            };

            try
            {
                result.body = await _ticketDAL.CloseTicket(_commandText.CloseTicket, question);
                return result;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "TicketService",
                    ProcedureName = "CloseTicket"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetTicketDetails(int id=0)
        {

            if (id == 0)
            {
                try
                {
                    result.body = await _ticketDAL.GetTicketList(_commandText.GetLoadTicketDatatable);
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "TicketService",
                        ProcedureName = "GetTicketList"
                    };
                    _logging.LogError();
                    result.result = 0;
                }
            }
            else
            {
                try
                {
                    result.body = await _ticketDAL.GetTicketDetails(_commandText.GetTicketDetails, id);
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "TicketService",
                        ProcedureName = "GetTicketDetails"
                    };
                    _logging.LogError();
                    result.result = 0;
                }
            }
                return result;
        }


    }
}
