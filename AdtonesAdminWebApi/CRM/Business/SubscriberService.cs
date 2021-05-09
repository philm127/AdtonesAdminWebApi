using AdtonesAdminWebApi.CRM.Repositories;
using AdtonesAdminWebApi.Services;
using System;
using System.Threading.Tasks;
using AdtonesAdminWebApi.CRM.Models;
using AutoMapper;
using AdtonesAdminWebApi.CRM.Models.Subscriber;
using System.Collections.Generic;
using System.Linq;

namespace AdtonesAdminWebApi.CRM.Business
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ISubscriberRepositorry _subDAL;
        private readonly ILoggingService _logServ;
        private readonly IMapper _mapper;
        ReturnResult result = new ReturnResult();
        const string PageName = "SubscriberService";

        public SubscriberService(ISubscriberRepositorry subDAL, ILoggingService logServ, IMapper mapper)
        {
            _subDAL = subDAL;
            _logServ = logServ;
            _mapper = mapper;
        }


        public async Task<ReturnResult> GetSubscriberList(PagingSearchClass paging)
        {
            try
            {
                var res = await _subDAL.GetSubscribers(paging);
                result.recordcount = res.Count();
                var body = res.Skip(paging.page * paging.pageSize).Take(paging.pageSize);
                result.body = _mapper.Map<IEnumerable<SubscriberListDto>>(body);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetSubscriberList";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }


        public async Task<ReturnResult> GetSubscriber(int subscriberId)
        {
            try
            {
                result.body = await _subDAL.GetSubscriber(subscriberId);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetSubscriber";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }
    }
}
