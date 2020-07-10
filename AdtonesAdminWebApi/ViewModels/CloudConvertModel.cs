using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class CloudConvertModel
    {
        public class Output
        {
            public string url { get; set; }
        }

        public class Input
        {
            public string type { get; set; }
        }

        public class Upload
        {
            public string url { get; set; }
        }

        public class RootObject
        {
            public string id { get; set; }
            public string url { get; set; }
            public int expire { get; set; }
            public int percent { get; set; }
            public string message { get; set; }
            public string step { get; set; }
            public int starttime { get; set; }
            public Output output { get; set; }
            public Input input { get; set; }
            public Upload upload { get; set; }
        }
    }
}