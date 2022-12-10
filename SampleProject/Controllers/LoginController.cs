
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SampleProject.Model.Entity;
using SampleProject.Model.Repository;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
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
        public async Task<ActionResult<string>> Login([FromForm] string userName, [FromForm] string password)
        {
            try
            {
                var client = await _context.Client.Include(c=>c.RoleClients).ThenInclude(r=>r.Role).Where(c => c.Email == userName.ToLower()).FirstAsync();
                var clientPassword = await _context.Password.Where(p => p.Clients == client).FirstAsync();
                string pass = password + clientPassword.PrimaryKey;

                if (Crypto.VerifyHashedPassword(clientPassword.Passwords, pass))
                {
                    string token = CreateToken(client);
                    return Ok(token);
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
        private string CreateToken(Client user)
        {
            List<Claim> claims = new List<Claim>
            {
                
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Family),

            };
            if (user.RoleClients != null)
            {
                foreach (RoleClient roleClient in user.RoleClients)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleClient.Role.Name));
                }
            }
            var key = new SymmetricSecurityKey(System.Text.Encoding
                .UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
