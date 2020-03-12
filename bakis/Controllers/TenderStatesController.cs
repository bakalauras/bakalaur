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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTenderState([FromRoute] int id, [FromBody] TenderState tenderState)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tenderState.TenderStateId)
            {
                return BadRequest();
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

            var tenders = _context.Tenders.Where(l => l.TenderState == id).Select(l => l.TenderId).FirstOrDefault().ToString();

            if (tenders != "0")
            {
                return BadRequest();
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