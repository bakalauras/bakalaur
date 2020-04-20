using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using bakis.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bakis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
       // private readonly ProjectContext _context;

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Certificates");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if(file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using(var strem = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(strem);
                    }

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
                
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
            /*var file = Request.Form.Files[0];
            var folderName = Path.Combine("Resources", "Certificates");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);

                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                byte[] bytes = System.IO.File.ReadAllBytes(fileName);
               /* _context.EmployeeCertificates.Add(employeeCertificate);
                await _context.SaveChangesAsync();*/
            
        }

      /*  [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> OnPostUploadAsync()
        {
            var file = Request.Form.Files[0];
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {
                    var files = new EmployeeCertificate()
                    {
                        File = memoryStream.ToArray()
                    };

                    await _context.SaveChangesAsync();
                }
                else
                {
                    return BadRequest();
                }
            }

            return Ok();
        }*/
    }
}