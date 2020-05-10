using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bakis.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace bakis.Controllers
{
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
        /*
        public class temp_GanttChart
        {
            public int ProjectStageNameId { get; set; }
            public string StageName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public DateTime ScheduledStartDate { get; set; }
            public DateTime ScheduledEndDate { get; set; }

            public temp_GanttChart(int id, string name, DateTime startDate, DateTime endDate, DateTime scheduledStartDate, DateTime scheduledEndDate)
            {
                ProjectStageNameId = id;
                StageName = name;
                StartDate = startDate;
                EndDate = endDate;
                ScheduledStartDate = scheduledStartDate;
                ScheduledEndDate = scheduledEndDate;

            }
        }*/
        
       /* [HttpGet("{id}/gantt")]
        public async Task<IActionResult> GetGanttChartData([FromRoute] int id)
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
                stage.ProjectStageName.StageName = _context.ProjectStageNames.Where(l => l.ProjctStageNameId == stage.ProjectStageNameId).FirstOrDefault().StageName;
                var ganttObj = new GanttChart(stage.ProjectStageNameId, stage.ProjectStageName.StageName, stage.StartDate, stage.EndDate,
                        stage.ScheduledStartDate, stage.ScheduledStartDate);
                gantts.Append(ganttObj);
            }
            


            return Ok(gantts);
        }*/

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}