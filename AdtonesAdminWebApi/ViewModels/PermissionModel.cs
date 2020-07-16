using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    
    public class PermissionList
    {
        public List<PermissionModel> pages { get; set; }
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
        public string type { get; set; }
        public string route { get; set; }
        public string description { get; set; }
    }
}