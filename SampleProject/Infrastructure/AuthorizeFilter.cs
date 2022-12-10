
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SampleProject.Infrastructure
{
    public class AuthorizeFilter :IActionFilter
    {
        private readonly string[] Roles;
        public AuthorizeFilter(params string[] roles)
        {
            Roles = roles;
        }

        public bool AllowMultiple => throw new NotImplementedException();

        public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            bool authorize = false;
            string token = actionContext.Request.Headers.Authorization.ToString();
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
            throw new NotImplementedException();
        }









        /*
             bool authorize = false;
             string token = actionContext.Request.Headers.Authorization.ToString();
             var tokenHandler = new JwtSecurityTokenHandler();
             var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
             HttpResponseMessage response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
        */




    }
    
}
