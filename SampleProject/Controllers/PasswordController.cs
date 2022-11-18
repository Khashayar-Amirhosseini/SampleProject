using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleProject.Model.Entity;
using SampleProject.Model.Repository;
using System.Web.Helpers;

namespace SampleProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly MyContext myContext;
        public PasswordController(MyContext myContext)
        {
            this.myContext = myContext;
        }
        [HttpGet("id")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var Password = await myContext.Password.FindAsync(id);
                return Ok(Password);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] string password, [FromForm] long id)
        {
            try
            {

                
                string salt = Crypto.GenerateSalt();
                Password password1 = new()
                {
                    Clients = myContext.Client.Find(id),
                    PrimaryKey = salt,
                    Passwords=Crypto.HashPassword(password+salt)
                };

                var newPassword = await myContext.Password.Where(p => p.Clients.Id == id).FirstAsync();
                if ( newPassword!= null)
                {
                    newPassword.PrimaryKey = password1.PrimaryKey;
                    newPassword.Passwords = password1.Passwords;
                }
                else
                {
                    await myContext.Password.AddAsync(password1);
                }
                await myContext.SaveChangesAsync();
                return CreatedAtAction(nameof(Get), new { id = password1.Id }, password1);

            }
            catch(Exception e)
            {
                return BadRequest(e.InnerException);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Exception),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> update([FromForm] string newPassword, [FromForm] long clientId)
        {
            try
            {
                var password = await myContext.Password.Where(p => p.Clients.Id == clientId).FirstAsync();
                var salt = Crypto.GenerateSalt();
                var pass = newPassword + salt;
                password.PrimaryKey = salt;
                password.Passwords = Crypto.HashPassword(pass);
                
                await myContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }


        }
        
    }
}
