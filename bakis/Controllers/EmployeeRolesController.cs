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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeRolesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public EmployeeRolesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeRoles
        [HttpGet]
        public IEnumerable<EmployeeRole> GetEmployeeRoles()
        {
            return _context.EmployeeRoles;
        }

        // GET: api/EmployeeRoles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeRole([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeRole = await _context.EmployeeRoles.FindAsync(id);

            if (employeeRole == null)
            {
                return NotFound();
            }

            return Ok(employeeRole);
        }

        // PUT: api/EmployeeRoles/5
        [Authorize(Policy = "manageClassifiers")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeRole([FromRoute] int id, [FromBody] EmployeeRole employeeRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeRole.EmployeeRoleId)
            {
                return BadRequest("Užklausos ID nesutampa su formoje esančiu ID");
            }

            _context.Entry(employeeRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeRoleExists(id))
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

        // POST: api/EmployeeRoles
        [Authorize(Policy = "manageClassifiers")]
        [HttpPost]
        public async Task<IActionResult> PostEmployeeRole([FromBody] EmployeeRole employeeRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EmployeeRoles.Add(employeeRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeRole", new { id = employeeRole.EmployeeRoleId }, employeeRole);
        }

        // DELETE: api/EmployeeRoles/5
        [Authorize(Policy = "manageClassifiers")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeRole([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            var employeeRole = await _context.EmployeeRoles.FindAsync(id);
            if (employeeRole == null)
            {
                return NotFound();
            }

            var workingTimeRegisters = _context.WorkingTimeRegisters.Where(l => l.EmployeeRoleId == id).Select(l => l.WorkingTimeRegisterId).FirstOrDefault().ToString();

            var resourcePlans = _context.ResourcePlans.Where(l => l.EmployeeRoleId == id).Select(l => l.ResourcePlanId).FirstOrDefault().ToString();

            if (workingTimeRegisters != "0" || resourcePlans !="0")
            {
                return BadRequest("Darbuotojo rolė turi susijusių įrašų ir negali būti ištrinta");
            }

            _context.EmployeeRoles.Remove(employeeRole);
            await _context.SaveChangesAsync();
            return Ok(employeeRole);
        }

        private bool EmployeeRoleExists(int id)
        {
            return _context.EmployeeRoles.Any(e => e.EmployeeRoleId == id);
        }
    }
}