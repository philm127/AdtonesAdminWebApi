using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class FtpDetailsModel
    {
        public int OperatorFTPDetailId { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FtpRoot { get; set; }
        public int OperatorId { get; set; }
    }
}
