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
    public class ExamsController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ExamsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Exams
        [HttpGet]
        public IEnumerable<Exam> GetExams()
        {
            return _context.Exams;
        }

        // GET: api/Exams/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetExam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exam = await _context.Exams.FindAsync(id);

            if (exam == null)
            {
                return NotFound();
            }

            return Ok(exam);
        }

        // PUT: api/Exams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExam([FromRoute] int id, [FromBody] Exam exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != exam.ExamId)
            {
                return BadRequest();
            }

            _context.Entry(exam).State = EntityState.Modified;

            var allexams = await _context.Exams.ToArrayAsync();
            var exams = await _context.Exams.FindAsync(id);
            var certi = await _context.Certificates.FindAsync(exams.CertificateId);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamExists(id))
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

        // POST: api/Exams
        [HttpPost]
        public async Task<IActionResult> PostExam([FromBody] Exam exam)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExam", new { id = exam.ExamId }, exam);
        }

        // DELETE: api/Exams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            var employeeExam = _context.EmployeeExams.Where(l => l.ExamId == id).Select(l => l.EmployeeExamId).FirstOrDefault().ToString();

            if (employeeExam != "0")
            {
                return BadRequest("Negalima ištrinti šio egzamino, nes jis yra priskirtas bent vienam darbuotojui.");
            }

            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();

            return Ok(exam);
        }

        private bool ExamExists(int id)
        {
            return _context.Exams.Any(e => e.ExamId == id);
        }
    }
}