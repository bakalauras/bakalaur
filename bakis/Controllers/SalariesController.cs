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
    [ExcludeFromCodeCoverage]
    [Authorize(Policy = "manageEmployees")]
    [Route("api/[controller]")]
    [ApiController]
    public class SalariesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public SalariesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Salaries
        [HttpGet]
        public IEnumerable<Salary> GetSalaries()
        {
            foreach (Salary sal in _context.Salaries)
            {
                sal.Employee = _context.Employees.Where(l => l.EmployeeId == sal.EmployeeId).FirstOrDefault();
            }

            return _context.Salaries;
        }

        // GET: api/Salaries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSalary([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var salary = await _context.Salaries.FindAsync(id);

            if (salary == null)
            {
                return NotFound();
            }
            foreach (Salary sal in _context.Salaries)
            {
                sal.Employee = _context.Employees.Where(l => l.EmployeeId == sal.EmployeeId).FirstOrDefault();
            }

            return Ok(salary);
        }

        // PUT: api/Salaries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalary([FromRoute] int id, [FromBody] Salary salary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != salary.SalaryId)
            {
                return BadRequest();
            }

            salary.DateFrom = salary.DateFrom.ToLocalTime();
            salary.DateTo = salary.DateTo.ToLocalTime();
            _context.Entry(salary).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalaryExists(id))
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

        // POST: api/Salaries
        [HttpPost]
        public async Task<IActionResult> PostSalary([FromBody] Salary salary)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            salary.DateFrom = salary.DateFrom.ToLocalTime();
            salary.DateTo = salary.DateTo.ToLocalTime();
            _context.Salaries.Add(salary);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalary", new { id = salary.SalaryId }, salary);
        }

        // DELETE: api/Salaries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalary([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var salary = await _context.Salaries.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }

            _context.Salaries.Remove(salary);
            await _context.SaveChangesAsync();

            return Ok(salary);
        }

        private bool SalaryExists(int id)
        {
            return _context.Salaries.Any(e => e.SalaryId == id);
        }
    }
}