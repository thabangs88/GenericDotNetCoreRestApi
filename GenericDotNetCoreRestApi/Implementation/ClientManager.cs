using GenericDotNetCoreRestApi.Helpers;
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
    public partial class ClientManager : IClientManager
    {
        private readonly MasterContext masterContext;

        public ClientManager(MasterContext context)
        {
            masterContext = context;
        }

        private ResponseResult ValidateClientInfo(ClientInfo client)
        {
            if (client == null)
                return ((new ResponseResult() { Success = false, ErrorMessage = "Invalid request" }));

            if (string.IsNullOrEmpty(client.FirstName) || string.IsNullOrEmpty(client.LastName) || string.IsNullOrEmpty(client.Title) || string.IsNullOrEmpty(client.Email)) 
                return ((new ResponseResult() { Success = false, ErrorMessage = "Please provide all client information" }));

            var isEmailValid = ValidationHelper.IsValidEmail(client.Email);
            if(!isEmailValid)
                return ((new ResponseResult() { Success = false, ErrorMessage = "Please enter a valid email" }));

            if (string.IsNullOrEmpty(client.IdNumber))
                return ((new ResponseResult() { Success = false, ErrorMessage = "Client IDNumber was not found" }));

            if (client.Addresses == null || client.Addresses.Count() ==0 )
                return ((new ResponseResult() { Success = false, ErrorMessage = "No client address was found" }));

            if (client.Contacts == null || client.Contacts.Count() == 0)
                return ((new ResponseResult() { Success = false, ErrorMessage = "No client contacts was found" }));

            return ((new ResponseResult() { Success = true}));
        }

        private void UpdateClientAddressAndContacts(ClientInfo client)
        {
            foreach (var item in client.Contacts)
            {
                var contact = masterContext.ClientContacts.FirstOrDefault(x => x.ClientId == client.ClientId &
                x.ContactNo == item.ContactNo & x.Type == item.Type);

                if(contact ==null)
                {
                    ClientContacts clientContacts = new ClientContacts()
                    {
                        ClientId = client.ClientId,
                        ContactNo = item.ContactNo,
                        Type = item.Type,

                    };
                    masterContext.ClientContacts.Add(clientContacts);
                    masterContext.SaveChanges();
                }
            };

            foreach (var item in client.Addresses)
            {
                var address = masterContext.ClientAddress.FirstOrDefault(x => x.ClientId == client.ClientId & x.Address == item.Address);

                if (address == null)
                {
                    ClientAddress clientAddress = new ClientAddress()
                    {
                        Address = item.Address,
                        Code = item.Code,
                        Province = item.Province,
                        Suburb = item.Suburb,
                        ClientId = client.ClientId
                    };
                    masterContext.ClientAddress.Add(clientAddress);
                    masterContext.SaveChanges();
                }
            };
        }

        public async Task<(ResponseResult, ResultResponse)> CreateClient(ClientInfo client)
        {
            try
            {
                var validateUserInfo = ValidateClientInfo(client);

                if(validateUserInfo.Success != true)
                    return ((new ResponseResult() { Success = false, ErrorMessage = validateUserInfo .ErrorMessage}, null));

                var clientExists = masterContext.Client.FirstOrDefault(x => x.IdNumber == client.IdNumber);

                if (clientExists != null)
                    return ((new ResponseResult() { Success = false, ErrorMessage = "Client with same IdNumber already exists" }, null));
                else 
                {
                    //Add Client
                    Client c = new Client()
                    {
                        Email = client.Email,
                        IdNumber = client.IdNumber,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Gender = client.Gender,
                        Title = client.Title,
                    };

                    masterContext.Client.Add(c);
                    masterContext.SaveChanges();

                    client.ClientId = c.ClientId;

                    UpdateClientAddressAndContacts(client);

                    return await Task.FromResult((new ResponseResult() { Success = true }, new ResultResponse()
                    {
                        Message = "Client successfully saved",
                        Reference = c.ClientId.ToString()
                    }));
                }
            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        public async Task<(ResponseResult, List<ClientAddressResponse>)> GetAllClientsWithAddress()
        {
            try
            {
                List<ClientAddressResponse> result = new List<ClientAddressResponse>();

                var clients = masterContext.Client.ToList();

                foreach (var client in clients)
                {
                    result.Add(new ClientAddressResponse()
                    {
                        ClientId = client.ClientId,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Title = client.Title,
                        FullName = $"{client.Title} {client.FirstName} {client.LastName}",
                        Addresses = GetClientAddressInfo(client.ClientId)
                });
                }

                return await Task.FromResult((new ResponseResult() { Success = true }, result));

            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        private List<AddressInfo> GetClientAddressInfo(int? clientId)
        {
            var clientAddresses = masterContext.ClientAddress.Where(x => x.ClientId == clientId);

            var addressInfo = new List<AddressInfo>();

            foreach (var item in clientAddresses)
            {
                addressInfo.Add(new AddressInfo()
                {
                    ID = item.ID,
                    Address = item.Address,
                    Code = item.Code,
                    Province = item.Province,
                    Suburb = item.Suburb,
                    AddressId = item.ID
                });
            }

            return addressInfo;
        }

        private List<ContactInfo> GetClientAContactInfo(int? clientId)
        {
            var clientContacts = masterContext.ClientContacts.Where(x => x.ClientId == clientId);

            var contactInfo = new List<ContactInfo>();

            foreach (var item in clientContacts)
            {
                contactInfo.Add(new ContactInfo()
                {
                    ID = item.ID,
                    Type = item.Type,
                    ContactNo = item.ContactNo
                });
            }

            return contactInfo;
        }

        public async Task<(ResponseResult, ClientInfo)> GetClientInfoById(int clientId)
        {
            try
            {
                var client = masterContext.Client.FirstOrDefault(x => x.ClientId == clientId);

                if (client != null)
                {
                    return await Task.FromResult((new ResponseResult() { Success = true }, new ClientInfo()
                    {
                        ClientId = client.ClientId,
                        Addresses = GetClientAddressInfo(client.ClientId),
                        Contacts = GetClientAContactInfo(client.ClientId),
                        Email = client.Email,
                        FirstName = client.FirstName,
                        Gender = client.Gender,
                        IdNumber = client.IdNumber,
                        LastName = client.LastName,
                        Title = client.Title
                    }));
                }
                else
                {
                    return ((new ResponseResult() { Success = false, ErrorMessage = "Client does not exist" }, null));
                }
            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        public async Task<(ResponseResult, ClientInfo)> GetClientInfoByName(string keyword)
        {
            try
            {
                var client = masterContext.Client.FirstOrDefault(x => $"{x.FirstName} {x.LastName}".Contains(keyword));

                if (client != null)
                {
                    return await Task.FromResult((new ResponseResult() { Success = true }, new ClientInfo()
                    {
                        ClientId = client.ClientId,
                        Addresses = GetClientAddressInfo(client.ClientId),
                        Contacts = GetClientAContactInfo(client.ClientId),
                        Email = client.Email,
                        FirstName = client.FirstName,
                        Gender = client.Gender,
                        IdNumber = client.IdNumber,
                        LastName = client.LastName,
                        Title = client.Title
                    }));
                }
                else
                {
                    return ((new ResponseResult() { Success = false, ErrorMessage = "Client does not exist" }, null));
                }
            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        public async Task<(ResponseResult, List<ClientAddressResponse>)> SmartSearch(string keyword)
        {
            try
            {
                var query = from c in masterContext.Client
                            join contact in masterContext.ClientContacts
                            on c.ClientId equals contact.ClientId
                            join address in masterContext.ClientAddress
                            on c.ClientId equals address.ClientId
                            select new
                            {
                                Client = c,
                                Contact = contact,
                                Address = address
                            };

                keyword = keyword.ToLower();

                query = query.Where(x => $"{x.Client.Title} {x.Client.FirstName} {x.Client.LastName}".ToLower().Contains(keyword) || x.Address.Province.ToLower().Contains(keyword) || x.Address.Suburb.ToLower().Contains(keyword) || x.Address.Address.ToLower().Contains(keyword));

                List<ClientAddressResponse> result = new List<ClientAddressResponse>();

                foreach (var item in query)
                {
                    if (!result.Exists(x => x.ClientId == item.Client.ClientId))
                    {
                        result.Add(new ClientAddressResponse()
                        {
                            ClientId = item.Client.ClientId,
                            FirstName = item.Client.FirstName,
                            LastName = item.Client.LastName,
                            Title = item.Client.Title,
                            FullName = $"{item.Client.Title} {item.Client.FirstName} {item.Client.LastName}",
                            Addresses = GetClientAddressInfo(item.Client.ClientId)
                        });
                    }
                }

                return await Task.FromResult((new ResponseResult() { Success = true }, result));
            }
            catch (Exception ex)
            {
                    return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        public async Task<(ResponseResult, ResultResponse)> UpdateClientInfo(ClientInfo client)
        {
            try
            {
                var validateUserInfo = ValidateClientInfo(client);

                if (validateUserInfo.Success != true)
                    return ((new ResponseResult() { Success = false, ErrorMessage = validateUserInfo.ErrorMessage }, null));

                var clientExists = masterContext.Client.FirstOrDefault(x => x.IdNumber == client.IdNumber);

                if (clientExists == null)
                    return ((new ResponseResult() { Success = false, ErrorMessage = "Client does not exists" }, null));

                clientExists.FirstName = client.FirstName;
                clientExists.LastName = client.LastName;
                clientExists.Gender = client.Gender;
                clientExists.Email = client.Email;
                clientExists.Title = client.Title;

                masterContext.SaveChanges();

                UpdateClientAddressAndContacts(client);

                return await Task.FromResult((new ResponseResult() { Success = true }, new ResultResponse()
                {
                    Message = "Client successfully saved",
                    Reference = clientExists.ClientId.ToString()
                }));

            }
            catch (Exception ex)
            {
                    return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        public async Task<(ResponseResult, ResultResponse)> DeleteClientAddress(int? clientId, int? clientAddressId)
        {
            try
            {
                var address = masterContext.ClientAddress.FirstOrDefault(x => x.ClientId == clientId && x.ID == clientAddressId);
                if (address != null)
                {
                    masterContext.Remove(address);
                    masterContext.SaveChanges();

                    return await Task.FromResult(((new ResponseResult() { Success = true }, new ResultResponse()
                    {
                        Message = "Client Address has been removed",
                        Reference = DateTime.Now.ToString()
                    })));
                }
                else
                    return (new ResponseResult() { Success = false, ErrorMessage = "Contact Address not found" }, null);

            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }

        public async Task<(ResponseResult, ResultResponse)> DeleteClientContact(int? clientId, int? clientContactId)
        {
            try
            {
                var contact = masterContext.ClientContacts.FirstOrDefault(x => x.ClientId == clientId && x.ID == clientContactId);
                if (contact != null)
                {
                    masterContext.Remove(contact);
                    masterContext.SaveChanges();

                    return await Task.FromResult(((new ResponseResult() { Success = true },
                    new ResultResponse() 
                    {                  
                        Reference = DateTime.Now.ToString(),
                        Message = "Client Address has been removed "
                    })));
                }
                else
                    return ((new ResponseResult() { Success = false, ErrorMessage = "Contact contact not found" }, null));
            }
            catch (Exception ex)
            {
                return ((new ResponseResult() { Success = false, ErrorMessage = ex.Message }, null));
            }
        }
    }
}
