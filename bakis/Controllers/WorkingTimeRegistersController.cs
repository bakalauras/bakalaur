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
    public class WorkingTimeRegistersController : ControllerBase
    {
        private readonly ProjectContext _context;

        public WorkingTimeRegistersController(ProjectContext context)
        {
            _context = context;
        }

        [ExcludeFromCodeCoverage]
        // GET: api/WorkingTimeRegisters
        [HttpGet]
        public IEnumerable<WorkingTimeRegister> GetWorkingTimeRegisters()
        {
            foreach (WorkingTimeRegister register in _context.WorkingTimeRegisters)
            {
                register.EmployeeRole = _context.EmployeeRoles.Where(l => l.EmployeeRoleId == register.EmployeeRoleId).FirstOrDefault();
                register.ProjectStage = _context.ProjectStages.Where(l => l.ProjectStageId == register.ProjectStageId).FirstOrDefault();
                register.Employee = _context.Employees.Where(l => l.EmployeeId == register.EmployeeId).FirstOrDefault();
            }
            return _context.WorkingTimeRegisters;
        }

        [ExcludeFromCodeCoverage]
        // GET: api/WorkingTimeRegisters/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkingTimeRegister([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workingTimeRegister = await _context.WorkingTimeRegisters.FindAsync(id);

            if (workingTimeRegister == null)
            {
                return NotFound();
            }

            return Ok(workingTimeRegister);
        }

        [ExcludeFromCodeCoverage]
        // PUT: api/WorkingTimeRegisters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkingTimeRegister([FromRoute] int id, [FromBody] WorkingTimeRegister workingTimeRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != workingTimeRegister.WorkingTimeRegisterId)
            {
                return BadRequest("Užklausos ID nesutampa su formoje esančiu ID");
            }

            var projectStage = _context.ProjectStages.Where(l => l.ProjectStageId == workingTimeRegister.ProjectStageId).Select(l => l.ProjectStageId).FirstOrDefault().ToString();

            var employee = _context.Employees.Where(l => l.EmployeeId == workingTimeRegister.EmployeeId).Select(l => l.EmployeeId).FirstOrDefault().ToString();

            var employeeRole = _context.EmployeeRoles.Where(l => l.EmployeeRoleId == workingTimeRegister.EmployeeRoleId).Select(l => l.EmployeeRoleId).FirstOrDefault().ToString();

            if (projectStage == "0" || employee == "0" || employeeRole == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas projekto etapas, darbuotojas arba darbuotojo rolė");
            }

            workingTimeRegister = calculatePrice(workingTimeRegister);

            if (workingTimeRegister == null)
            {
                return BadRequest("Neįvestas darbuotojo atlyginimas");
            }

            workingTimeRegister.DateFrom = workingTimeRegister.DateFrom.ToLocalTime();
            workingTimeRegister.DateTo = workingTimeRegister.DateTo.ToLocalTime();

            _context.Entry(workingTimeRegister).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkingTimeRegisterExists(id))
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
        // POST: api/WorkingTimeRegisters
        [HttpPost]
        public async Task<IActionResult> PostWorkingTimeRegister([FromBody] WorkingTimeRegister workingTimeRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectStage = _context.ProjectStages.Where(l => l.ProjectStageId == workingTimeRegister.ProjectStageId).Select(l => l.ProjectStageId).FirstOrDefault().ToString();

            var employee = _context.Employees.Where(l => l.EmployeeId == workingTimeRegister.EmployeeId).Select(l => l.EmployeeId).FirstOrDefault().ToString();

            var employeeRole = _context.EmployeeRoles.Where(l => l.EmployeeRoleId == workingTimeRegister.EmployeeRoleId).Select(l => l.EmployeeRoleId).FirstOrDefault().ToString();

            if (projectStage == "0" || employee == "0" || employeeRole == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas projekto etapas, darbuotojas arba darbuotojo rolė");
            }

            workingTimeRegister = calculatePrice(workingTimeRegister);

            if(workingTimeRegister.Price == -1)
            {
                return BadRequest("Neįvestas darbuotojo atlyginimas");
            }

            workingTimeRegister.DateFrom = workingTimeRegister.DateFrom.ToLocalTime();
            workingTimeRegister.DateTo = workingTimeRegister.DateTo.ToLocalTime();

            _context.WorkingTimeRegisters.Add(workingTimeRegister);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkingTimeRegister", new { id = workingTimeRegister.WorkingTimeRegisterId }, workingTimeRegister);
        }

        public WorkingTimeRegister calculatePrice(WorkingTimeRegister workingTimeRegister)
        {
            WorkingTimeRegister register = workingTimeRegister;

            //int EmployeeId = _context.Employees.Where(l => l.EmployeeId == register.EmployeeId).Select(l => l.EmployeeId).FirstOrDefault();

            Salary salary = _context.Salaries.Where(l => l.EmployeeId == register.EmployeeId).Where(l => (l.DateTo == null || l.DateTo > register.DateTo) && l.DateFrom < register.DateTo).OrderByDescending(l => l.SalaryId).FirstOrDefault();

            if(salary == null)
            {
                register.Price = -1;
                return register;
            }

            register.Price = (register.Hours * salary.EmployeeSalary) / (168 * salary.Staff);

            register.Price = Convert.ToDouble(String.Format("{0:0.00}", register.Price));

            return register;
        }

        [ExcludeFromCodeCoverage]
        // DELETE: api/WorkingTimeRegisters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkingTimeRegister([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workingTimeRegister = await _context.WorkingTimeRegisters.FindAsync(id);
            if (workingTimeRegister == null)
            {
                return NotFound();
            }

            _context.WorkingTimeRegisters.Remove(workingTimeRegister);
            await _context.SaveChangesAsync();

            return Ok(workingTimeRegister);
        }

        [ExcludeFromCodeCoverage]
        private bool WorkingTimeRegisterExists(int id)
        {
            return _context.WorkingTimeRegisters.Any(e => e.WorkingTimeRegisterId == id);
        }
    }
}