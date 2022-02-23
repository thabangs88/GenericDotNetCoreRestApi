using GenericDotNetCoreRestApi.Model.Request;
using GenericDotNetCoreRestApi.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericDotNetCoreRestApi.Interface
{
    public interface IUserManager
    {
        Task<(ResponseResult response, ResultResponse model)> Create(UserInfo user);
        Task<(ResponseResult response, ResultResponse model)> Update(UserInfo user);
        Task<(ResponseResult response, string model)> ResetPassword(string username);
        Task<(ResponseResult response, ResultResponse model)> ChangePassword(string username, string password);
        Task<(ResponseResult response, ResultResponse model)> SetActive(string username);
        Task<(ResponseResult response, ResultResponse model)> SetInActive(string username);
    }
}
