using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericDotNetCoreRestApi.Model.Response
{
    public class ResponseResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
