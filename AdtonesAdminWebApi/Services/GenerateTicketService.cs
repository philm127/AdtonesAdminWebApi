using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.TicketingModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    public interface IGenerateTicketService
    {
        Task CreateAdTicket(int userId, string sub, string msg, int subjectId, int advertId);
        Task CreateAdTicketForBilling(int userId, string sub, string msg, int subjectId, int? ClientId, int CampaignProfileId, int PaymentMethodId);
    }


    public class GenerateTicketService : IGenerateTicketService
    {
        private readonly ILiveAgentService _liveService;
        private readonly IUserManagementDAL _userDal;
        private readonly ITicketDAL _ticketDAL;
        private readonly IConfiguration _configuration;

        public GenerateTicketService(ILiveAgentService liveService, IUserManagementDAL userDal, ITicketDAL ticketDAL,
                                        IConfiguration configuration)
        {
            _liveService = liveService;
            _userDal = userDal;
            _ticketDAL = ticketDAL;
            _configuration = configuration;
        }


        public async Task CreateAdTicket(int userId, string sub, string msg, int subjectId, int advertId)
        {
            var userDetail = await _userDal.GetUserById(userId);
            string subject = sub;
            string message = msg;
            string ticketCode = string.Empty;
            var test = _configuration.GetValue<bool>("Environment:Test");
            if (!test)
                ticketCode = _liveService.CreateTicket(subject, message, userDetail.Email);
            else
                ticketCode = "testing123";

            TicketFormModel model = new TicketFormModel();

            model.UserId = userId;
            model.QNumber = ticketCode;
            model.SubjectId = subjectId;
            model.ClientId = null;
            model.CampaignProfileId = null;
            model.PaymentMethodId = null;
            model.Title = subject;
            model.Description = message;
            model.CreatedDate = DateTime.Now;
            model.UpdatedDate = DateTime.Now;
            model.LastResponseDateTime = null;
            model.LastResponseDateTimeByUser = null;
            model.Status = (int)Enums.QuestionStatus.New;
            model.UpdatedBy = null;
            if (advertId == 0)
            {
                model.AdvertId = null;
            }
            else
            {
                model.AdvertId = advertId;
            }

            var x = _ticketDAL.CreateNewHelpTicket(model);
        }

        public async Task CreateAdTicketForBilling(int userId, string sub, string msg, int subjectId, int? ClientId, int CampaignProfileId, int PaymentMethodId)
        {
            var userDetail = await _userDal.GetUserById(userId);
            string subject = sub;
            string message = msg;
            string ticketCode = string.Empty;
            var test = _configuration.GetValue<bool>("Environment:Test");
            if (!test)
                ticketCode = _liveService.CreateTicket(subject, message, userDetail.Email);
            else
                ticketCode = "testing123";

            TicketFormModel model = new TicketFormModel();

            model.UserId = userId;
            model.QNumber = ticketCode;
            model.SubjectId = subjectId;
            model.ClientId = ClientId == 0 ? null : ClientId;
            model.CampaignProfileId = CampaignProfileId;
            model.PaymentMethodId = PaymentMethodId;
            model.Title = subject;
            model.Description = message;
            model.CreatedDate = DateTime.Now;
            model.UpdatedDate = DateTime.Now;
            model.LastResponseDateTime = null;
            model.LastResponseDateTimeByUser = null;
            model.Status = (int)Enums.QuestionStatus.New;
            model.UpdatedBy = null;
            model.AdvertId = null;

            var x = _ticketDAL.CreateNewHelpTicket(model);
        }
    }
}