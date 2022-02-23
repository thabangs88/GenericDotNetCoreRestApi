using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericDotNetCoreRestApi.Model.Request
{
    public class UserInfo
    {
        public string Username { get; set; }
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public int CompanyID { get; set; }
        public int AppID { get; set; }
        public bool Active { get; set; }
        public CompanyInfo CompanyInfo { get; set; }
        public AppInfo AppInfo { get; set; }
    }
}
