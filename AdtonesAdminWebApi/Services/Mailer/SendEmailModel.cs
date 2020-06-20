using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services.Mailer
{
    public class SendEmailModel
    {
        public string Subject { get; set; }
        public string[] To { get; set; }
        public string SingleTo { get; set; }
        public string[] CC { get; set; }
        public string[] Bcc { get; set; }

        public string From { get; set; }

        public string Body { get; set; }

        public string[] attachment { get; set; }

        public bool isBodyHTML { get; set; }
        public string Link { get; set; }

        public string Title { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Organisation { get; set; }
        public string CompletedDatetime { get; set; }
        public String FullName
        {
            get
            {
                return Title + " " + Fname + " " + Lname;
            }
        }

        public int FormatId { get; set; }

        public string PaymentLink { get; set; }
        public string InvoiceNumber { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? DueDate { get; set; }
    }
}