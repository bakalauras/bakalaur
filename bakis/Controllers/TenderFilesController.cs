﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Diagnostics.CodeAnalysis;

namespace BE.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize(Policy = "manageTenders")]
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
            foreach (TenderFile file in _context.TenderFiles)
            {
                file.Tender = _context.Tenders.Where(l => l.TenderId == file.TenderId).FirstOrDefault();
            }
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
                return BadRequest("Užklausos ID nesutampa su formoje esančiu ID");
            }

            var tender = _context.Tenders.Where(l => l.TenderId == tenderFile.TenderId).Select(l => l.TenderId).FirstOrDefault().ToString();

            if (tender == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas pasiūlymas");
            }

            tenderFile.FileName = GetFile();
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

            var tender = _context.Tenders.Where(l => l.TenderId == tenderFile.TenderId).Select(l => l.TenderId).FirstOrDefault().ToString();

            if (tender == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas pasiūlymas");
            }

            tenderFile.FileName = GetFile();
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