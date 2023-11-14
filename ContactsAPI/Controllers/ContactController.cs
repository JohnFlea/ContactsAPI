using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Xml.Linq;


namespace ContactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly MyDbContext _myDbContext;

        public ContactController(MyDbContext myDbContext) => _myDbContext = myDbContext;

        /// <summary>
        /// Retourne la liste de tous les Contact
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<Contact>> Get()
            => await _myDbContext.Contacts.ToListAsync();

        /// <summary>
        /// Retourne un Contact à l'aide de son Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _myDbContext.Contacts.FindAsync(id);
            return contact == null ? NotFound() : Ok(contact);
        }

        /// <summary>
        /// Créer un nouveau Contact
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Contact contact)
        {
            await _myDbContext.Contacts.AddAsync(contact);
            await _myDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = contact.Id }, contact);
        }

        /// <summary>
        /// Modifier un contact existant
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Contact contact)
        {
            if (id != contact.Id) return BadRequest();

            _myDbContext.Entry(contact).State = EntityState.Modified;
            await _myDbContext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Supprimer un contact existant
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var contactToDelete = await _myDbContext.Contacts.FindAsync(id);
            if (contactToDelete == null) return NotFound();

            _myDbContext.Contacts.Remove(contactToDelete);
            await _myDbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
