﻿using System;
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
    public class EmployeesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public EmployeesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public IEnumerable<Employee> GetEmployees()
        {
            return _context.Employees;

        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // GET: api/Employees/5/manager
        [Authorize(Policy = "manageEmployees")]
        [HttpGet("{id}/manager")]
        public async Task<IActionResult> GetManager([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employees.FindAsync(id);
            string name = employee.Name + ' ' + employee.Surname;

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(name);
        }

        // GET: api/Employees/5/isActive
        [Authorize(Policy = "manageEmployees")]
        [HttpGet("{id}/isActive")]
        public async Task<IActionResult> GetActiveParam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            string param;

            return Ok();
        }

        // GET: api/Employees/5/exams
        [Authorize(Policy = "manageEmployees")]
        [HttpGet("{id}/exams")]
        public async Task<IActionResult> GetEmployeeExams([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var exams = _context.EmployeeExams.Where(l => l.EmployeeId == id);

            foreach (EmployeeExam empExam in _context.EmployeeExams)
            {
                empExam.Exam = _context.Exams.Where(l => l.ExamId == empExam.ExamId).FirstOrDefault();
                empExam.Employee = _context.Employees.Where(l => l.EmployeeId == empExam.EmployeeId).FirstOrDefault();
                empExam.Certificate = _context.Certificates.Where(l => l.CertificateId == empExam.CertificateId).FirstOrDefault();
            }

            return Ok(exams);
        }

        // GET: api/Employees/5/salaries
        [Authorize(Policy = "manageEmployees")]
        [HttpGet("{id}/salaries")]
        public async Task<IActionResult> GetEmployeeSalaries([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var salaries = _context.Salaries.Where(l => l.EmployeeId == id);

            return Ok(salaries);
        }

        // GET: api/Employees/5/certificates
        [Authorize(Policy = "manageEmployees")]
        [HttpGet("{id}/certificates")]
        public async Task<IActionResult> GetEmployeeCertificates([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var cert = _context.EmployeeCertificates.Where(l => l.EmployeeId == id);

            foreach (EmployeeCertificate empExam in _context.EmployeeCertificates)
            {
                empExam.Certificate = _context.Certificates.Where(l => l.CertificateId == empExam.CertificateId).FirstOrDefault();
                empExam.Employee = _context.Employees.Where(l => l.EmployeeId == empExam.EmployeeId).FirstOrDefault();
            }

            return Ok(cert);
        }

        // GET: api/Employees/5/duties
        [Authorize(Policy = "manageEmployees")]
        [HttpGet("{id}/duties")]
        public async Task<IActionResult> GetEmployeeDuties([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var duties = _context.EmployeeDuties.Where(l => l.EmployeeId == id);

            foreach (EmployeeDuty empDuty in _context.EmployeeDuties)
            {
                empDuty.Duty = _context.Duties.Where(l => l.DutyId == empDuty.DutyId).FirstOrDefault();
                empDuty.Employee = _context.Employees.Where(l => l.EmployeeId == empDuty.EmployeeId).FirstOrDefault();
            }

            return Ok(duties);
        }

        // GET: api/Employees/5/competencies
        [Authorize(Policy = "manageEmployees")]
        [HttpGet("{id}/competencies")]
        public async Task<IActionResult> GetEmployeeCompetencies([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            var competencies = _context.EmployeeCompetencies.Where(l => l.EmployeeId == id);

            foreach (EmployeeCompetency empComp in _context.EmployeeCompetencies)
            {
                empComp.Competency = _context.Competencies.Where(l => l.CompetencyId == empComp.CompetencyId).FirstOrDefault();
                empComp.Employee = _context.Employees.Where(l => l.EmployeeId == empComp.EmployeeId).FirstOrDefault();
            }

            return Ok(competencies);
        }

        // PUT: api/Employees/5
        [Authorize(Policy = "manageEmployees")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            if(employee.EmployeeId == employee.FkEmployeeId)
            {
                return BadRequest("Negalite priskirti to paties žmogaus vadovu");
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
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

        // POST: api/Employees
        [Authorize(Policy = "manageEmployees")]
        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee);
        }

        // DELETE: api/Employees/5
        [Authorize(Policy = "manageEmployees")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var salary = _context.Salaries.Where(l => l.EmployeeId == id).Select(l => l.SalaryId).FirstOrDefault().ToString();
            var employeeCertificate = _context.EmployeeCertificates.Where(l => l.EmployeeId == id).Select(l => l.EmployeeCertificateId).FirstOrDefault().ToString();
            var employeeExam = _context.EmployeeExams.Where(l => l.EmployeeId == id).Select(l => l.EmployeeExamId).FirstOrDefault().ToString();
            var employeeCompetency = _context.EmployeeCompetencies.Where(l => l.EmployeeId == id).Select(l => l.EmployeeCompetencyId).FirstOrDefault().ToString();
            var employeeDuties = _context.EmployeeDuties.Where(l => l.EmployeeId == id).Select(l => l.EmployeeDutyId).FirstOrDefault().ToString();

            if (salary != "0" || employeeCertificate != "0" || employeeExam != "0" || employeeCompetency != "0" || employeeDuties != "0")
            {
                return BadRequest("Negalima ištrinti šio darbuotojo, nes jis turi susijusių įrašų.");
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return Ok(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}