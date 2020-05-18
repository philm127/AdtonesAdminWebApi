using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class HelpAdminResult
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int? fuserId { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Organisation { get; set; }
        public string QNumber { get; set; }

        public int? ClientId { get; set; }
        public int? fClientId { get; set; }

        public string ClientName { get; set; }

        public int? CampaignProfileId { get; set; }

        public string CampaignName { get; set; }

        
        public DateTime? CreatedDate { get; set; }

        public string QuestionTitle { get; set; }

        public int QuestionSubjectId { get; set; }
        public int? fQuestionSubjectId { get; set; }
        public string QuestionSubject { get; set; }

        public int Status { get; set; }
        public string fStatus { get; set; }
        
        public DateTime? LastResponseDatetime { get; set; }

        public DateTime? LastResponseDateTimeByUser { get; set; }

        public int? PaymentMethodId { get; set; }

        public int? fPaymentMethodId { get; set; }

        public int UpdatedBy { get; set; }
    }
}
