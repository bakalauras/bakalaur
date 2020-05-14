using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using bakis.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bakis.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly ProjectContext _context;

        public UploadController(ProjectContext context)
        {
            _context = context;
        }

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var strem = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(strem);
                    }
                   
                    return Ok(new { fileName });
                }
                else
                {
                    return BadRequest();
                }
                
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Serverio klaida: {ex}");
            }
        }

        [HttpGet("{id}/certificates")]
        public async Task<IActionResult> Download([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeCertificate = await _context.EmployeeCertificates.FindAsync(id);
            if (employeeCertificate == null)
            {
                return NotFound();
            }

            var certPath = employeeCertificate.File;
            var memory = new MemoryStream();

            using (var stream = new FileStream(certPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(certPath).ToLowerInvariant();
            return File(memory, GetMimeTypes()[ext], Path.GetFileName(certPath));
        }

        [HttpGet("{id}/exams")]
        public async Task<IActionResult> DownloadExam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var empExam = await _context.EmployeeExams.FindAsync(id);
            if (empExam == null)
            {
                return NotFound();
            }

            var ePath = empExam.File;
            var memory = new MemoryStream();

            using (var stream = new FileStream(ePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(ePath).ToLowerInvariant();
            return File(memory, GetMimeTypes()[ext], Path.GetFileName(ePath));
        }

        [HttpGet("{id}/contest")]
        public async Task<IActionResult> DownloadContestFile([FromRoute] int id)
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

            var certPath = contestFile.FileName;
            var memory = new MemoryStream();

            using (var stream = new FileStream(certPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(certPath).ToLowerInvariant();
            return File(memory, GetMimeTypes()[ext], Path.GetFileName(certPath));
        }

        [HttpGet("{id}/tender")]
        public async Task<IActionResult> DownloadTenderFile([FromRoute] int id)
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

            var certPath = tenderFile.FileName;
            var memory = new MemoryStream();

            using (var stream = new FileStream(certPath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var ext = Path.GetExtension(certPath).ToLowerInvariant();
            return File(memory, GetMimeTypes()[ext], Path.GetFileName(certPath));
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf" },
                {".doc",  "application/vnd.ms-word"},
                {".docx", "applications/vnd.ms-word" },
                {".xls",  "application/vnd.ms-excel"},
                {".xlsx",  "application/vnd.ms-excel"},
                {".png", "image/png" },
                {".jpg", "image/jpg" }
            };
        }

    }
}