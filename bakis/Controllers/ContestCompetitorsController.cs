using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE.Models;
using Microsoft.AspNetCore.Authorization;

namespace BE.Controllers
{
    [Authorize(Policy = "manageContests")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContestCompetitorsController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ContestCompetitorsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/ContestCompetitors
        [HttpGet]
        public IEnumerable<ContestCompetitor> GetContestCompetitors()
        {
            foreach (ContestCompetitor contestCompetitor in _context.ContestCompetitors)
            {
                contestCompetitor.Contest = _context.Contests.Where(l => l.ContestId == contestCompetitor.ContestId).FirstOrDefault();
                contestCompetitor.Competitor = _context.Competitors.Where(l => l.CompetitorId == contestCompetitor.CompetitorId).FirstOrDefault();
            }

            return _context.ContestCompetitors;
        }

        // GET: api/ContestCompetitors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContestCompetitor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contestCompetitor = await _context.ContestCompetitors.FindAsync(id);

            if (contestCompetitor == null)
            {
                return NotFound();
            }

            return Ok(contestCompetitor);
        }

        // PUT: api/ContestCompetitors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContestCompetitor([FromRoute] int id, [FromBody] ContestCompetitor contestCompetitor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contestCompetitor.ContestCompetitorId)
            {
                return BadRequest("Užklausos ID nesutampa su formoje esančiu ID");
            }

            var competitor = _context.Competitors.Where(l => l.CompetitorId == contestCompetitor.CompetitorId).Select(l => l.CompetitorId).FirstOrDefault().ToString();

            var contest = _context.Contests.Where(l => l.ContestId == contestCompetitor.ContestId).Select(l => l.ContestId).FirstOrDefault().ToString();

            if (competitor == "0" || contest == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas konkursas arba konkurentas");
            }

            _context.Entry(contestCompetitor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContestCompetitorExists(id))
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

        // POST: api/ContestCompetitors
        [HttpPost]
        public async Task<IActionResult> PostContestCompetitor([FromBody] ContestCompetitor contestCompetitor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var competitor = _context.Competitors.Where(l => l.CompetitorId == contestCompetitor.CompetitorId).Select(l => l.CompetitorId).FirstOrDefault().ToString();

            var contest = _context.Contests.Where(l => l.ContestId == contestCompetitor.ContestId).Select(l => l.ContestId).FirstOrDefault().ToString();

            if (competitor == "0" || contest == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas konkursas arba konkurentas");
            }

            _context.ContestCompetitors.Add(contestCompetitor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContestCompetitor", new { id = contestCompetitor.ContestCompetitorId }, contestCompetitor);
        }

        // DELETE: api/ContestCompetitors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContestCompetitor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contestCompetitor = await _context.ContestCompetitors.FindAsync(id);
            if (contestCompetitor == null)
            {
                return NotFound();
            }

           

            _context.ContestCompetitors.Remove(contestCompetitor);
            await _context.SaveChangesAsync();

            return Ok(contestCompetitor);
        }

        private bool ContestCompetitorExists(int id)
        {
            return _context.ContestCompetitors.Any(e => e.ContestCompetitorId == id);
        }
    }
}