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
    public class TenderStatesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public TenderStatesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/TenderStates
        [HttpGet]
        public IEnumerable<TenderState> GetTenderStates()
        {
            return _context.TenderStates;
        }

        // GET: api/TenderStates/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenderState([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenderState = await _context.TenderStates.FindAsync(id);

            if (tenderState == null)
            {
                return NotFound();
            }

            return Ok(tenderState);
        }

        // PUT: api/TenderStates/5
        [Authorize(Policy = "manageClassifiers")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenderState([FromRoute] int id, [FromBody] TenderState tenderState)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tenderState.TenderStateId)
            {
                return BadRequest("Užklausos ID nesutampa su formoje esančiu ID");
            }

            _context.Entry(tenderState).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenderStateExists(id))
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

        // POST: api/TenderStates
        [Authorize(Policy = "manageClassifiers")]
        [HttpPost]
        public async Task<IActionResult> PostTenderState([FromBody] TenderState tenderState)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TenderStates.Add(tenderState);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTenderState", new { id = tenderState.TenderStateId }, tenderState);
        }

        // DELETE: api/TenderStates/5
        [Authorize(Policy = "manageClassifiers")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenderState([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenderState = await _context.TenderStates.FindAsync(id);
            if (tenderState == null)
            {
                return NotFound();
            }

            var tenders = _context.Tenders.Where(l => l.TenderStateId == id).Select(l => l.TenderId).FirstOrDefault().ToString();

            if (tenders != "0")
            {
                return BadRequest("Pasiūlymo būsena turi susijusių įrašų ir negali būti ištrinta");
            }

            _context.TenderStates.Remove(tenderState);
            await _context.SaveChangesAsync();

            return Ok(tenderState);
        }

        private bool TenderStateExists(int id)
        {
            return _context.TenderStates.Any(e => e.TenderStateId == id);
        }
    }
}