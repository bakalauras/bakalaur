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
    public class TendersController : ControllerBase
    {
        private readonly ProjectContext _context;

        public TendersController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Tenders
        [HttpGet]
        public IEnumerable<Tender> GetTenders()
        {
            foreach (Tender tender in _context.Tenders)
            {
                tender.TenderState = _context.TenderStates.Where(l => l.TenderStateId == tender.TenderStateId).FirstOrDefault();
                tender.Contest = _context.Contests.Where(l => l.ContestId == tender.ContestId).FirstOrDefault();
            }
            return _context.Tenders;
        }

        // GET: api/Tenders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTender([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tender = await _context.Tenders.FindAsync(id);

            if (tender == null)
            {
                return NotFound();
            }

            return Ok(tender);
        }

        [HttpGet("{id}/contests")]
        public async Task<IActionResult> GetTendersContest([FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tender = await _context.Tenders.FindAsync(id);

            if (tender == null)
            {
                return NotFound();
            }
            var contests = _context.Contests.Where(l => l.ContestId == tender.ContestId);

            return Ok(contests);
        }

        [HttpGet("{id}/files")]
        public async Task<IActionResult> GetTenderFiles([FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tender = await _context.Tenders.FindAsync(id);

            if (tender == null)
            {
                return NotFound();
            }
            var files = _context.TenderFiles.Where(l => l.TenderId == id);

            return Ok(files);
        }

        // PUT: api/Tenders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTender([FromRoute] int id, [FromBody] Tender tender)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tender.TenderId)
            {
                return BadRequest("Užklausos ID nesutampa su formoje esančiu ID");
            }

            var contest = _context.Contests.Where(l => l.ContestId == tender.ContestId).Select(l => l.ContestId).FirstOrDefault().ToString();

            var tenderState = _context.TenderStates.Where(l => l.TenderStateId == tender.TenderStateId).Select(l => l.TenderStateId).FirstOrDefault().ToString();

            if (contest == "0" || tenderState == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas konkursas ar pasiūlymo būsena");
            }

            tender.FillingDate = tender.FillingDate.ToLocalTime();

            _context.Entry(tender).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenderExists(id))
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

        // POST: api/Tenders
        [HttpPost]
        public async Task<IActionResult> PostTender([FromBody] Tender tender)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contest = _context.Contests.Where(l => l.ContestId == tender.ContestId).Select(l => l.ContestId).FirstOrDefault().ToString();

            var tenderState = _context.TenderStates.Where(l => l.TenderStateId == tender.TenderStateId).Select(l => l.TenderStateId).FirstOrDefault().ToString();

            if (contest == "0" || tenderState == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas konkursas ar pasiūlymo būsena");
            }

            tender.FillingDate = tender.FillingDate.ToLocalTime();

            _context.Tenders.Add(tender);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTender", new { id = tender.TenderId }, tender);
        }

        // DELETE: api/Tenders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTender([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tender = await _context.Tenders.FindAsync(id);
            if (tender == null)
            {
                return NotFound();
            }

            var projects = _context.Projects.Where(l => l.TenderId == id).Select(l => l.ProjectId).FirstOrDefault().ToString();

            var tenderFiles = _context.TenderFiles.Where(l => l.TenderId == id).Select(l => l.TenderFileId).FirstOrDefault().ToString();

            if (tenderFiles != "0" || projects != "0")
            {
                return BadRequest("Pasiūlymas turi susijusių įrašų ir negali būti ištrintas");
            }

            _context.Tenders.Remove(tender);
            await _context.SaveChangesAsync();

            return Ok(tender);
        }

        private bool TenderExists(int id)
        {
            return _context.Tenders.Any(e => e.TenderId == id);
        }
    }
}