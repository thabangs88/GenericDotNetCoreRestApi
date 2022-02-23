using GenericDotNetCoreRestApi.Model.Request;
using GenericDotNetCoreRestApi.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericDotNetCoreRestApi.Interface
{
    public interface IClientManager
    {
        Task<(ResponseResult response, ResultResponse model)> CreateClient(ClientInfo client);
        Task<(ResponseResult response, ResultResponse model)> UpdateClientInfo(ClientInfo client);
        Task<(ResponseResult response, ClientInfo model)> GetClientInfoById(int clientId);
        Task<(ResponseResult response, ClientInfo model)> GetClientInfoByName(string keyword);
        Task<(ResponseResult response, List<ClientAddressResponse> model)> SmartSearch(string keyword);
        Task<(ResponseResult response, List<ClientAddressResponse> model)> GetAllClientsWithAddress();
        Task<(ResponseResult response, ResultResponse model)> DeleteClientAddress (int? clientId, int? clientAddressId);
        Task<(ResponseResult response, ResultResponse model)> DeleteClientContact(int? clientId, int? clientContactId);
    }
}
