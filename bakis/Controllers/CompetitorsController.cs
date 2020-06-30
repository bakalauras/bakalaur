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
    [Authorize(Policy = "manageClassifiers")]
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitorsController : ControllerBase
    {
        private readonly ProjectContext _context;

        public CompetitorsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Competitors
        [HttpGet]
        public IEnumerable<Competitor> GetCompetitor()
        {
            return _context.Competitors;
        }

        // GET: api/Competitors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompetitor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var competitor = await _context.Competitors.FindAsync(id);

            if (competitor == null)
            {
                return NotFound();
            }

            return Ok(competitor);
        }

        // PUT: api/Competitors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompetitor([FromRoute] int id, [FromBody] Competitor competitor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != competitor.CompetitorId)
            {
                return BadRequest("Užklausos konkurento ID nesutampa su formoje esančiu konkurento ID");
            }

            _context.Entry(competitor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompetitorExists(id))
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

        // POST: api/Competitors
        [HttpPost]
        public async Task<IActionResult> PostCompetitor([FromBody] Competitor competitor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Competitors.Add(competitor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompetitor", new { id = competitor.CompetitorId }, competitor);
        }

        // DELETE: api/Competitors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompetitor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var competitor = await _context.Competitors.FindAsync(id);
            if (competitor == null)
            {
                return NotFound();
            }

            var contestCompetitors = _context.ContestCompetitors.Where(l => l.ContestCompetitorId == id).Select(l => l.ContestCompetitorId).FirstOrDefault().ToString();

            if (contestCompetitors != "0")
            {
                return BadRequest("Konkurentas turi susijusių įrašų ir negali būti ištrintas");
            }

            _context.Competitors.Remove(competitor);
            await _context.SaveChangesAsync();

            return Ok(competitor);
        }

        private bool CompetitorExists(int id)
        {
            return _context.Competitors.Any(e => e.CompetitorId == id);
        }
    }
}