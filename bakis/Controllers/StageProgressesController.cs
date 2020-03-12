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
    public class StageProgressesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public StageProgressesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/StageProgresses
        [HttpGet]
        public IEnumerable<StageProgress> GetStageProgresses()
        {
            return _context.StageProgresses;
        }

        // GET: api/StageProgresses/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStageProgress([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stageProgress = await _context.StageProgresses.FindAsync(id);

            if (stageProgress == null)
            {
                return NotFound();
            }

            return Ok(stageProgress);
        }

        // PUT: api/StageProgresses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStageProgress([FromRoute] int id, [FromBody] StageProgress stageProgress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stageProgress.StageProgressId)
            {
                return BadRequest();
            }

            var projectStage = _context.ProjectStages.Where(l => l.ProjectStageId == stageProgress.ProjectStageId).Select(l => l.ProjectStageId).FirstOrDefault().ToString();

            if (projectStage == "0")
            {
                return BadRequest();
            }

            _context.Entry(stageProgress).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StageProgressExists(id))
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

        // POST: api/StageProgresses
        [HttpPost]
        public async Task<IActionResult> PostStageProgress([FromBody] StageProgress stageProgress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectStage = _context.ProjectStages.Where(l => l.ProjectStageId == stageProgress.ProjectStageId).Select(l => l.ProjectStageId).FirstOrDefault().ToString();

            if (projectStage == "0")
            {
                return BadRequest();
            }

            _context.StageProgresses.Add(stageProgress);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStageProgress", new { id = stageProgress.StageProgressId }, stageProgress);
        }

        // DELETE: api/StageProgresses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStageProgress([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stageProgress = await _context.StageProgresses.FindAsync(id);
            if (stageProgress == null)
            {
                return NotFound();
            }

            _context.StageProgresses.Remove(stageProgress);
            await _context.SaveChangesAsync();

            return Ok(stageProgress);
        }

        private bool StageProgressExists(int id)
        {
            return _context.StageProgresses.Any(e => e.StageProgressId == id);
        }
    }
}