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
    public class LookupManager : ILookupManager
    {
        private readonly MasterContext masterContext;

        public LookupManager(MasterContext context)
        {
            masterContext = context;
        }

        public async Task<(ResponseResult response, List<string> model)> GetClientEmailsAddresses()
        {
            try
            {
                List<string> emails = new List<string>();

                var clients = masterContext.Client;

                foreach (var item in clients)
                {
                    if(!string.IsNullOrEmpty(item.Email))
                        emails.Add(item.Email);
                }

                return await Task.FromResult((new ResponseResult() { Success = true }, emails));
            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        public async Task<(ResponseResult response, List<string> model)> GetContactNumbers()
        {
            try
            {
                List<string> contacts = new List<string>();

                var clients = masterContext.ClientContacts;

                foreach (var item in clients)
                {
                    if (!string.IsNullOrEmpty(item.ContactNo))
                        contacts.Add(item.ContactNo);
                }

                return await Task.FromResult((new ResponseResult() { Success = true }, contacts));
            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        public async Task<(ResponseResult response, List<string> model)> GetAddresses()
        {
            try
            {
                List<string> addresses = new List<string>();

                var contacts = masterContext.ClientAddress;

                foreach (var item in contacts)
                {
                    var address = $"{item?.Address}, {item?.Suburb}, {item?.Province}, {item?.Code}";
                    addresses.Add(address);
                }

                return await Task.FromResult((new ResponseResult() { Success = true }, addresses));
            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }
    }
}
