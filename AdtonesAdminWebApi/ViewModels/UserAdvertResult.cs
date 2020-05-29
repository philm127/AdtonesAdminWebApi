using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class UserAdvertResult
    {
        public int AdvertId { get; set; }
        public string AdvertName { get; set; }
        public string Brand { get; set; }
        public string MediaFileLocation { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public string Scripts { get; set; }

        public string ScriptsPath { get; set; }
        public int userId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int Status { get; set; }
        public string rStatus => $"{(Enums.AdvertStatus)Status}";
        public string RejectionReason { get; set; }
        public int UpdatedBy { get; set; }

    }
}
