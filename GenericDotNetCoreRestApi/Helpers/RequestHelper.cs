using GenericDotNetCoreRestApi.Model.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenericDotNetCoreRestApi.Helpers
{
    public class RequestHelper
    {
        public static async Task<(ResponseResult response, string json)> GetUsingToken(string requestEndpoint, string token, string headerName, object obj)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestEndpoint);

                request.Headers.Add(headerName, token);
                request.ContentType = "application/json";
                request.Method = "GET";

                if (obj != null)
                {
                    using var streamWriter = new StreamWriter(request.GetRequestStream());

                    streamWriter.Write(JsonConvert.SerializeObject(obj));
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                else
                {
                    request.ContentLength = 0;
                }

                WebResponse response = request.GetResponse();
                var streamReader = new StreamReader(response.GetResponseStream());

                return await Task.FromResult((new ResponseResult() { Success = true }, streamReader.ReadToEnd()));
            }
            catch (WebException ex)
            {

                using var errorResponse = (HttpWebResponse)ex.Response;
                using var reader = new StreamReader(errorResponse.GetResponseStream());
                return (new ResponseResult() { Success = false, ErrorMessage = reader.ReadToEnd() }, null);
            }
            catch (Exception ex)
            {
                return (new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null);
            }

        }

        public static async Task<(ResponseResult response, string json)> PostUsingToken(string requestEndpoint, string token, string headerName, object obj)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestEndpoint);

                request.Headers.Add(headerName, token);
                request.ContentType = "application/json";
                request.Method = "POST";

                if (obj != null)
                {
                    using var streamWriter = new StreamWriter(request.GetRequestStream());

                    streamWriter.Write(JsonConvert.SerializeObject(obj));
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                WebResponse response = request.GetResponse();
                var streamReader = new StreamReader(response.GetResponseStream());

                return await Task.FromResult((new ResponseResult() { Success = true }, streamReader.ReadToEnd()));
            }
            catch (WebException ex)
            {

                using var errorResponse = (HttpWebResponse)ex.Response;
                using var reader = new StreamReader(errorResponse.GetResponseStream());
                return (new ResponseResult() { Success = false, ErrorMessage = reader.ReadToEnd() }, null);
            }
            catch (Exception ex)
            {
                return (new ResponseResult() { Success = false, ErrorMessage = ex.Message }, string.Empty);
            }

        }

        public static async Task<string> PutUsingToken(string requestEndpoint, string token, string headerName, object obj)
        {

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestEndpoint);


                request.Headers.Add(headerName, token);
                request.ContentType = "application/json";
                request.Method = "PUT";

                if (obj != null)
                {
                    using var streamWriter = new StreamWriter(request.GetRequestStream());
                    streamWriter.Write(JsonConvert.SerializeObject(obj));
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                WebResponse response = request.GetResponse();
                var streamReader = new StreamReader(response.GetResponseStream());

                return await streamReader.ReadToEndAsync();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static async Task<(ResponseResult response, string json)> Get(string requestEndpoint)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestEndpoint);

                request.ContentType = "application/json";
                request.Method = "GET";
                request.ContentLength = 0;


                WebResponse response = request.GetResponse();
                var streamReader = new StreamReader(response.GetResponseStream());

                return await Task.FromResult((new ResponseResult() { Success = true }, streamReader.ReadToEnd()));

            }
            catch (WebException ex)
            {

                using var errorResponse = (HttpWebResponse)ex.Response;
                using var reader = new StreamReader(errorResponse.GetResponseStream());
                return (new ResponseResult() { Success = false, ErrorMessage = reader.ReadToEnd() }, null);
            }
            catch (Exception ex)
            {
                return (new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null);
            }

        }

     
        public static async Task<string> Post(string requestUri, string json, bool statuscode)
        {

            using var client = new HttpClient();
            var request = await client.PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json"));
            var content = await request.Content.ReadAsStringAsync();

            if (statuscode)
            {
                return request.StatusCode.ToString();
            }
            else
            {
                return content;
            }
        }

        public static async Task<HttpResponseMessage> PostReturnResponseMessage(string requestUri, string json)
        {
            using var client = new HttpClient();
            var request = await client.PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json"));

            return request;
        }
    }
}
