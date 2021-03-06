﻿using System;
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
    public class ContestsController : ControllerBase
    {
        private readonly ProjectContext _context;

        public ContestsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Contests
        [HttpGet]
        public IEnumerable<Contest> GetContests()
        {
            foreach (Contest contest in _context.Contests)
            {
                contest.Customer = _context.Customers.Where(l => l.CustomerId == contest.CustomerId).FirstOrDefault();
                contest.ContestStatus = _context.ContestStatuses.Where(l => l.ContestStatusId == contest.ContestStatusId).FirstOrDefault();
            }
            return _context.Contests;
        }

        // GET: api/Contests/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contest = await _context.Contests.FindAsync(id);

            if (contest == null)
            {
                return NotFound();
            }

            return Ok(contest);
        }

        [Authorize(Policy = "manageContests")]
        [HttpGet("{id}/files")]
        public async Task<IActionResult> GetContestFiles([FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contest = await _context.Contests.FindAsync(id);

            if (contest == null)
            {
                return NotFound();
            }
            var files = _context.ContestFiles.Where(l => l.ContestId == id);

            return Ok(files);
        }

        [Authorize(Policy = "manageContests")]
        [HttpGet("{id}/certificates")]
        public async Task<IActionResult> GetContestCertificates([FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contest = await _context.Contests.FindAsync(id);

            if (contest == null)
            {
                return NotFound();
            }
            var certificates = _context.ContestCertificates.Where(l => l.ContestId == id);

            foreach (ContestCertificate cert in _context.ContestCertificates)
            {
                cert.Certificate = _context.Certificates.Where(l => l.CertificateId == cert.CertificateId).FirstOrDefault();
            }

            return Ok(certificates);
        }

        [Authorize(Policy = "manageContests")]
        [HttpGet("{id}/competitors")]
        public async Task<IActionResult> GetContestCompetitors([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contest = await _context.Contests.FindAsync(id);

            if (contest == null)
            {
                return NotFound();
            }
            var competitors = _context.ContestCompetitors.Where(l => l.ContestId == id);

            foreach (ContestCompetitor comp in _context.ContestCompetitors)
            {
                comp.Competitor = _context.Competitors.Where(l => l.CompetitorId == comp.CompetitorId).FirstOrDefault();
            }

            return Ok(competitors);
        }

        // PUT: api/Contests/5
        [Authorize(Policy = "manageContests")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContest([FromRoute] int id, [FromBody] Contest contest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contest.ContestId)
            {
                return BadRequest("Užklausos konkurso ID nesutampa su formoje esančiu konkurso ID");
            }

            var contestStatus = _context.ContestStatuses.Where(l => l.ContestStatusId == contest.ContestStatusId).Select(l => l.ContestStatusId).FirstOrDefault().ToString();

            var customer = _context.Customers.Where(l => l.CustomerId == contest.CustomerId).Select(l => l.CustomerId).FirstOrDefault().ToString();

            if (contestStatus == "0" || customer == "0")
            {
                return BadRequest("Pasirinkta nekorektiška konkurso būsena ar užsakovas");
            }

            contest.ClaimsFillingDate = contest.ClaimsFillingDate?.ToLocalTime();
            contest.FillingDate = contest.FillingDate?.ToLocalTime();
            contest.PriceRobbingDate = contest.PriceRobbingDate?.ToLocalTime();
            contest.PublicationDate = contest.PublicationDate?.ToLocalTime();

            _context.Entry(contest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContestExists(id))
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

        // POST: api/Contests
        [Authorize(Policy = "manageContests")]
        [HttpPost]
        public async Task<IActionResult> PostContest([FromBody] Contest contest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contestStatus = _context.ContestStatuses.Where(l => l.ContestStatusId == contest.ContestStatusId).Select(l => l.ContestStatusId).FirstOrDefault().ToString();

            var customer = _context.Customers.Where(l => l.CustomerId == contest.CustomerId).Select(l => l.CustomerId).FirstOrDefault().ToString();

            if (contestStatus == "0" || customer == "0")
            {
                return BadRequest("Pasirinkta nekorektiška konkurso būsena ar užsakovas");
            }

            contest.ClaimsFillingDate = contest.ClaimsFillingDate?.ToLocalTime();
            contest.FillingDate = contest.FillingDate?.ToLocalTime();
            contest.PriceRobbingDate = contest.PriceRobbingDate?.ToLocalTime();
            contest.PublicationDate = contest.PublicationDate?.ToLocalTime();

            _context.Contests.Add(contest);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContest", new { id = contest.ContestId }, contest);
        }

        // DELETE: api/Contests/5
        [Authorize(Policy = "manageContests")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contest = await _context.Contests.FindAsync(id);
            if (contest == null)
            {
                return NotFound();
            }

            var tenders = _context.Tenders.Where(l => l.ContestId == id).Select(l => l.TenderId).FirstOrDefault().ToString();

            var contestFiles = _context.ContestFiles.Where(l => l.ContestId == id).Select(l => l.ContestFileId).FirstOrDefault().ToString();

            var contestCertificates = _context.ContestCertificates.Where(l => l.ContestId == id).Select(l => l.ContestCertificateId).FirstOrDefault().ToString();

            var contestCompetitors = _context.ContestCompetitors.Where(l => l.ContestId == id).Select(l => l.ContestCompetitorId).FirstOrDefault().ToString();

            if (contestFiles != "0" || tenders != "0" || contestCertificates != "0" || contestCompetitors != "0")
            {
                return BadRequest("Konkursas turi susijusių įrašų ir negali būti ištrintas");
            }

            _context.Contests.Remove(contest);
            await _context.SaveChangesAsync();

            return Ok(contest);
        }

        private bool ContestExists(int id)
        {
            return _context.Contests.Any(e => e.ContestId == id);
        }
    }
}