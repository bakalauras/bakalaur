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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectStageNamesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ProjectStageNamesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/ProjectStageNames
        [HttpGet]
        public IEnumerable<ProjectStageName> GetProjectStageNames()
        {
            return _context.ProjectStageNames;
        }

        // GET: api/ProjectStageNames/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectStageName([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectStageName = await _context.ProjectStageNames.FindAsync(id);

            if (projectStageName == null)
            {
                return NotFound();
            }

            return Ok(projectStageName);
        }

        // PUT: api/ProjectStageNames/5
        [Authorize(Policy = "manageClassifiers")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectStageName([FromRoute] int id, [FromBody] ProjectStageName projectStageName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != projectStageName.ProjctStageNameId)
            {
                return BadRequest("Užklausos ID nesutampa su formoje esančiu ID");
            }

            _context.Entry(projectStageName).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectStageNameExists(id))
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

        // POST: api/ProjectStageNames
        [Authorize(Policy = "manageClassifiers")]
        [HttpPost]
        public async Task<IActionResult> PostProjectStageName([FromBody] ProjectStageName projectStageName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ProjectStageNames.Add(projectStageName);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectStageName", new { id = projectStageName.ProjctStageNameId }, projectStageName);
        }

        // DELETE: api/ProjectStageNames/5
        [Authorize(Policy = "manageClassifiers")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectStageName([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectStageName = await _context.ProjectStageNames.FindAsync(id);
            if (projectStageName == null)
            {
                return NotFound();
            }

            var projectStages = _context.ProjectStages.Where(l => l.ProjectStageNameId == id).Select(l => l.ProjectStageId).FirstOrDefault().ToString();

            if (projectStages != "0")
            {
                return BadRequest("Projekto etapo pavadinimas turi susijusių įrašų ir negali būti ištrintas");
            }

            _context.ProjectStageNames.Remove(projectStageName);
            await _context.SaveChangesAsync();

            return Ok(projectStageName);
        }

        private bool ProjectStageNameExists(int id)
        {
            return _context.ProjectStageNames.Any(e => e.ProjctStageNameId == id);
        }
    }
}