using GenericDotNetCoreRestApi.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericDotNetCoreRestApi.Model.Response
{
    public class ClientAddressResponse
    {
        public ClientAddressResponse()
        {
            Addresses = new List<AddressInfo>();
        }
        public int? ClientId { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<AddressInfo> Addresses { get; set; }
    }
}
