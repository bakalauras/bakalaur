using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bakis.Models;

namespace bakis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingTimeRegistersController : ControllerBase
    {
        private readonly ProjectContext _context;

        public WorkingTimeRegistersController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/WorkingTimeRegisters
        [HttpGet]
        public IEnumerable<WorkingTimeRegister> GetWorkingTimeRegisters()
        {
            return _context.WorkingTimeRegisters;
        }

        // GET: api/WorkingTimeRegisters/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkingTimeRegister([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workingTimeRegister = await _context.WorkingTimeRegisters.FindAsync(id);

            if (workingTimeRegister == null)
            {
                return NotFound();
            }

            return Ok(workingTimeRegister);
        }

        // PUT: api/WorkingTimeRegisters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkingTimeRegister([FromRoute] int id, [FromBody] WorkingTimeRegister workingTimeRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != workingTimeRegister.WorkingTimeRegisterId)
            {
                return BadRequest();
            }

            _context.Entry(workingTimeRegister).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkingTimeRegisterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WorkingTimeRegisters
        [HttpPost]
        public async Task<IActionResult> PostWorkingTimeRegister([FromBody] WorkingTimeRegister workingTimeRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.WorkingTimeRegisters.Add(workingTimeRegister);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkingTimeRegister", new { id = workingTimeRegister.WorkingTimeRegisterId }, workingTimeRegister);
        }

        // DELETE: api/WorkingTimeRegisters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkingTimeRegister([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workingTimeRegister = await _context.WorkingTimeRegisters.FindAsync(id);
            if (workingTimeRegister == null)
            {
                return NotFound();
            }

            _context.WorkingTimeRegisters.Remove(workingTimeRegister);
            await _context.SaveChangesAsync();

            return Ok(workingTimeRegister);
        }

        private bool WorkingTimeRegisterExists(int id)
        {
            return _context.WorkingTimeRegisters.Any(e => e.WorkingTimeRegisterId == id);
        }
    }
}