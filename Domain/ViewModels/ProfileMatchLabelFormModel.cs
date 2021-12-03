using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class ProfileMatchLabelFormModel
    {
        public int Id { get; set; } = 0;
        public int ProfileMatchInformationId { get; set; } = 0;
        public string ProfileLabel { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
