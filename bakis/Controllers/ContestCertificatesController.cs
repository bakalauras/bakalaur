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
    public class ContestCertificatesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ContestCertificatesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/ContestCertificates
        [HttpGet]
        public IEnumerable<ContestCertificate> GetContestCertificates()
        {
            return _context.ContestCertificates;
        }

        // GET: api/ContestCertificates/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContestCertificate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contestCertificate = await _context.ContestCertificates.FindAsync(id);

            if (contestCertificate == null)
            {
                return NotFound();
            }

            return Ok(contestCertificate);
        }

        // PUT: api/ContestCertificates/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContestCertificate([FromRoute] int id, [FromBody] ContestCertificate contestCertificate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contestCertificate.ContestCertificateId)
            {
                return BadRequest();
            }

            if (contestCertificate.Amount <= 0)
            {
                return BadRequest("Kiekis turi būti didesnis už 0");
            }

            _context.Entry(contestCertificate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContestCertificateExists(id))
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

        // POST: api/ContestCertificates
        [HttpPost]
        public async Task<IActionResult> PostContestCertificate([FromBody] ContestCertificate contestCertificate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(contestCertificate.Amount <= 0)
            {
                return BadRequest("Kiekis turi būti didesnis už 0");
            }

            _context.ContestCertificates.Add(contestCertificate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContestCertificate", new { id = contestCertificate.ContestCertificateId }, contestCertificate);
        }

        // DELETE: api/ContestCertificates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContestCertificate([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contestCertificate = await _context.ContestCertificates.FindAsync(id);
            if (contestCertificate == null)
            {
                return NotFound();
            }

            _context.ContestCertificates.Remove(contestCertificate);
            await _context.SaveChangesAsync();

            return Ok(contestCertificate);
        }

        private bool ContestCertificateExists(int id)
        {
            return _context.ContestCertificates.Any(e => e.ContestCertificateId == id);
        }
    }
}