using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericDotNetCoreRestApi.Model.Request
{
    public class ClientInfo
    {
        public ClientInfo()
        {
            Addresses = new List<AddressInfo>();
            Contacts = new List<ContactInfo>();
        }

        public int? ClientId { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public List<AddressInfo> Addresses { get; set; }
        public List<ContactInfo> Contacts { get; set; }
    }
}
