using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{

    public class PermissionList
    {
        public List<PermissionModel> pageData { get; set; }
    }

    public class PermissionModel
    {
        public string pageName { get; set; }
        public bool visible { get; set; }
        public List<PermElement> elements { get; set; }
    }

    public class PermElement
    {
        public string name { get; set; }
        public bool visible { get; set; }
        public bool enabled { get; set; }

        // Type of usage. Search to be included in search,
        // element, a general element
        public string type { get; set; }
        public string route { get; set; }
        public string description { get; set; }

        // Used in general section to set Operator,
        // Country or Advertiser restrictions
        public int[] arrayId { get; set; }// => arrayId.ToString().Split(',').Select(n => Convert.ToInt32(n)).ToArray();

        // Needed for menus to work
        public List<PermElement> elements { get; set; }
    }

    public class PermissionChangeModel
    {
        public int UserId { get; set; }

        public string permissions { get; set; }
    }


    public class AddNewPermissionPart
    {
        public bool all { get; set; }
        public string[] roles { get; set; }
        public string[] users { get; set; }
        public string pageName { get; set; }
        public bool visible { get; set; }
        public List<PermElement> elements { get; set; }
    }

}