using System;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupRightsController : ControllerBase
    {
        private readonly ProjectContext _context;

        public GroupRightsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/GroupRights
        [HttpGet]
        public IEnumerable<GroupRight> GetGroupRights()
        {
            return _context.GroupRights;
        }

        // GET: api/GroupRights/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupRight([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groupRight = await _context.GroupRights.FindAsync(id);

            if (groupRight == null)
            {
                return NotFound();
            }

            return Ok(groupRight);
        }

        // PUT: api/GroupRights/5
        [Authorize(Policy = "manageClassifiers")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupRight([FromRoute] int id, [FromBody] GroupRight groupRight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != groupRight.GroupRightId)
            {
                return BadRequest();
            }

            _context.Entry(groupRight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupRightExists(id))
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

        // POST: api/GroupRights
        [Authorize(Policy = "manageClassifiers")]
        [HttpPost]
        public async Task<IActionResult> PostGroupRight([FromBody] GroupRight groupRight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GroupRights.Add(groupRight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGroupRight", new { id = groupRight.GroupRightId }, groupRight);
        }

        // DELETE: api/GroupRights/5
        [Authorize(Policy = "manageClassifiers")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupRight([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var groupRight = await _context.GroupRights.FindAsync(id);
            if (groupRight == null)
            {
                return NotFound();
            }

            var user = _context.Users.Where(l => l.GroupRightId == groupRight.GroupRightId).Select(l => l.GroupRightId).FirstOrDefault().ToString();

            if (user != "0")
            {
                return BadRequest("Grupė yra priskirta bent vienam naudotojui ir negali būti ištrinta");
            }

            _context.GroupRights.Remove(groupRight);
            await _context.SaveChangesAsync();

            return Ok(groupRight);
        }

        private bool GroupRightExists(int id)
        {
            return _context.GroupRights.Any(e => e.GroupRightId == id);
        }
    }
}