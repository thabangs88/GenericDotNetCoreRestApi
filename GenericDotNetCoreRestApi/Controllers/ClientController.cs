using GenericDotNetCoreRestApi.Implementation;
using GenericDotNetCoreRestApi.Interface;
using GenericDotNetCoreRestApi.Model.Context;
using GenericDotNetCoreRestApi.Model.Request;
using GenericDotNetCoreRestApi.Model.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericDotNetCoreRestApi.Controllers
{
    [Authorize]
    [ApiExplorerSettings(GroupName = "Client Manager")]
    [Route("api/client/")]
    [ApiController]

    public class ClientController : ControllerBase
    {
        private readonly IClientManager _clientManager;

        public ClientController(MasterContext context)
        {
            _clientManager = new ClientManager(context);
        }

        [HttpPost("create/")]
        public async Task<ActionResult<ResultResponse>> Create(ClientInfo model)
        {
            try
            {
                var response = _clientManager.CreateClient(model).Result;

                if (response.response.Success)
                    return await Task.FromResult(response.model);
                else
                    return await Task.FromResult(StatusCode(400, response.response.ErrorMessage));

            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

        }

        [HttpPost("update/")]
        public async Task<ActionResult<ResultResponse>> Update(ClientInfo model)
        {
            try
            {
                var response = _clientManager.UpdateClientInfo(model).Result;

                if (response.response.Success)
                    return await Task.FromResult(response.model);
                else
                    return StatusCode(400, response.response.ErrorMessage);
            }
            catch (Exception ex)
            {

                return StatusCode(400, ex.Message);
            }

        }
        [HttpGet("getClientInfoById/")]
        public async Task<ActionResult<ClientInfo>> GetClientInfoById(int clientId)
        {
            try
            {
                var response = _clientManager.GetClientInfoById(clientId).Result;

                if (response.response.Success)
                    return await Task.FromResult(response.model);
                else
                    return StatusCode(400, response.response.ErrorMessage);
            }
            catch (Exception ex)
            {

                return StatusCode(400, ex.Message);
            }

        }

        [HttpGet("getClientInfoByName/")]
        public async Task<ActionResult<ClientInfo>> GetClientInfoByName(string keyword)
        {
            try
            {
                var response = _clientManager.GetClientInfoByName(keyword).Result;

                if (response.response.Success)
                    return await Task.FromResult(response.model);
                else
                    return StatusCode(400, response.response.ErrorMessage);
            }
            catch (Exception ex)
            {

                return StatusCode(400, ex.Message);
            }

        }


        [HttpGet("smartsearch/")]
        public async Task<ActionResult<List<ClientAddressResponse>>> SmartSearch(string keyword)
        {
            try
            {
                var response = _clientManager.SmartSearch(keyword).Result;

                if (response.response.Success)
                    return await Task.FromResult(response.model);
                else
                    return StatusCode(400, response.response.ErrorMessage);
            }
            catch (Exception ex)
            {

                return StatusCode(400, ex.Message);
            }

        }

        [HttpGet("getAllClientsWithAddress/")]
        public async Task<ActionResult<List<ClientAddressResponse>>> GetAllClientsWithAddress()
        {
            try
            {
                var response = _clientManager.GetAllClientsWithAddress().Result;

                if (response.response.Success)
                    return await Task.FromResult(response.model);
                else
                    return StatusCode(400, response.response.ErrorMessage);
            }
            catch (Exception ex)
            {

                return StatusCode(400, ex.Message);
            }

        }
        [HttpPost("deleteClientAddress/")]
        public async Task<ActionResult<ResultResponse>> DeleteClientAddress(int? clientId, int? clientAddressId)
        {
            try
            {
                var response = _clientManager.DeleteClientAddress(clientId, clientAddressId).Result;

                if (response.response.Success)
                    return await Task.FromResult(response.model);
                else
                    return StatusCode(400, response.response.ErrorMessage);
            }
            catch (Exception ex)
            {

                return StatusCode(400, ex.Message);
            }

        }

        [HttpPost("deleteClientContact/")]
        public async Task<ActionResult<ResultResponse>> DeleteClientContact(int? clientId, int? clientContactId)
        {
            try
            {
                var response = _clientManager.DeleteClientContact(clientId, clientContactId).Result;

                if (response.response.Success)
                    return await Task.FromResult(response.model);
                else
                    return StatusCode(400, response.response.ErrorMessage);
            }
            catch (Exception ex)
            {

                return StatusCode(400, ex.Message);
            }

        }


    }
}
