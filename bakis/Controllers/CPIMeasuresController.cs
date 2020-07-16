using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE.Models;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace BE.Controllers
{
    [Authorize(Policy = "manageProjects")]
    [Route("api/[controller]")]
    [ApiController]
    public class CPIMeasuresController : ControllerBase
    {
        private readonly ProjectContext _context;

        public CPIMeasuresController(ProjectContext context)
        {
            _context = context;
        }

        [ExcludeFromCodeCoverage]
        // GET: api/CPIMeasures
        [HttpGet]
        public IEnumerable<CPIMeasure> GetCPIMeasures()
        {
            return _context.CPIMeasures;
        }

        [ExcludeFromCodeCoverage]
        // GET: api/CPIMeasures/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCPIMeasure([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cPIMeasure = await _context.CPIMeasures.FindAsync(id);

            if (cPIMeasure == null)
            {
                return NotFound();
            }

            return Ok(cPIMeasure);
        }

        [ExcludeFromCodeCoverage]
        // PUT: api/CPIMeasures/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCPIMeasure([FromRoute] int id, [FromBody] CPIMeasure cPIMeasure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cPIMeasure.CPIMeasureId)
            {
                return BadRequest("Užklausos ID nesutampa su formoje esančiu ID");
            }

            var projectStage = _context.ProjectStages.Where(l => l.ProjectStageId == cPIMeasure.ProjectStageId).Select(l => l.ProjectStageId).FirstOrDefault().ToString();

            if (projectStage == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas projekto etapas");
            }

cPIMeasure.Date = cPIMeasure.Date.ToLocalTime();

            calculateCPI(cPIMeasure);

            

            _context.Entry(cPIMeasure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CPIMeasureExists(id))
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

        [ExcludeFromCodeCoverage]
        // POST: api/CPIMeasures
        [HttpPost]
        public async Task<IActionResult> PostCPIMeasure([FromBody] CPIMeasure cPIMeasure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectStage = _context.ProjectStages.Where(l => l.ProjectStageId == cPIMeasure.ProjectStageId).Select(l => l.ProjectStageId).FirstOrDefault().ToString();

            if (projectStage == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas projekto etapas");
            }

            cPIMeasure.Date = cPIMeasure.Date.ToLocalTime();

            cPIMeasure = calculateCPI(cPIMeasure);

            if (cPIMeasure.CPI == -1)
            {
                return BadRequest("Nekorektiški duomenys - CPI apskaičiuoti negalima");
            }

            

            _context.CPIMeasures.Add(cPIMeasure);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCPIMeasure", new { id = cPIMeasure.CPIMeasureId }, cPIMeasure);
        }

        public CPIMeasure calculateCPI(CPIMeasure cPIMeasure)
        {
            CPIMeasure measure = cPIMeasure;

            measure.CPI = -1;

            measure.ActualPrice = _context.WorkingTimeRegisters.Where(l => l.ProjectStageId == measure.ProjectStageId && l.DateTo <= measure.Date).Select(l => l.Price).Sum();

            measure.ActualPrice = Convert.ToDouble(String.Format("{0:0.00}", measure.ActualPrice));

            measure.PlannedPrice = _context.ResourcePlans.Where(l => l.ProjectStageId == measure.ProjectStageId && l.DateTo <= measure.Date).Select(l => l.Price).Sum();

            IEnumerable<ResourcePlan> plans = _context.ResourcePlans.Where(l => l.ProjectStageId == measure.ProjectStageId && l.DateTo > measure.Date);

            foreach(ResourcePlan plan in plans)
            {
                measure.PlannedPrice += calculatePlanSum(plan, measure);
            }

            measure.PlannedPrice = Convert.ToDouble(String.Format("{0:0.00}", measure.PlannedPrice));

            if (measure.ActualPrice != 0 && measure.PlannedPrice != 0)
            {
                measure.CPI = measure.PlannedPrice / measure.ActualPrice;
                measure.CPI = Convert.ToDouble(String.Format("{0:0.00}", measure.CPI));
            }
            return measure;
        }

        private double calculatePlanSum(ResourcePlan resourcePlan, CPIMeasure cPIMeasure)
        {
            double planSum = 0;

            double timeElapsed = GetNumberOfBusinessDays(resourcePlan.DateFrom, cPIMeasure.Date);

            double timePlanned = GetNumberOfBusinessDays(resourcePlan.DateFrom, resourcePlan.DateTo);

            if(timePlanned !=0 && timeElapsed != 0)
            {
                planSum = (timeElapsed / timePlanned) * resourcePlan.Price;
                planSum = Convert.ToDouble(String.Format("{0:0.00}", planSum));
            }

            return planSum;
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

        [ExcludeFromCodeCoverage]
        // DELETE: api/CPIMeasures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCPIMeasure([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cPIMeasure = await _context.CPIMeasures.FindAsync(id);
            if (cPIMeasure == null)
            {
                return NotFound();
            }

            _context.CPIMeasures.Remove(cPIMeasure);
            await _context.SaveChangesAsync();

            return Ok(cPIMeasure);
        }

        [ExcludeFromCodeCoverage]
        private bool CPIMeasureExists(int id)
        {
            return _context.CPIMeasures.Any(e => e.CPIMeasureId == id);
        }
    }
}