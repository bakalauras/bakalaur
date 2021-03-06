﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Diagnostics.CodeAnalysis;

namespace BE.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize]
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
            foreach (Project project in _context.Projects)
            {
                project.Customer = _context.Customers.Where(l => l.CustomerId == project.CustomerId).FirstOrDefault();
                project.Tender = _context.Tenders.Where(l => l.TenderId == project.TenderId).FirstOrDefault();
                project.Tender.Contest = _context.Contests.Where(l => l.ContestId == project.Tender.ContestId).FirstOrDefault();
            }

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

        [Authorize(Policy = "manageProjects")]
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

            foreach (ProjectStage stage in projectStages)
            {
                stage.Project = _context.Projects.Where(l => l.ProjectId == stage.ProjectId).FirstOrDefault();
                stage.ProjectStageName = _context.ProjectStageNames.Where(l => l.ProjctStageNameId == stage.ProjectStageNameId).FirstOrDefault();
            }

            return Ok(projectStages);
        }

        [Authorize(Policy = "manageProjects")]
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
        [Authorize(Policy = "manageProjects")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject([FromRoute] int id, [FromBody] Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.ProjectId)
            {
                return BadRequest("Užklausos ID nesutampa su formoje esančiu ID");
            }

            var customer = _context.Customers.Where(l => l.CustomerId == project.CustomerId).Select(l => l.CustomerId).FirstOrDefault().ToString();

            var tender = _context.Tenders.Where(l => l.TenderId == project.TenderId).Select(l => l.TenderId).FirstOrDefault().ToString();

            if (customer == "0" || tender == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas užsakovas ar pasiūlymas");
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
        [Authorize(Policy = "manageProjects")]
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
                return BadRequest("Pasirinktas nekorektiškas užsakovas ar pasiūlymas");
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.ProjectId }, project);
        }

        // DELETE: api/Projects/5
        [Authorize(Policy = "manageProjects")]
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
                return BadRequest("Projektas turi susijusių įrašų ir negali būti ištrintas");
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok(project);
        }

        [HttpGet("{id}/projectStagesGantt")]
        public async Task<IActionResult> GetProjectStagesGantt([FromRoute] int id)
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

            foreach (ProjectStage stage in projectStages)
            {

                stage.Project = _context.Projects.Where(l => l.ProjectId == stage.ProjectId).FirstOrDefault();
                stage.ProjectStageName = _context.ProjectStageNames.Where(l => l.ProjctStageNameId == stage.ProjectStageNameId).FirstOrDefault();

                var start = stage.StartDate;//.ToString();
                var end = stage.EndDate;//.ToString();

                if((!start.HasValue) || (!end.HasValue))
                {
                    if (!start.HasValue)
                    {
                        stage.StartDate = stage.ScheduledStartDate.AddMonths(1);
                    }

                    if (!end.HasValue)
                    {
                        stage.EndDate = stage.ScheduledEndDate.AddMonths(1);
                    }
                }

            }
            

            return Ok(projectStages);
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}