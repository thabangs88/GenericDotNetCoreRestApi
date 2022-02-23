using GenericDotNetCoreRestApi.Options;
using GenericDotNetCoreRestApi.Model;
using GenericDotNetCoreRestApi.Model.Response;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using GenericDotNetCoreRestApi.Model.Context;
using GenericDotNetCoreRestApi.Model.Request;
using GenericDotNetCoreRestApi.Extension;

namespace GenericDotNetCoreRestApi.Controllers
{
    [ApiExplorerSettings(GroupName = "Authenticate")]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {

        /// 
        private readonly MasterServiceContext _context;
        private TokenOptions TokenOptions { get; }

        public AuthenticationController(MasterServiceContext context,IOptions<TokenOptions> tokenOptions)
        {
            _context = context;
            TokenOptions = tokenOptions.Value;
        }

        /// <summary>
        /// Creates a Login Session for a specific User and a Specific Login Type
        /// </summary>

        [HttpPost("[action]")]
        [Produces("application/json")]
        [Consumes("application/json")]

        public ActionResult<TokenResponse> Token([FromBody] TokenRequest request)
        {
            if (request == null)
            {
                return new TokenResponse();
            }
            else
            {
                var user = _context.Users.FirstOrDefault(x => x.Username == request.Username);

                if (user == null)
                {
                    return StatusCode(400, "User does not exists, could not process request");
                }
                else
                {
                    if ((user.Active) && (user.Password == request.Password))
                    {
                        var company = _context.Companies.Find(user.CompanyID);
                        var app = _context.Apps.Find(user.AppID);
                        if ((company == null) || (app == null))
                        {
                            return StatusCode(400, "Could not process request, user infornation is invalid");
                        }
                        else
                        {

                            if ((company.Active) && (app.Active))
                            {

                                var claims = new[] {
                                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                                    new Claim(JwtRegisteredClaimNames.GivenName, company.Name)
                                };

                                var token = new JwtSecurityToken(
                                    audience: TokenOptions.Audience,
                                    issuer: TokenOptions.Issuer,
                                    claims: claims,
                                    expires: TokenOptions.GetExpiration(),
                                    signingCredentials: TokenOptions.GetSigningCredentials());

                                TokenResponse tokenResponse = new TokenResponse
                                {
                                    token_type = TokenOptions.Type,
                                    access_token = new JwtSecurityTokenHandler().WriteToken(token),
                                    expires_in = (int)TokenOptions.ValidFor.TotalSeconds
                                };

                                return tokenResponse;
                            }
                            else
                            {
                                return StatusCode(400, "Could not process request, User is not active");
                            }
                
                        }

           ;
                    }
                    else
                    {
                        return StatusCode(400, "Could not process request, User password is incorrect");
                    }
                }

            }
        }
    }
}
