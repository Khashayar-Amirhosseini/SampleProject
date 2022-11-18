
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using SampleProject.Infrastructure;
using SampleProject.Model.Repository;
using System.Net;
using System.Web.Helpers;
using System.Web.Http;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace SampleProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public readonly MyContext _context;
        public IConfiguration configuration;
        public LoginController(MyContext myContext, IConfiguration configuration)
        {
            this._context = myContext;
            this.configuration = configuration;
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<string> Login([FromForm] string userName, [FromForm] string password)
        {
            try
            {
                var client = await _context.Client.Where(c => c.Email == userName.ToLower()).FirstAsync();
                var clientPassword = await _context.Password.Where(p => p.Clients == client).FirstAsync();
                string pass = password + clientPassword.PrimaryKey;
               
                if (Crypto.VerifyHashedPassword(clientPassword.Passwords,pass ))
                {
                    return  JwtManager.GenerateToken(client);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
                }
            }
            catch(Exception e)
            {
                   throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            
            
        }
    }
}
