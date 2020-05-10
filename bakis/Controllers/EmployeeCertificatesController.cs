using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bakis.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace bakis.Controllers
{
    [Authorize(Policy = "manageEmployees")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeCertificatesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public EmployeeCertificatesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeCertificates
        [HttpGet]
        public IEnumerable<EmployeeCertificate> GetEmployeeCertificates()
        {
            return _context.EmployeeCertificates;
        }

        // GET: api/EmployeeCertificates/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeCertificate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeCertificate = await _context.EmployeeCertificates.FindAsync(id);

            if (employeeCertificate == null)
            {
                return NotFound();
            }

            return Ok(employeeCertificate);
        }

       // [HttpGet("{id}/files")]
       // public IActionResult GetFile()
       public string GetFile()
        {
            try
            {
                var folderName = Path.Combine("Resources");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                string[] fileEntries = Directory.GetFiles(pathToSave);
                int count = 0;
                DateTime[] dates = new DateTime[10];

                for (int i = 0; i < fileEntries.Length; i++)
                {
                    dates[count] = System.IO.File.GetLastWriteTime(fileEntries[i]);
                    count++;
                }
                var reikiama = dates.Max();
                string kelias = "";

                for (int i = 0; i < fileEntries.Length; i++)
                {
                    if (System.IO.File.GetLastWriteTime(fileEntries[i]) == reikiama)
                        kelias = fileEntries[i];
                }

                return kelias;

            }
            catch (Exception ex)
            {
                return "Nerasta";
            }
        }

        // PUT: api/EmployeeCertificates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeCertificate([FromRoute] int id, [FromBody] EmployeeCertificate employeeCertificate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeCertificate.EmployeeCertificateId)
            {
                return BadRequest();
            }

            employeeCertificate.File = GetFile();
            _context.Entry(employeeCertificate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeCertificateExists(id))
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

        // POST: api/EmployeeCertificates
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> PostEmployeeCertificate([FromBody] EmployeeCertificate employeeCertificate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            employeeCertificate.File = GetFile();
            _context.EmployeeCertificates.Add(employeeCertificate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeCertificate", new { id = employeeCertificate.EmployeeCertificateId }, employeeCertificate);
        }

        // DELETE: api/EmployeeCertificates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeCertificate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeCertificate = await _context.EmployeeCertificates.FindAsync(id);
            if (employeeCertificate == null)
            {
                return NotFound();
            }

            _context.EmployeeCertificates.Remove(employeeCertificate);
            await _context.SaveChangesAsync();

            return Ok(employeeCertificate);
        }

        private bool EmployeeCertificateExists(int id)
        {
            return _context.EmployeeCertificates.Any(e => e.EmployeeCertificateId == id);
        }
    }
}