using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bakis.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace bakis.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize(Policy = "manageContests")]
    [Route("api/[controller]")]
    [ApiController]
    public class ContestFilesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ContestFilesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/ContestFiles
        [HttpGet]
        public IEnumerable<ContestFile> GetContestFiles()
        {
            foreach (ContestFile file in _context.ContestFiles)
            {
                file.Contest = _context.Contests.Where(l => l.ContestId == file.ContestId).FirstOrDefault();
            }
            return _context.ContestFiles;
        }

        // GET: api/ContestFiles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContestFile([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contestFile = await _context.ContestFiles.FindAsync(id);

            if (contestFile == null)
            {
                return NotFound("Konkurso failas nerastas");
            }

            return Ok(contestFile);
        }

        // PUT: api/ContestFiles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContestFile([FromRoute] int id, [FromBody] ContestFile contestFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contestFile.ContestFileId)
            {
                return BadRequest("Užklausos failo ID nesutampa su formoje esančiu failo ID");
            }

            var contests = _context.Contests.Where(l => l.ContestId == contestFile.ContestId).Select(l => l.ContestId).FirstOrDefault().ToString();

            if (contests == "0")
            {
                return BadRequest("Konkursas, kurio failą bandoma pridėti, neegzistuoja");
            }

            contestFile.FileName = GetFile();
            _context.Entry(contestFile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContestFileExists(id))
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

        // POST: api/ContestFiles
        [HttpPost]
        public async Task<IActionResult> PostContestFile([FromBody] ContestFile contestFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contests = _context.Contests.Where(l => l.ContestId == contestFile.ContestId).Select(l => l.ContestId).FirstOrDefault().ToString();

            if (contests == "0")
            {
                return BadRequest("Konkursas, kurio failą bandoma pridėti, neegzistuoja");
            }

            contestFile.FileName = GetFile();
            _context.ContestFiles.Add(contestFile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContestFile", new { id = contestFile.ContestFileId }, contestFile);
        }

        // DELETE: api/ContestFiles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContestFile([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contestFile = await _context.ContestFiles.FindAsync(id);
            if (contestFile == null)
            {
                return NotFound();
            }

            _context.ContestFiles.Remove(contestFile);
            await _context.SaveChangesAsync();

            return Ok(contestFile);
        }

        private bool ContestFileExists(int id)
        {
            return _context.ContestFiles.Any(e => e.ContestFileId == id);
        }

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
                var fileDate = dates.Max();
                string pathFile = "";

                for (int i = 0; i < fileEntries.Length; i++)
                {
                    if (System.IO.File.GetLastWriteTime(fileEntries[i]) == fileDate)
                        pathFile = fileEntries[i];
                }

                return pathFile;

            }
            catch (Exception ex)
            {
                return "Nerasta";
            }
        }
    }
}