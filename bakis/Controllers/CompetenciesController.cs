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
    public class CompetenciesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public CompetenciesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Competencies
        [HttpGet]
        public IEnumerable<Competency> GetCompetencies()
        {
            return _context.Competencies;
        }

        // GET: api/Competencies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompetency([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var competency = await _context.Competencies.FindAsync(id);

            if (competency == null)
            {
                return NotFound();
            }

            return Ok(competency);
        }

        // PUT: api/Competencies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompetency([FromRoute] int id, [FromBody] Competency competency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != competency.CompetencyId)
            {
                return BadRequest();
            }

            _context.Entry(competency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompetencyExists(id))
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

        // POST: api/Competencies
        [HttpPost]
        public async Task<IActionResult> PostCompetency([FromBody] Competency competency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Competencies.Add(competency);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompetency", new { id = competency.CompetencyId }, competency);
        }

        // DELETE: api/Competencies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompetency([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var competency = await _context.Competencies.FindAsync(id);
            if (competency == null)
            {
                return NotFound();
            }

            var employeeCompetency = _context.EmployeeCompetencies.Where(l => l.CompetencyId == id).Select(l => l.EmployeeCompetencyId).FirstOrDefault().ToString();
            var stageCompetency = _context.StageCompetencies.Where(l => l.CompetencyId == id).Select(l => l.StageCompetencyId).FirstOrDefault().ToString();

            if (employeeCompetency != "0" || stageCompetency != "0")
            {
                return BadRequest("Negalima ištrinti šios kompetencijos, nes ji turi susijusių įrašų.");
            }

            _context.Competencies.Remove(competency);
            await _context.SaveChangesAsync();

            return Ok(competency);
        }

        private bool CompetencyExists(int id)
        {
            return _context.Competencies.Any(e => e.CompetencyId == id);
        }
    }
}