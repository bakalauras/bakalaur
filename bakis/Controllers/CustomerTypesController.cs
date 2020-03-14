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
    public class CustomerTypesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public CustomerTypesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/CustomerTypes
        [HttpGet]
        public IEnumerable<CustomerType> GetCustomerTypes()
        {
            return _context.CustomerTypes;
        }

        // GET: api/CustomerTypes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerType = await _context.CustomerTypes.FindAsync(id);

            if (customerType == null)
            {
                return NotFound();
            }

            return Ok(customerType);
        }

        [HttpGet("{id}/customers")]
        public async Task<IActionResult> GetCustomerTypeCustomers([FromRoute] int id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerType = await _context.CustomerTypes.FindAsync(id);

            if (customerType == null)
            {
                return NotFound();
            }
            var customers = _context.Customers.Where(l => l.CustomerTypeId == id);

            return Ok(customers);
        }

        // PUT: api/CustomerTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerType([FromRoute] int id, [FromBody] CustomerType customerType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerType.CustomerTypeId)
            {
                return BadRequest();
            }

            _context.Entry(customerType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerTypeExists(id))
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

        // POST: api/CustomerTypes
        [HttpPost]
        public async Task<IActionResult> PostCustomerType([FromBody] CustomerType customerType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CustomerTypes.Add(customerType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerType", new { id = customerType.CustomerTypeId }, customerType);
        }

        // DELETE: api/CustomerTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerType = await _context.CustomerTypes.FindAsync(id);
            if (customerType == null)
            {
                return NotFound();
            }

            var customers = _context.Customers.Where(l => l.CustomerTypeId == id).Select(l => l.CustomerTypeId).FirstOrDefault().ToString();

            if (customers != "0")
            {
                return BadRequest();
            }

            _context.CustomerTypes.Remove(customerType);
            await _context.SaveChangesAsync();

            return Ok(customerType);
        }

        private bool CustomerTypeExists(int id)
        {
            return _context.CustomerTypes.Any(e => e.CustomerTypeId == id);
        }
    }
}