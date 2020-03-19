﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bakis.Models;

namespace bakis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDutiesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public EmployeeDutiesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeDuties
        [HttpGet]
        public IEnumerable<EmployeeDuty> GetEmployeeD()
        {
            return _context.EmployeeD;
        }

        // GET: api/EmployeeDuties/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeDuty([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeDuty = await _context.EmployeeD.FindAsync(id);

            if (employeeDuty == null)
            {
                return NotFound();
            }

            return Ok(employeeDuty);
        }

        // PUT: api/EmployeeDuties/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeDuty([FromRoute] int id, [FromBody] EmployeeDuty employeeDuty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeDuty.EmployeeDutyId)
            {
                return BadRequest();
            }

            _context.Entry(employeeDuty).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeDutyExists(id))
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

        // POST: api/EmployeeDuties
        [HttpPost]
        public async Task<IActionResult> PostEmployeeDuty([FromBody] EmployeeDuty employeeDuty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.EmployeeD.Add(employeeDuty);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeDuty", new { id = employeeDuty.EmployeeDutyId }, employeeDuty);
        }

        // DELETE: api/EmployeeDuties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeDuty([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeDuty = await _context.EmployeeD.FindAsync(id);
            if (employeeDuty == null)
            {
                return NotFound();
            }

            _context.EmployeeD.Remove(employeeDuty);
            await _context.SaveChangesAsync();

            return Ok(employeeDuty);
        }

        private bool EmployeeDutyExists(int id)
        {
            return _context.EmployeeD.Any(e => e.EmployeeDutyId == id);
        }
    }
}