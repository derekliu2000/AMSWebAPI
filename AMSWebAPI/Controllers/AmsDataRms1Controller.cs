using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AMSWebAPI.Models;

namespace AMSWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmsDataRms1Controller : ControllerBase
    {
        private readonly AMS_SiteContext _context;

        public AmsDataRms1Controller(AMS_SiteContext context)
        {
            _context = context;
        }

        // GET: api/AmsDataRms1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AmsDataRms1>>> GetAmsDataRms1s()
        {
            return await _context.AmsDataRms1s.ToListAsync();
        }

        // GET: api/AmsDataRms1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AmsDataRms1>> GetAmsDataRms1(DateTime id)
        {
            var amsDataRms1 = await _context.AmsDataRms1s.FindAsync(id);

            if (amsDataRms1 == null)
            {
                return NotFound();
            }

            return amsDataRms1;
        }

        // PUT: api/AmsDataRms1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAmsDataRms1(DateTime id, AmsDataRms1 amsDataRms1)
        {
            if (id != amsDataRms1.Utc)
            {
                return BadRequest();
            }

            _context.Entry(amsDataRms1).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmsDataRms1Exists(id))
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

        // POST: api/AmsDataRms1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AmsDataRms1>> PostAmsDataRms1(AmsDataRms1 amsDataRms1)
        {
            _context.AmsDataRms1s.Add(amsDataRms1);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AmsDataRms1Exists(amsDataRms1.Utc))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAmsDataRms1", new { id = amsDataRms1.Utc }, amsDataRms1);
        }

        // DELETE: api/AmsDataRms1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmsDataRms1(DateTime id)
        {
            var amsDataRms1 = await _context.AmsDataRms1s.FindAsync(id);
            if (amsDataRms1 == null)
            {
                return NotFound();
            }

            _context.AmsDataRms1s.Remove(amsDataRms1);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AmsDataRms1Exists(DateTime id)
        {
            return _context.AmsDataRms1s.Any(e => e.Utc == id);
        }
    }
}
