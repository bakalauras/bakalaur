using System;
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
    public class EmployeeExamsController : ControllerBase
    {
        private readonly ProjectContext _context;

        public EmployeeExamsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/EmployeeExams
        [HttpGet]
        public IEnumerable<EmployeeExam> GetEmployeeExams()
        {
            return _context.EmployeeExams;
        }

        // GET: api/EmployeeExams/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeExam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeExam = await _context.EmployeeExams.FindAsync(id);

            if (employeeExam == null)
            {
                return NotFound();
            }

            return Ok(employeeExam);
        }


        // PUT: api/EmployeeExams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeExam([FromRoute] int id, [FromBody] EmployeeExam employeeExam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employeeExam.EmployeeExamId)
            {
                return BadRequest();
            }

            _context.Entry(employeeExam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExamExists(id))
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

        // POST: api/EmployeeExams
        [HttpPost]
        public async Task<IActionResult> PostEmployeeExam([FromBody] EmployeeExam employeeExam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var firstCert = await _context.Certificates.FirstAsync();
            var cert = await _context.Certificates.FindAsync(employeeExam.CertificateId);
            var query = from b in _context.EmployeeExams
                        from p in _context.EmployeeCertificates.Where(p => b.CertificateId == p.CertificateId)
                        from c in _context.Certificates.Where(c => p.CertificateId == c.CertificateId).Where(a => b.EmployeeId == employeeExam.EmployeeId)
                        select new { b, p, c };
           
            if (cert.CertificateId == 2)
            {
                var query1 = query.Select(s => s.c).Where(a => a.CertificateId == 1);
                if (query1.Count() == 0)
                {
                    return BadRequest("Negalite registruotis, nes neturite " + firstCert.Title + " sertifikato.");
                }
                var query2 = query.Select(s => s.p).Where(a => a.CertificateId == 2);
                if (query2.Count() == 1)
                {
                    return BadRequest("Jūs jau turite šį sertifikatą.");
                }
            }

             _context.EmployeeExams.Add(employeeExam);
             await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeExam", new { id = employeeExam.EmployeeExamId }, employeeExam);
        }

        // DELETE: api/EmployeeExams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeExam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeExam = await _context.EmployeeExams.FindAsync(id);
            if (employeeExam == null)
            {
                return NotFound();
            }

            _context.EmployeeExams.Remove(employeeExam);
            await _context.SaveChangesAsync();

            return Ok(employeeExam);
        }

        private bool EmployeeExamExists(int id)
        {
            return _context.EmployeeExams.Any(e => e.EmployeeExamId == id);
        }
    }
}