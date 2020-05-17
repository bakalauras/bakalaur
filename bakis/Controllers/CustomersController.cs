using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bakis.Models;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace bakis.Controllers
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ProjectContext _context;

        public CustomersController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public IEnumerable<Customer> GetCustomers()
        {
            foreach (Customer customer in _context.Customers)
            {
                customer.CustomerType = _context.CustomerTypes.Where(l => l.CustomerTypeId == customer.CustomerTypeId).FirstOrDefault();
            }
            return _context.Customers;
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [Authorize(Policy = "manageCustomers")]
        [HttpGet("{id}/projects")]
        public async Task<IActionResult> GetCustomerProjects([FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            var projects = _context.Projects.Where(l => l.CustomerId == id);

            return Ok(projects);
        }

        [Authorize(Policy = "manageCustomers")]
        [HttpGet("{id}/contests")]
        public async Task<IActionResult> GetCustomerContests([FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }
            var contests = _context.Contests.Where(l => l.CustomerId == id);

            return Ok(contests);
        }

        [Authorize(Policy = "manageCustomers")]
        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {
                return BadRequest("Užklausos būsenos ID nesutampa su formoje esančiu būsenos ID");
            }

            var customerType = _context.CustomerTypes.Where(l => l.CustomerTypeId == customer.CustomerTypeId).Select(l => l.CustomerTypeId).FirstOrDefault().ToString();

            if (customerType == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas užsakovo tipas");
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        [Authorize(Policy = "manageCustomers")]
        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerType = _context.CustomerTypes.Where(l => l.CustomerTypeId == customer.CustomerTypeId).Select(l => l.CustomerTypeId).FirstOrDefault().ToString();

            if (customerType == "0")
            {
                return BadRequest("Pasirinktas nekorektiškas užsakovo tipas");
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        [Authorize(Policy = "manageCustomers")]
        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var projects = _context.Projects.Where(l => l.CustomerId == id).Select(l => l.ProjectId).FirstOrDefault().ToString();

            var contests = _context.Contests.Where(l => l.CustomerId == id).Select(l => l.ContestId).FirstOrDefault().ToString();
            
            if (contests != "0" || projects != "0")
            {
                return BadRequest("Užsakovas turi susijusių įrašų ir negali būti ištrintas");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}