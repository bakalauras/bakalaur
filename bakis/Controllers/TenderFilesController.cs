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
    public class TenderFilesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public TenderFilesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/TenderFiles
        [HttpGet]
        public IEnumerable<TenderFile> GetTenderFiles()
        {
            return _context.TenderFiles;
        }

        // GET: api/TenderFiles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenderFile([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenderFile = await _context.TenderFiles.FindAsync(id);

            if (tenderFile == null)
            {
                return NotFound();
            }

            return Ok(tenderFile);
        }

        // PUT: api/TenderFiles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenderFile([FromRoute] int id, [FromBody] TenderFile tenderFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tenderFile.TenderFileId)
            {
                return BadRequest();
            }

            _context.Entry(tenderFile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TenderFileExists(id))
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

        // POST: api/TenderFiles
        [HttpPost]
        public async Task<IActionResult> PostTenderFile([FromBody] TenderFile tenderFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.TenderFiles.Add(tenderFile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTenderFile", new { id = tenderFile.TenderFileId }, tenderFile);
        }

        // DELETE: api/TenderFiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenderFile([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenderFile = await _context.TenderFiles.FindAsync(id);
            if (tenderFile == null)
            {
                return NotFound();
            }

            _context.TenderFiles.Remove(tenderFile);
            await _context.SaveChangesAsync();

            return Ok(tenderFile);
        }

        private bool TenderFileExists(int id)
        {
            return _context.TenderFiles.Any(e => e.TenderFileId == id);
        }
    }
}