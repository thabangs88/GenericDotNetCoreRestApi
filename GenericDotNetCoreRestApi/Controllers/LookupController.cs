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
    [ApiController]
    [EnableCors("AllowsAll")]
    [Route("api/lookup/")]
    [ApiExplorerSettings(GroupName = "Lookup")]
    [Authorize]
    public class LookupController:ControllerBase
    {

        private readonly ILookupManager _lookupManager;

        public LookupController(MasterContext context)
        {
            _lookupManager = new LookupManager(context);
        }

        [HttpGet("emails/")]
        public async Task<ActionResult<List<string>>> GetClientEmailsAddresses()
        {
            try
            {
                var response = _lookupManager.GetClientEmailsAddresses().Result;

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

        [HttpGet("contactNumbers/")]
        public async Task<ActionResult<List<string>>> GetContactNumbers()
        {
            try
            {
                var response = _lookupManager.GetContactNumbers().Result;

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

        [HttpGet("addresses/")]
        public async Task<ActionResult<List<string>>> GetAddresses()
        {
            try
            {
                var response = _lookupManager.GetAddresses().Result;

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
