using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bakis.Models;
using Microsoft.AspNetCore.Authorization;

namespace bakis.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DutiesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public DutiesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Duties
        [HttpGet]
        public IEnumerable<Duty> GetDuties()
        {
            return _context.Duties;
        }

        // GET: api/Duties/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDuty([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var duty = await _context.Duties.FindAsync(id);

            if (duty == null)
            {
                return NotFound();
            }

            return Ok(duty);
        }

        // PUT: api/Duties/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDuty([FromRoute] int id, [FromBody] Duty duty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Jūs neįvedėte duomenų.");
            }

            if (id != duty.DutyId)
            {
                return BadRequest();
            }

            _context.Entry(duty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DutyExists(id))
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

        // POST: api/Duties
        [HttpPost]
        public async Task<IActionResult> PostDuty([FromBody] Duty duty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }

            if(duty.Title == null)
            {
                return BadRequest("Jūs neįvedėte duomenų.");
            }

            _context.Duties.Add(duty);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDuty", new { id = duty.DutyId }, duty);
        }

        // DELETE: api/Duties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDuty([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var duty = await _context.Duties.FindAsync(id);
            if (duty == null)
            {
                return NotFound();
            }

            var employeeDuties = _context.EmployeeDuties.Where(l => l.DutyId == id).Select(l => l.EmployeeDutyId).FirstOrDefault().ToString();

            if (employeeDuties != "0")
            {
                return BadRequest("Negalima ištrinti, nes yra šios pareigos yra priskirtos bent vienam darbuotojui.");
            }

            _context.Duties.Remove(duty);
            await _context.SaveChangesAsync();

            return Ok(duty);
        }

        private bool DutyExists(int id)
        {
            return _context.Duties.Any(e => e.DutyId == id);
        }
    }
}