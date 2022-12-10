
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleProject.Model.Entity;
using SampleProject.Model.Repository;
using System.Collections;
using System.Web.Http.Filters;
using SampleProject.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SampleProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="manager")]
    public class RoleController : ControllerBase
    {
        private readonly MyContext myContext;
        public RoleController (MyContext myContext)
        {
            this.myContext=myContext;

        }
        
        [HttpGet("id")]
        [ProducesResponseType(typeof(List<Role>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<IActionResult> GetRoles(long clientId)
        {
            try
            {
                var roles = await myContext.RoleClients.Include(r=>r.Role).Where(rc => rc.ClientId == clientId).ToListAsync();

                List<string> clientRoles = new List<string>();

                foreach(var r in roles)
                {
                    clientRoles.Add(r.Role.Name);
                }
                return Ok(clientRoles);
                    
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromForm] long clientId, [FromForm] string roleName)
        {
            try
            {
                var client = await myContext.Client.FindAsync(clientId);
                var role = await myContext.Role.Where(r => r.Name == roleName).FirstOrDefaultAsync();
                if (role!=null)
                {
                    var exRol=await myContext.Role.FirstOrDefaultAsync(r => r.Name == roleName);
                    var roleClient = await myContext.RoleClients.AnyAsync(r => r.Role.Name == roleName);
                    
                    if(!roleClient){
                        await myContext.RoleClients.AddAsync(new RoleClient { client = client, Role =exRol });
                        myContext.SaveChanges();
                    }
                   
                    return Ok("role "+roleName+" has been assigned to client ID "+clientId);
                }

                Role newRole = new()
                {
                    Name = roleName
                };

                await myContext.Role.AddAsync(newRole);
                await myContext.SaveChangesAsync();
                await myContext.RoleClients.AddAsync(new RoleClient
                {
                    Role=newRole,
                    client=client,
                });;

                myContext.SaveChanges();
                return Ok("role " + roleName + " has been assigned to client ID " + clientId);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
