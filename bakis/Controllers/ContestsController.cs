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
    public class ContestsController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ContestsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Contests
        [HttpGet]
        public IEnumerable<Contest> GetContests()
        {
            return _context.Contests;
        }

        // GET: api/Contests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contest = await _context.Contests.FindAsync(id);

            if (contest == null)
            {
                return NotFound();
            }

            return Ok(contest);
        }

        [HttpGet("{id}/files")]
        public async Task<IActionResult> GetContestFiles([FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contest = await _context.Contests.FindAsync(id);

            if (contest == null)
            {
                return NotFound();
            }
            var files = _context.ContestFiles.Where(l => l.ContestId == id);

            return Ok(files);
        }

        // PUT: api/Contests/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContest([FromRoute] int id, [FromBody] Contest contest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contest.ContestId)
            {
                return BadRequest();
            }

            var contestStatus = _context.ContestStatuses.Where(l => l.ContestStatusId == contest.ContestStatusId).Select(l => l.ContestStatusId).FirstOrDefault().ToString();

            var customer = _context.Customers.Where(l => l.CustomerId == contest.CustomerId).Select(l => l.CustomerId).FirstOrDefault().ToString();

            if (contestStatus == "0" || customer == "0")
            {
                return BadRequest();
            }

            _context.Entry(contest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContestExists(id))
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

        // POST: api/Contests
        [HttpPost]
        public async Task<IActionResult> PostContest([FromBody] Contest contest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contestStatus = _context.ContestStatuses.Where(l => l.ContestStatusId == contest.ContestStatusId).Select(l => l.ContestStatusId).FirstOrDefault().ToString();

            var customer = _context.Customers.Where(l => l.CustomerId == contest.CustomerId).Select(l => l.CustomerId).FirstOrDefault().ToString();

            if (contestStatus == "0" || customer == "0")
            {
                return BadRequest();
            }

            _context.Contests.Add(contest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContest", new { id = contest.ContestId }, contest);
        }

        // DELETE: api/Contests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contest = await _context.Contests.FindAsync(id);
            if (contest == null)
            {
                return NotFound();
            }

            var tenders = _context.Tenders.Where(l => l.ContestId == id).Select(l => l.TenderId).FirstOrDefault().ToString();

            var contestFiles = _context.ContestFiles.Where(l => l.ContestId == id).Select(l => l.ContestFileId).FirstOrDefault().ToString();

            if (contestFiles != "0" || tenders != "0")
            {
                return BadRequest();
            }

            _context.Contests.Remove(contest);
            await _context.SaveChangesAsync();

            return Ok(contest);
        }

        private bool ContestExists(int id)
        {
            return _context.Contests.Any(e => e.ContestId == id);
        }
    }
}