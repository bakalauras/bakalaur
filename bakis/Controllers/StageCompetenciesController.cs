using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bakis.Models;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace bakis.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize(Policy = "manageProjects")]
    [Route("api/[controller]")]
    [ApiController]
    public class StageCompetenciesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public StageCompetenciesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/StageCompetencies
        [HttpGet]
        public IEnumerable<StageCompetency> GetStageCompetencies()
        {
            foreach (StageCompetency sal in _context.StageCompetencies)
            {
                sal.ProjectStage = _context.ProjectStages.Where(l => l.ProjectStageId == sal.ProjectStageId).FirstOrDefault();
                sal.Competency = _context.Competencies.Where(l => l.CompetencyId == sal.CompetencyId).FirstOrDefault();
            }

            return _context.StageCompetencies;
        }

        // GET: api/StageCompetencies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStageCompetency([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stageCompetency = await _context.StageCompetencies.FindAsync(id);

            if (stageCompetency == null)
            {
                return NotFound();
            }

            foreach (StageCompetency sal in _context.StageCompetencies)
            {
                sal.ProjectStage = _context.ProjectStages.Where(l => l.ProjectStageId == sal.ProjectStageId).FirstOrDefault();
                sal.Competency = _context.Competencies.Where(l => l.CompetencyId == sal.CompetencyId).FirstOrDefault();
            }

            return Ok(stageCompetency);
        }

        // GET: api/StageCompetencies/5/ProjectStages/5/ProjectStageNames/5
        [HttpGet("{id}/ProjectStages")]
        public async Task<IActionResult> GetStageCompetencyNames([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stageCompetency = await _context.StageCompetencies.FindAsync(id);

            if (stageCompetency == null)
            {
                return NotFound();
            }

          //  var stages = _context.ProjectStages.Where(l =)
            return Ok(stageCompetency);
        }

        // PUT: api/StageCompetencies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStageCompetency([FromRoute] int id, [FromBody] StageCompetency stageCompetency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stageCompetency.StageCompetencyId)
            {
                return BadRequest();
            }

            if(stageCompetency.Amount <= 0)
            {
                return BadRequest("Kiekis turi būti didesnis už 0");
            }

            _context.Entry(stageCompetency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StageCompetencyExists(id))
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

        // POST: api/StageCompetencies
        [HttpPost]
        public async Task<IActionResult> PostStageCompetency([FromBody] StageCompetency stageCompetency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(stageCompetency.Amount <= 0)
            {
                return BadRequest("Kiekis turi būti didesnis už 0");
            }

            _context.StageCompetencies.Add(stageCompetency);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStageCompetency", new { id = stageCompetency.StageCompetencyId }, stageCompetency);
        }

        // DELETE: api/StageCompetencies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStageCompetency([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stageCompetency = await _context.StageCompetencies.FindAsync(id);
            if (stageCompetency == null)
            {
                return NotFound();
            }

            _context.StageCompetencies.Remove(stageCompetency);
            await _context.SaveChangesAsync();

            return Ok(stageCompetency);
        }

        private bool StageCompetencyExists(int id)
        {
            return _context.StageCompetencies.Any(e => e.StageCompetencyId == id);
        }
    }
}