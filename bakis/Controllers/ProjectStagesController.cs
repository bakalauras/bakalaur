using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bakis.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace bakis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectStagesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ProjectStagesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/ProjectStages
        [HttpGet]
        public IEnumerable<ProjectStage> GetProjectStages()
        {
            return _context.ProjectStages;
        }

        // GET: api/ProjectStages/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectStage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectStage = await _context.ProjectStages.FindAsync(id);

            if (projectStage == null)
            {
                return NotFound();
            }

            return Ok(projectStage);
        }

        [HttpGet("{id}/stageProgresses")]
        public async Task<IActionResult> GetProjectStageProgresses([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectStage = await _context.ProjectStages.FindAsync(id);

            if (projectStage == null)
            {
                return NotFound();
            }
            var stageProgresses = _context.StageProgresses.Where(l => l.ProjectStageId == id);

            return Ok(stageProgresses);
        }

        [HttpGet("{id}/workingTimeRegisters")]
        public async Task<IActionResult> GetProjectStageWorkingTimeRegsters([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectStage = await _context.ProjectStages.FindAsync(id);

            if (projectStage == null)
            {
                return NotFound();
            }
            var workingTimeRegisters = _context.WorkingTimeRegisters.Where(l => l.ProjectStageId == id);

            return Ok(workingTimeRegisters);
        }

        [HttpGet("{id}/resourcePlans")]
        public async Task<IActionResult> GetProjectResourcePlans([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectStage = await _context.ProjectStages.FindAsync(id);

            if (projectStage == null)
            {
                return NotFound();
            }
            var resourcePlans = _context.ResourcePlans.Where(l => l.ProjectStageId == id);

            return Ok(resourcePlans);
        }

        [HttpGet("{id}/SPI")]
        public string GetSPI([FromRoute] int id)
        {
            //services.AddDbContext<ProjectContext>(options =>
            //options.UseSqlServer(Configuration.GetConnectionString("DevConnection")));
            //var result;
            using (SqlConnection conn = new SqlConnection("Server=(LocalDB)\\MSSQLLocalDB;Database=ResourcePlanningDB2;Trusted_Connection=True;MultipleActiveResultSets=True;"))
            using (SqlCommand cmd = conn.CreateCommand())
            {
               // cmd.CommandText = parameterStatement.getQuery();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.SPI";
                //cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@StageId", SqlDbType.Int)
                    { Value = id });
                //cmd.Parameters.AddWithValue("SeqName", "SeqNameValue");

                var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                
                conn.Open();
                cmd.ExecuteNonQuery();
                var result = 0;
                result = (int)returnParameter.Value;
                Console.WriteLine(result);
                return result.ToString();
            }
        }

        [HttpGet("{id}/competencies")]
        public async Task<IActionResult> GetProjectStageCompetencies([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectStage = await _context.ProjectStages.FindAsync(id);

            if (projectStage == null)
            {
                return NotFound();
            }
            var competencies = _context.StageCompetencies.Where(l => l.ProjectStageId == id);

            return Ok(competencies);
        }

        // PUT: api/ProjectStages/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectStage([FromRoute] int id, [FromBody] ProjectStage projectStage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != projectStage.ProjectStageId)
            {
                return BadRequest("Užklausos ID nesutampa su formoje esančiu ID");
            }

            var projectStageName = _context.ProjectStageNames.Where(l => l.ProjctStageNameId == projectStage.ProjectStageNameId).Select(l => l.ProjctStageNameId).FirstOrDefault().ToString();

            var project = _context.Projects.Where(l => l.ProjectId == projectStage.ProjectId).Select(l => l.ProjectId).FirstOrDefault().ToString();

            if (projectStageName == "0" || project == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas projektas ar projekto etapo pavadinimas");
            }

            _context.Entry(projectStage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectStageExists(id))
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

        // POST: api/ProjectStages
        [HttpPost]
        public async Task<IActionResult> PostProjectStage([FromBody] ProjectStage projectStage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectStageName = _context.ProjectStageNames.Where(l => l.ProjctStageNameId == projectStage.ProjectStageNameId).Select(l => l.ProjctStageNameId).FirstOrDefault().ToString();

            var project = _context.Projects.Where(l => l.ProjectId == projectStage.ProjectId).Select(l => l.ProjectId).FirstOrDefault().ToString();

            if (projectStageName == "0" || project == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas projektas ar projekto etapo pavadinimas");
            }

            _context.ProjectStages.Add(projectStage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectStage", new { id = projectStage.ProjectStageId }, projectStage);
        }

        // DELETE: api/ProjectStages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectStage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectStage = await _context.ProjectStages.FindAsync(id);
            if (projectStage == null)
            {
                return NotFound();
            }

            var stageProgresses = _context.StageProgresses.Where(l => l.ProjectStageId == id).Select(l => l.StageProgressId).FirstOrDefault().ToString();

            var resourcePlans = _context.ResourcePlans.Where(l => l.ProjectStageId == id).Select(l => l.ResourcePlanId).FirstOrDefault().ToString();

            var workingTimeRegisters = _context.WorkingTimeRegisters.Where(l => l.ProjectStageId == id).Select(l => l.WorkingTimeRegisterId).FirstOrDefault().ToString();

            var stageCompetencies = _context.StageCompetencies.Where(l => l.ProjectStageId == id).Select(l => l.StageCompetencyId).FirstOrDefault().ToString();

            if (stageProgresses != "0" || resourcePlans != "0" || workingTimeRegisters != "0" || stageCompetencies != "0")
            {
                return BadRequest("Projekto etapas turi susijusių įrašų ir negali būti ištrintas");
            }

            _context.ProjectStages.Remove(projectStage);
            await _context.SaveChangesAsync();

            return Ok(projectStage);
        }

        private bool ProjectStageExists(int id)
        {
            return _context.ProjectStages.Any(e => e.ProjectStageId == id);
        }
    }
}