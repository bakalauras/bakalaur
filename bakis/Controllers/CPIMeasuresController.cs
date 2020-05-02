using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bakis.Models;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.Authorization;

namespace bakis.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CPIMeasuresController : ControllerBase
    {
        private readonly ProjectContext _context;

        public CPIMeasuresController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/CPIMeasures
        [HttpGet]
        public IEnumerable<CPIMeasure> GetCPIMeasures()
        {
            return _context.CPIMeasures;
        }

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
                return BadRequest();
            }

            calculateCPI(cPIMeasure);

            cPIMeasure.Date = cPIMeasure.Date.ToLocalTime();

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

        // POST: api/CPIMeasures
        [HttpPost]
        public async Task<IActionResult> PostCPIMeasure([FromBody] CPIMeasure cPIMeasure)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            calculateCPI(cPIMeasure);

            cPIMeasure.Date = cPIMeasure.Date.ToLocalTime();

            _context.CPIMeasures.Add(cPIMeasure);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCPIMeasure", new { id = cPIMeasure.CPIMeasureId }, cPIMeasure);
        }

        private void calculateCPI(CPIMeasure cPIMeasure)
        {
            cPIMeasure.ActualPrice = _context.WorkingTimeRegisters.Where(l => l.ProjectStageId == cPIMeasure.ProjectStageId && l.DateTo <= cPIMeasure.Date).Select(l => l.Price).Sum();

            cPIMeasure.ActualPrice = Convert.ToDouble(String.Format("{0:0.00}", cPIMeasure.ActualPrice));

            cPIMeasure.PlannedPrice = _context.ResourcePlans.Where(l => l.ProjectStageId == cPIMeasure.ProjectStageId && l.DateTo <= cPIMeasure.Date).Select(l => l.Price).Sum();

            IEnumerable<ResourcePlan> plans = _context.ResourcePlans.Where(l => l.ProjectStageId == cPIMeasure.ProjectStageId && l.DateTo > cPIMeasure.Date);

            foreach(ResourcePlan plan in plans)
            {
                cPIMeasure.PlannedPrice += calculatePlanSum(plan, cPIMeasure);
            }

            cPIMeasure.PlannedPrice = Convert.ToDouble(String.Format("{0:0.00}", cPIMeasure.PlannedPrice));

            cPIMeasure.CPI = 0;

            if (cPIMeasure.ActualPrice != 0 && cPIMeasure.PlannedPrice != 0)
            {
                cPIMeasure.CPI = cPIMeasure.PlannedPrice / cPIMeasure.ActualPrice;
                cPIMeasure.CPI = Convert.ToDouble(String.Format("{0:0.00}", cPIMeasure.CPI));
            }
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

        private bool CPIMeasureExists(int id)
        {
            return _context.CPIMeasures.Any(e => e.CPIMeasureId == id);
        }
    }
}