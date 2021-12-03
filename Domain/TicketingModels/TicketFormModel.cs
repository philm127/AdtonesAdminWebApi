using AdtonesAdminWebApi.Model;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.TicketingModels
{
    public class TicketFormModel
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string QNumber { get; set; }

        [Required]
        public int SubjectId { get; set; }


        public int? ClientId { get; set; }


        public int? CampaignProfileId { get; set; }

        public int? PaymentMethodId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]

        [StringLength(200, ErrorMessage = "The Description field cannot be more than 200 characters.")]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? LastResponseDateTime { get; set; }

        public DateTime? LastResponseDateTimeByUser { get; set; }

        public int Status { get; set; }

        public int? UpdatedBy { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int? AdvertId { get; set; }
        public TicketSubject QuestionSubject { get; set; }
        public User User { get; set; }

        public Client Client { get; set; }

        // public CampaignProfile CampaignProfile { get; set; }

        public PaymentModel PaymentMethod { get; set; }

        public ICollection<TicketImages> QuestionImages { get; set; }

        public ICollection<TicketComments> QuestionComment { get; set; }
    }
}