using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE.Models;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace BE.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize(Policy = "manageEmployees")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeCompetenciesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public EmployeeCompetenciesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeCompetencies
        [HttpGet]
        public IEnumerable<EmployeeCompetency> GetEmployeeCompetencies()
        {
            foreach (EmployeeCompetency comp in _context.EmployeeCompetencies)
            {
                comp.Competency = _context.Competencies.Where(l => l.CompetencyId == comp.CompetencyId).FirstOrDefault();
                comp.Employee = _context.Employees.Where(l => l.EmployeeId == comp.EmployeeId).FirstOrDefault();
            }
            return _context.EmployeeCompetencies;
        }

        // GET: api/EmployeeCompetencies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeCompetency([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeCompetency = await _context.EmployeeCompetencies.FindAsync(id);

            if (employeeCompetency == null)
            {
                return NotFound();
            }

            foreach (EmployeeCompetency comp in _context.EmployeeCompetencies)
            {
                comp.Competency = _context.Competencies.Where(l => l.CompetencyId == comp.CompetencyId).FirstOrDefault();
                comp.Employee = _context.Employees.Where(l => l.EmployeeId == comp.EmployeeId).FirstOrDefault();
            }

            return Ok(employeeCompetency);
        }

        // PUT: api/EmployeeCompetencies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeCompetency([FromRoute] int id, [FromBody] EmployeeCompetency employeeCompetency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeCompetency.EmployeeCompetencyId)
            {
                return BadRequest();
            }

            employeeCompetency.DateFrom = employeeCompetency.DateFrom.ToLocalTime();
            employeeCompetency.DateTo = employeeCompetency.DateTo?.ToLocalTime();
            _context.Entry(employeeCompetency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeCompetencyExists(id))
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

        // POST: api/EmployeeCompetencies
        [HttpPost]
        public async Task<IActionResult> PostEmployeeCompetency([FromBody] EmployeeCompetency employeeCompetency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            employeeCompetency.DateFrom = employeeCompetency.DateFrom.ToLocalTime();
            employeeCompetency.DateTo = employeeCompetency.DateTo?.ToLocalTime();
            _context.EmployeeCompetencies.Add(employeeCompetency);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeCompetency", new { id = employeeCompetency.EmployeeCompetencyId }, employeeCompetency);
        }

        // DELETE: api/EmployeeCompetencies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeCompetency([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeCompetency = await _context.EmployeeCompetencies.FindAsync(id);
            if (employeeCompetency == null)
            {
                return NotFound();
            }

            _context.EmployeeCompetencies.Remove(employeeCompetency);
            await _context.SaveChangesAsync();

            return Ok(employeeCompetency);
        }

        private bool EmployeeCompetencyExists(int id)
        {
            return _context.EmployeeCompetencies.Any(e => e.EmployeeCompetencyId == id);
        }
    }
}