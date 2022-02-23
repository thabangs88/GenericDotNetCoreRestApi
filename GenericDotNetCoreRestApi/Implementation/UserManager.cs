using GenericDotNetCoreRestApi.Interface;
using GenericDotNetCoreRestApi.Model.Context;
using GenericDotNetCoreRestApi.Model.Request;
using GenericDotNetCoreRestApi.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace GenericDotNetCoreRestApi.Implementation
{
    public class UserManager : IUserManager
    {
        private readonly MasterServiceContext masterContext;

        public UserManager(MasterServiceContext context)
        {
            masterContext = context;
        }

        private (bool exist,User user) UserExists(string username)
        {
            var user = masterContext.Users.FirstOrDefault(x => x.Username == username);

            if (user != null)
                return (true, user);

            return (false, null);
        }


        public async Task<(ResponseResult response, ResultResponse model)> Create(UserInfo user)
        {
            try
            {
                var findUser = UserExists(user.Username);

                if (!findUser.exist)
                {
                    var password = !string.IsNullOrEmpty(user.Password) ? user.Password : new Guid().ToString().Take(5).ToString();

                    User newUser = new User()
                    {
                        Username = user.Username,
                        Active = false,
                        FirstName = user.FirstName,
                        Password = password,
                        LastName = user.LastName,
                        AppID = 1,
                        CompanyID = 1
                    };

                    masterContext.Users.Add(newUser);
                    masterContext.SaveChanges();

                    return await Task.FromResult(((new ResponseResult() { Success = true },
                        new ResultResponse()
                        {
                        Reference = DateTime.Now.ToString(),
                        Message = "New User has been created",
                        })));

                }
                else
                    return ((new ResponseResult() { Success = false, ErrorMessage = "Username already exists" }, null));
            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        public async Task<(ResponseResult response, ResultResponse model)> Update(UserInfo user)
        {
            try
            {
                var findUser = UserExists(user.Username);

                if (findUser.exist)
                {
                    findUser.user.FirstName = user.FirstName;
                    findUser.user.LastName = user.LastName;

                    masterContext.SaveChanges();

                    return await Task.FromResult(((new ResponseResult() { Success = true },
              new ResultResponse()
              {
                  Reference = DateTime.Now.ToString(),
                  Message = "User details has been updated"
              })));
                }
                else
                    return ((new ResponseResult() { Success = false, ErrorMessage = "User does not exists" }, null));
            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        public async Task<(ResponseResult response, ResultResponse model)> ChangePassword(string username, string password)
        {
            try
            {
                var user = UserExists(username);

                if (user.exist)
                {
                    user.user.Password = password;
                    masterContext.SaveChanges();

                    return await Task.FromResult(((new ResponseResult() { Success = true },
                             new ResultResponse()
                             {
                                 Reference = DateTime.Now.ToString(),
                                 Message = "User password has been updated"
                             })));
                }
                else
                    return ((new ResponseResult() { Success = false, ErrorMessage = "User does not exist" }, null));

            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        public async Task<(ResponseResult response, ResultResponse model)> SetInActive(string username)
        {
            try
            {
                var findUser = UserExists(username);

                if (findUser.exist)
                {
                    findUser.user.Active = true;

                    masterContext.SaveChanges();
                    return await Task.FromResult(((new ResponseResult() { Success = true },
                     new ResultResponse()
                     {
                         Reference = DateTime.Now.ToString(),
                         Message = "User has been set Active"
                     })));

                }
                else
                    return ((new ResponseResult() { Success = false, ErrorMessage = "User does not exists" }, null));
            
            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        public async Task<(ResponseResult response, string model)> ResetPassword(string username)
        {
            try
            {
                var findUser = UserExists(username);

                if (findUser.exist)
                {
                    masterContext.SaveChanges();
                    return await Task.FromResult((new ResponseResult() { Success = true }, new Guid().ToString().Take(5).ToString()));

                }
                else
                    return ((new ResponseResult() { Success = false, ErrorMessage = "User does not exists" }, null));
            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        public async Task<(ResponseResult response, ResultResponse model)> SetActive(string username)
        {
            try
            {
                var findUser = UserExists(username);

                if (findUser.exist)
                {
                    findUser.user.Active = false;

                    masterContext.SaveChanges();
                    return await Task.FromResult(((new ResponseResult() { Success = true },
                     new ResultResponse()
                     {
                         Reference = DateTime.Now.ToString(),
                         Message = "User has been set Inactive"
                     })));

                }
                else
                    return ((new ResponseResult() { Success = false, ErrorMessage = "User does not exists" }, null));
            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }


    }
}
