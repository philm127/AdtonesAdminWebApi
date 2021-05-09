using AdtonesAdminWebApi.CrmApp.Application.Interfaces.Users;
using AdtonesAdminWebApi.CrmApp.Application.Users.Users.Dto;
using AdtonesAdminWebApi.CrmApp.Application.Users.Users.Queries;
using AdtonesAdminWebApi.ViewModels;
using AutoMapper;
// using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CrmApp.Application.Users.Users.Handlers
{
    public class GetAllUsersQueryHandler //: IRequestHandler<GetAllUsersQuery, ReturnResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<ReturnResult> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var retResult = new ReturnResult();
            try
            {
                var result = await _unitOfWork.Users.GetAll();
                retResult.body = _mapper.Map<List<UserDto>>(result.ToList());
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