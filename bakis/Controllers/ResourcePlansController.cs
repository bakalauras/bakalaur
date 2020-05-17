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
    [Authorize(Policy = "manageProjects")]
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcePlansController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ResourcePlansController(ProjectContext context)
        {
            _context = context;
        }

        [ExcludeFromCodeCoverage]
        // GET: api/ResourcePlans
        [HttpGet]
        public IEnumerable<ResourcePlan> GetResourcePlans()
        {
            foreach (ResourcePlan plan in _context.ResourcePlans)
            {
                plan.EmployeeRole = _context.EmployeeRoles.Where(l => l.EmployeeRoleId == plan.EmployeeRoleId).FirstOrDefault();
                plan.ProjectStage = _context.ProjectStages.Where(l => l.ProjectStageId == plan.ProjectStageId).FirstOrDefault();
            }
            return _context.ResourcePlans;
        }

        [ExcludeFromCodeCoverage]
        // GET: api/ResourcePlans/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResourcePlan([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resourcePlan = await _context.ResourcePlans.FindAsync(id);

            if (resourcePlan == null)
            {
                return NotFound();
            }

            return Ok(resourcePlan);
        }

        [ExcludeFromCodeCoverage]
        // PUT: api/ResourcePlans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResourcePlan([FromRoute] int id, [FromBody] ResourcePlan resourcePlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != resourcePlan.ResourcePlanId)
            {
                return BadRequest("Užklausos ID nesutampa su formoje esančiu ID");
            }

            var projectStage = _context.ProjectStages.Where(l => l.ProjectStageId == resourcePlan.ProjectStageId).Select(l => l.ProjectStageId).FirstOrDefault().ToString();

            var employeeRole = _context.EmployeeRoles.Where(l => l.EmployeeRoleId == resourcePlan.EmployeeRoleId).Select(l => l.EmployeeRoleId).FirstOrDefault().ToString();

            if (projectStage == "0" || employeeRole == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas projekto etapas ar darbuotojo rolė");
            }

            resourcePlan = calculatePrice(resourcePlan);

            resourcePlan.DateFrom = resourcePlan.DateFrom.ToLocalTime();
            resourcePlan.DateTo = resourcePlan.DateTo.ToLocalTime();

            _context.Entry(resourcePlan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResourcePlanExists(id))
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
        // POST: api/ResourcePlans
        [HttpPost]
        public async Task<IActionResult> PostResourcePlan([FromBody] ResourcePlan resourcePlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectStage = _context.ProjectStages.Where(l => l.ProjectStageId == resourcePlan.ProjectStageId).Select(l => l.ProjectStageId).FirstOrDefault().ToString();

            var employeeRole = _context.EmployeeRoles.Where(l => l.EmployeeRoleId == resourcePlan.EmployeeRoleId).Select(l => l.EmployeeRoleId).FirstOrDefault().ToString();

            if (projectStage == "0" || employeeRole == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas projekto etapas ar darbuotojo rolė");
            }

            resourcePlan.DateFrom = resourcePlan.DateFrom.ToLocalTime();
            resourcePlan.DateTo = resourcePlan.DateTo.ToLocalTime();

            resourcePlan = calculatePrice(resourcePlan);

            if (resourcePlan.Price == -1)
            {
                return BadRequest("Pasirinkta nekorektiška darbuotojo rolė");
            }

            _context.ResourcePlans.Add(resourcePlan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResourcePlan", new { id = resourcePlan.ResourcePlanId }, resourcePlan);
        }

        public ResourcePlan calculatePrice(ResourcePlan resourcePlan)
        {
            ResourcePlan plan = resourcePlan;

            EmployeeRole employeeRole = _context.EmployeeRoles.Where(l => l.EmployeeRoleId == resourcePlan.EmployeeRoleId).FirstOrDefault();

            if(employeeRole == null)
            {
                plan.Price = -1;
                return plan;
            }

            plan.Price = resourcePlan.Hours * employeeRole.AverageWage / 168;

            plan.Price = Convert.ToDouble(String.Format("{0:0.00}", resourcePlan.Price));

            return plan;
        }

        [ExcludeFromCodeCoverage]
        // DELETE: api/ResourcePlans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResourcePlan([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resourcePlan = await _context.ResourcePlans.FindAsync(id);
            if (resourcePlan == null)
            {
                return NotFound();
            }

            _context.ResourcePlans.Remove(resourcePlan);
            await _context.SaveChangesAsync();

            return Ok(resourcePlan);
        }

        [ExcludeFromCodeCoverage]
        private bool ResourcePlanExists(int id)
        {
            return _context.ResourcePlans.Any(e => e.ResourcePlanId == id);
        }
    }
}