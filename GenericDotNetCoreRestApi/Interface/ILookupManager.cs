using GenericDotNetCoreRestApi.Model.Request;
using GenericDotNetCoreRestApi.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace GenericDotNetCoreRestApi.Interface
{
    public interface ILookupManager
    {
        Task<(ResponseResult response, List<string> model)> GetClientEmailsAddresses();
        Task<(ResponseResult response, List<string> model)> GetContactNumbers();
        Task<(ResponseResult response, List<string> model)> GetAddresses();
    }
}
