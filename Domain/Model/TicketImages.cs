using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class TicketImages
    {
        public int Id { get; set; }

        public int? QuestionId { get; set; }
        public string UploadImage { get; set; }
    }
}
