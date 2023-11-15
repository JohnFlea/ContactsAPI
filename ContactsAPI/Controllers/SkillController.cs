using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace ContactsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly MyDbContext _myDbContext;

        public SkillController(MyDbContext myDbContext) => _myDbContext = myDbContext;

        /// <summary>
        /// Get list of all skills
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Description = "This method return all existing skills")]
        public async Task<IEnumerable<Skill>> Get()
        {
            return await _myDbContext.Skills.Include(p => p.Contacts).ToListAsync();
        }

        /// <summary>
        /// Get a skill using his ID
        /// </summary>
        /// <param name="id">Id of the skill</param>
        /// /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Description = "This method return a skill using his ID")]
        [ProducesResponseType(typeof(Skill), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var skill = await _myDbContext.Skills.FindAsync(id);
            return skill == null ? NotFound() : Ok(skill);
        }

        /// <summary>
        /// Create new skill
        /// </summary>
        /// <param name="skill">Skill to add</param>
        /// /// <returns></returns>
        [HttpPost]
        [SwaggerOperation(Description = "This method create a new skill")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody]Skill skill)
        {
            await _myDbContext.Skills.AddAsync(skill);
            await _myDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = skill.Id }, skill);
        }

        /// <summary>
        /// Modify existing contact
        /// </summary>
        /// <param name="id">Id of the skill</param>
        /// <param name="skill">Skill with modifications</param>
        /// /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Description = "This method modify an existing skill")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Skill skill)
        {
            if (id != skill.Id) return BadRequest();

            _myDbContext.Entry(skill).State = EntityState.Modified;
            await _myDbContext.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete existing skill
        /// </summary>
        /// <param name="id">Id of the skill</param>
        /// /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Description = "This method delete an existing skill")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var skillToDelete =  await _myDbContext.Skills.Include(p => p.Contacts).FirstOrDefaultAsync(p => p.Id == id);
            if (skillToDelete == null) return NotFound();

            if (skillToDelete.Contacts.Count() != 0) return BadRequest("Can't delete because skill is linked to a contact");


            _myDbContext.Skills.Remove(skillToDelete);
            await _myDbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}
