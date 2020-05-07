using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class SystemConfigFormModel
    {
        [Key]
        public int SystemConfigId { get; set; }
        public string SystemConfigKey { get; set; }

        public string SystemConfigValue { get; set; }
        public string SystemConfigType { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        /// No idea what this is or for when looking at the form
        /// TODO: Find out what this is
        /// 
        //public IEnumerable<SelectListItem> GetSystemConfigType
        //{
        //    get
        //    {
        //        return new[]
        //        {
        //        new SelectListItem { Value = "Website", Text = "Website" },
        //        new SelectListItem { Value = "ProvisioningService", Text = "ProvisioningService" },

        //    };
        //    }
        //}


    }
}
