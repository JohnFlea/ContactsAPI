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
        /// Get list of all contacts
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Description = "This method return all existing contacts")]
        public async Task<IEnumerable<Contact>> Get()
        { 
            return await _myDbContext.Contacts.Include(p => p.Skills).ToListAsync();
        }

        /// <summary>
        /// Get a contact using his ID
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Description = "This method return a contact using his ID")]
        [ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _myDbContext.Contacts.Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == id);
            return contact == null ? NotFound() : Ok(contact);
        }

        /// <summary>
        /// Create new contact
        /// </summary>
        /// <param name="contact">Contact to add</param>
        /// /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Description = "This method create a new contact")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] Contact contact)
        {
            await _myDbContext.Contacts.AddAsync(contact);
            await _myDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = contact.Id }, contact);
        }

        /// <summary>
        /// Modify existing contact
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// <param name="contact">Contact with modifications</param>
        /// /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Description = "This method modify an existing contact")]
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
        /// Delete existing contact
        /// </summary>
        /// <param name="id">Id of the contact</param>
        /// /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Description = "This method delete an existing contact")]
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



        /// <summary>
        /// Add a skill to a contact
        /// </summary>
        /// <param name="contactId">Id of the contact</param>
        /// <param name="skillId">Id of the skill</param>
        /// <returns></returns>
        [HttpPut("{contactId}/Skill/{skillId}")]
        [SwaggerOperation(Description = "This method add a skill to a contact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddSkill(int contactId, int skillId)
        {
            var contact = await _myDbContext.Contacts.FindAsync(contactId);
            if (contact == null) return NotFound($"No Contact with ID: {contactId}");

            var skill = await _myDbContext.Skills.FindAsync(skillId);
            if (skill == null) return NotFound($"No Skill with ID: {skillId}");

            contact.Skills.Add(skill);

            _myDbContext.Entry(contact).State = EntityState.Modified;
            await _myDbContext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Remove skill from a contact
        /// </summary>
        /// <param name="contactId">Id of the contact</param>
        /// <param name="skillId">Id of the skill</param>
        /// <returns></returns>
        [HttpDelete("{contactId}/Skill/{skillId}")]
        [SwaggerOperation(Description = "This method remove a skill from a contact")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveSkill(int contactId, int skillId)
        {
            var contact = await _myDbContext.Contacts.Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == contactId);
            if (contact == null) return NotFound($"No contact with ID: {contactId}");

            var skill = contact.Skills.Where(x => x.Id == skillId).FirstOrDefault();
            if (skill == null) return NotFound($"No Skill with ID: {skillId} found in the Contact skill's list");

            contact.Skills.Remove(skill);

            _myDbContext.Entry(contact).State = EntityState.Modified;
            await _myDbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
