using AdtonesAdminWebApi.CrmApp.Application.Interfaces.Users;
using AdtonesAdminWebApi.CrmApp.Application.Users.Subscribers.Dto;
using AdtonesAdminWebApi.CrmApp.Application.Users.Subscribers.Queries;
using AdtonesAdminWebApi.CrmApp.Core.Entities;
using AdtonesAdminWebApi.ViewModels;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CrmApp.Application.Users.Subscribers.Handlers
{
    public class GetAllSubscribersQueryHandler : IRequestHandler<GetAllSubscribersQuery, ReturnResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllSubscribersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ReturnResult> Handle(GetAllSubscribersQuery request, CancellationToken cancellationToken)
        {
            var retResult = new ReturnResult();
            try
            {
                var result = await _unitOfWork.Subscribers.GetAll(_mapper.Map<PagingSearchClass>(request));
                retResult.body = _mapper.Map<List<SubscriberDto>>(result.ToList());
                return retResult;
            }
            catch (Exception ex)
            {
                retResult.error = ex.Message.ToString();
                retResult.result = 0;
                return retResult;
            }
            
        }
    }
}