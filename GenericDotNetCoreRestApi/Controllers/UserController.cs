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
    [Route("api/user/")]
    [ApiExplorerSettings(GroupName = "User Manager")]
    [Authorize]

    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserManager _userManager;

        public UserController(ILogger<UserController> logger, MasterServiceContext context)
        {
            _logger = logger;
            _userManager = new UserManager(context);
        }


        [HttpPost("create/")]
        public async Task<ActionResult<ResultResponse>> Create(UserInfo model)
        {
            try
            {
                var response = _userManager.Create(model).Result;

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
        [HttpPost("update/")]
        public async Task<ActionResult<ResultResponse>> Update(UserInfo model)
        {
            try
            {
                var response = _userManager.Update(model).Result;

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

        [HttpPost("resetPassword/")]
        public async Task<ActionResult<string>> ResetPassword(string username)
        {
            try
            {
                var response = _userManager.ResetPassword(username).Result;

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

        [HttpPost("changePassword/")]
        public async Task<ActionResult<ResultResponse>> ChangePassword(string username, string password)
        {
            try
            {
                var response = _userManager.ChangePassword(username, password).Result;

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

        [HttpPost("setActive/")]
        public async Task<ActionResult<ResultResponse>> SetActive(string username)
        {
            try
            {
                var response = _userManager.SetActive(username).Result;

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

        [HttpPost("setInActive/")]
        public async Task<ActionResult<ResultResponse>> SetInActive(string username)
        {
            try
            {
                var response = _userManager.SetInActive(username).Result;

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
