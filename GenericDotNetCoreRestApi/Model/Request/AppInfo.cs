using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericDotNetCoreRestApi.Model.Request
{
    public class AppInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Platform { get; set; }
        public bool Active { get; set; }
    }
}
