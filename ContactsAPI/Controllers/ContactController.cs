using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly MyDbContext _myDbContext;

        public ContactController(MyDbContext myDbContext) => _myDbContext = myDbContext;

        // GET: api/<ContactController>
        [HttpGet("GetContacts")]
        public async Task<IEnumerable<Contact>> GetContacts()
        {
            return await _myDbContext.Contacts.ToListAsync();
        }

        // GET api/<ContactController>/5
        [HttpGet("GetContact/{id}")]
        public async Task<IActionResult> GetContact(int id)
        {
            var contact = await _myDbContext.Contacts.FindAsync(id);
            return contact == null ? NotFound() : Ok(contact);
        }

        // POST api/<ContactController>
        [HttpPost]
        public void Post([FromBody] Contact value)
        {
        }

        // PUT api/<ContactController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Contact value)
        {
        }

        // DELETE api/<ContactController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
