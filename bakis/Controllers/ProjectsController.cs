﻿using System;
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
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ProjectsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Projects
        [HttpGet]
        public IEnumerable<Project> GetProjects()
        {
            return _context.Projects;
        }

        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [HttpGet("{id}/projectStages")]
        public async Task<IActionResult> GetProjectStages([FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }
            var projectStages = _context.ProjectStages.Where(l => l.ProjectId == id);

            return Ok(projectStages);
        }

        [HttpGet("{id}/projectTenders")]
        public async Task<IActionResult> GetProjectTenders([FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }
            var tenders = _context.Tenders.Where(l => l.TenderId == project.TenderId);

            return Ok(tenders);
        }

        // PUT: api/Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject([FromRoute] int id, [FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            var customer = _context.Customers.Where(l => l.CustomerId == project.CustomerId).Select(l => l.CustomerId).FirstOrDefault().ToString();

            var tender = _context.Tenders.Where(l => l.TenderId == project.TenderId).Select(l => l.TenderId).FirstOrDefault().ToString();

            if (customer == "0" || tender == "0")
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        [HttpPost]
        public async Task<IActionResult> PostProject([FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = _context.Customers.Where(l => l.CustomerId == project.CustomerId).Select(l => l.CustomerId).FirstOrDefault().ToString();

            var tender = _context.Tenders.Where(l => l.TenderId == project.TenderId).Select(l => l.TenderId).FirstOrDefault().ToString();

            if (customer == "0" || tender == "0")
            {
                return BadRequest();
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.ProjectId }, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            var projectStages = _context.ProjectStages.Where(l => l.ProjectId == id).Select(l => l.ProjectStageId).FirstOrDefault().ToString();

            if (projectStages != "0")
            {
                return BadRequest();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}