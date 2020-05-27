using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class SystemConfigResult
    {
        [Key]
        public int SystemConfigId { get; set; }


        public string SystemConfigKey { get; set; }


        public string SystemConfigValue { get; set; }

        public string SystemConfigType { get; set; }


        public DateTime CreatedDate { get; set; }

    }
}
