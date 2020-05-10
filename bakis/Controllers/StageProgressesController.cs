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
    [Authorize(Policy = "manageProjects")]
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
            foreach (StageProgress progress in _context.StageProgresses)
            {
                progress.ProjectStage = _context.ProjectStages.Where(l => l.ProjectStageId == progress.ProjectStageId).FirstOrDefault();
            }
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
                return BadRequest("Užklausos ID nesutampa su formoje esančiu ID");
            }

            var projectStage = _context.ProjectStages.Where(l => l.ProjectStageId == stageProgress.ProjectStageId).Select(l => l.ProjectStageId).FirstOrDefault().ToString();

            if (projectStage == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas projekto etapas");
            }

            stageProgress.Date = stageProgress.Date.ToLocalTime();

            stageProgress = calculateSPI(stageProgress);

            if (stageProgress == null)
            {
                return BadRequest("Netinkamos projekto etapo datos - skaičiuojant rodiklius gaunama dalyba iš nulio");
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

            ProjectStage projectStage = _context.ProjectStages.Where(l => l.ProjectStageId == stageProgress.ProjectStageId).FirstOrDefault();

            if (projectStage == null)
            {
                return BadRequest("Pasirinktas nekorektiškas projekto etapas");
            }

            stageProgress = calculateSPI(stageProgress);

            if(stageProgress == null)
            {
                return BadRequest("Netinkamos projekto etapo datos - skaičiuojant rodiklius gaunama dalyba iš nulio");
            }

            stageProgress.Date = stageProgress.Date.ToLocalTime();

            _context.StageProgresses.Add(stageProgress);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStageProgress", new { id = stageProgress.StageProgressId }, stageProgress);
        }

        private StageProgress calculateSPI(StageProgress stageProgress)
        {
            ProjectStage projectStage = _context.ProjectStages.Where(l => l.ProjectStageId == stageProgress.ProjectStageId).FirstOrDefault();

            double timeElapsed = GetNumberOfBusinessDays(projectStage.StartDate, stageProgress.Date) * 100;

            double timePlanned = GetNumberOfBusinessDays(projectStage.ScheduledStartDate, projectStage.ScheduledEndDate);

            if(timeElapsed !=0 && timePlanned!=0)
            {
                stageProgress.ScheduledPercentage = timeElapsed / timePlanned ;

                stageProgress.SPI = stageProgress.Percentage / stageProgress.ScheduledPercentage;

                stageProgress.ScheduledPercentage = Convert.ToDouble(String.Format("{0:0.00}", stageProgress.ScheduledPercentage));

                stageProgress.SPI = Convert.ToDouble(String.Format("{0:0.00}", stageProgress.SPI));

                return stageProgress;
            }
            return null;
        }

        private static int GetNumberOfBusinessDays(DateTime start, DateTime stop)
        {
            int days = 0;
            while (start <= stop)
            {
                if (start.DayOfWeek != DayOfWeek.Saturday && start.DayOfWeek != DayOfWeek.Sunday)
                {
                    ++days;
                }
                start = start.AddDays(1);
            }
            return days;
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