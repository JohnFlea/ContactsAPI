﻿using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactsAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {

        private readonly MyDbContext _myDbContext;

        public SkillController(MyDbContext myDbContext) => _myDbContext = myDbContext;

        [HttpGet]
        public async Task<IEnumerable<Skill>> Get() 
            => await _myDbContext.Skills.ToListAsync();

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Skill), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var skill = await _myDbContext.Skills.FindAsync(id);
            return skill == null ? NotFound() : Ok(skill);
        }

        //[HttpGet("search/{name}")]
        //[ProducesResponseType(typeof(Skill), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetByname(string name)
        //{
        //    var skill = await _myDbContext.Skills.SingleOrDefaultAsync(c=> c.Name name);
        //    return skill == null ? NotFound() : Ok(skill);
        //}

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Skill skill)
        {
            await _myDbContext.Skills.AddAsync(skill);
            await _myDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = skill.Id }, skill);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Skill skill)
        {
            if (id != skill.Id) return BadRequest();

            _myDbContext.Entry(skill).State = EntityState.Modified;
            await _myDbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var skillToDelete = await _myDbContext.Skills.FindAsync(id);
            if (skillToDelete == null) return NotFound();

            _myDbContext.Skills.Remove(skillToDelete);
            await _myDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}