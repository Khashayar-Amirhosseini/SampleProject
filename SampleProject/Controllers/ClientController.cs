using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleProject.Model.Entity;
using SampleProject.Model.Repository;

namespace SampleProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly MyContext myContext;
        public ClientController(MyContext myContext)
        {
            this.myContext = myContext;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
             var res=  await myContext.Client.ToArrayAsync();
                return Ok(res);
            
        }
        [HttpGet("id")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var client = await myContext.Client.FindAsync(id);
            return client == null ? NotFound() : Ok(client);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Exception),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(Client client)
        {
            
            client.Email = client.Email.ToLower();

            await myContext.Client.AddAsync(client);
            try
            {
                await myContext.SaveChangesAsync();
          
                return CreatedAtAction(nameof(GetById), new { id = client.Id }, client);
            }
            catch(Exception e)
            {
                if (e.InnerException.Message.ToLower().Contains("unique"))
                {
                    return BadRequest("Email is Already Used");
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }
            
        }
        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> update(long id,Client client)
        {
            if (id != client.Id) return BadRequest();
            myContext.Entry(client).State = EntityState.Modified;
            try
            {
                await myContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception e)
            {
                if (e.InnerException.Message.ToLower().Contains("unique"))
                {
                    return BadRequest("Email is Already Used");
                }
                else
                {
                    return BadRequest(e.Message);
                }
            }

        }
        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var clientToDelete = await myContext.Client.FindAsync(id);
            if(clientToDelete==null) return NotFound();
            myContext.Remove(clientToDelete);
            await myContext.SaveChangesAsync();
            return NoContent();

        }
        
    }
}
