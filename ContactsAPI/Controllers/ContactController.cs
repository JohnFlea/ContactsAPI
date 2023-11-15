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
        { 
            return await _myDbContext.Contacts.Include(p => p.Skills).ToListAsync();
        }

        /// <summary>
        /// Retourne un Contact à l'aide de son Id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _myDbContext.Contacts.Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == id);
            return contact == null ? NotFound() : Ok(contact);
        }

        /// <summary>
        /// Créer un nouveau Contact
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] Contact contact)
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

        /// <summary>
        /// Ajouter un skill à un contact
        /// </summary>
        [HttpPut("{contactId}/Skill/{skillId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddSkill(int contactId, int skillId)
        {
            var contact = await _myDbContext.Contacts.FindAsync(contactId);
            if (contact == null) return NotFound($"Pas de contact avec l'ID: {contactId}");

            var skill = await _myDbContext.Skills.FindAsync(skillId);
            if (skill == null) return NotFound($"Pas de skill avec l'ID: {skillId}");

            contact.Skills.Add(skill);

            _myDbContext.Entry(contact).State = EntityState.Modified;
            await _myDbContext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Retirer un skill à un contact
        /// </summary>
        [HttpDelete("{contactId}/Skill/{skillId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveSkill(int contactId, int skillId)
        {
            var contact = await _myDbContext.Contacts.Include(p => p.Skills).FirstOrDefaultAsync(p => p.Id == contactId);
            if (contact == null) return NotFound($"Pas de contact avec l'ID: {contactId}");

            var skill = contact.Skills.Where(x => x.Id == skillId).FirstOrDefault();
            if (skill == null) return NotFound($"Pas de skill avec l'ID: {skillId}");

            contact.Skills.Remove(skill);

            _myDbContext.Entry(contact).State = EntityState.Modified;
            await _myDbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
