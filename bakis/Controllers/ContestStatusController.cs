using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE.Models;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace BE.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContestStatusController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ContestStatusController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/ContestStatus
        [HttpGet]
        public IEnumerable<ContestStatus> GetContestStatuses()
        {
            return _context.ContestStatuses;
        }

        // GET: api/ContestStatus/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContestStatus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contestStatus = await _context.ContestStatuses.FindAsync(id);

            if (contestStatus == null)
            {
                return NotFound();
            }

            return Ok(contestStatus);
        }

        // PUT: api/ContestStatus/5
        [Authorize(Policy = "manageClassifiers")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContestStatus([FromRoute] int id, [FromBody] ContestStatus contestStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contestStatus.ContestStatusId)
            {
                return BadRequest("Užklausos būsenos ID nesutampa su formoje esančiu būsenos ID");
            }

            _context.Entry(contestStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContestStatusExists(id))
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

        // POST: api/ContestStatus
        [Authorize(Policy = "manageClassifiers")]
        [HttpPost]
        public async Task<IActionResult> PostContestStatus([FromBody] ContestStatus contestStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ContestStatuses.Add(contestStatus);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContestStatus", new { id = contestStatus.ContestStatusId }, contestStatus);
        }

        // DELETE: api/ContestStatus/5
        [Authorize(Policy = "manageClassifiers")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContestStatus([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contestStatus = await _context.ContestStatuses.FindAsync(id);
            if (contestStatus == null)
            {
                return NotFound();
            }

            var contests = _context.Contests.Where(l => l.ContestStatusId == id).Select(l => l.ContestId).FirstOrDefault().ToString();

            if (contests != "0")
            {
                return BadRequest("Konkurso būsena turi susijusių įrašų ir negali būti ištrinta");
            }

            _context.ContestStatuses.Remove(contestStatus);
            await _context.SaveChangesAsync();

            return Ok(contestStatus);
        }

        private bool ContestStatusExists(int id)
        {
            return _context.ContestStatuses.Any(e => e.ContestStatusId == id);
        }
    }
}