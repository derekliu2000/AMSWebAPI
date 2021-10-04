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
    public class AmsFileLastUpdateUtcsController : ControllerBase
    {
        private readonly AMS_SiteContext _context;

        public AmsFileLastUpdateUtcsController(AMS_SiteContext context)
        {
            _context = context;
        }
                
        // GET: api/AmsFileLastUpdateUtcs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AmsFileLastUpdateUtc>>> GetAmsFileLastUpdateUtcs()
        {
            _context.SetDatabase(Request);
            return await _context.AmsFileLastUpdateUtcs.ToListAsync();
        }

        /*
        // GET: api/AmsFileLastUpdateUtcs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AmsFileLastUpdateUtc>> GetAmsFileLastUpdateUtc(string id)
        {
            var amsFileLastUpdateUtc = await _context.AmsFileLastUpdateUtcs.FindAsync(id);

            if (amsFileLastUpdateUtc == null)
            {
                return NotFound();
            }

            return amsFileLastUpdateUtc;
        }

        // PUT: api/AmsFileLastUpdateUtcs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAmsFileLastUpdateUtc(string id, AmsFileLastUpdateUtc amsFileLastUpdateUtc)
        {
            if (id != amsFileLastUpdateUtc.FileName)
            {
                return BadRequest();
            }

            _context.Entry(amsFileLastUpdateUtc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmsFileLastUpdateUtcExists(id))
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

        // POST: api/AmsFileLastUpdateUtcs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AmsFileLastUpdateUtc>> PostAmsFileLastUpdateUtc(AmsFileLastUpdateUtc amsFileLastUpdateUtc)
        {
            _context.AmsFileLastUpdateUtcs.Add(amsFileLastUpdateUtc);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AmsFileLastUpdateUtcExists(amsFileLastUpdateUtc.FileName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAmsFileLastUpdateUtc", new { id = amsFileLastUpdateUtc.FileName }, amsFileLastUpdateUtc);
        }

        // DELETE: api/AmsFileLastUpdateUtcs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmsFileLastUpdateUtc(string id)
        {
            var amsFileLastUpdateUtc = await _context.AmsFileLastUpdateUtcs.FindAsync(id);
            if (amsFileLastUpdateUtc == null)
            {
                return NotFound();
            }

            _context.AmsFileLastUpdateUtcs.Remove(amsFileLastUpdateUtc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AmsFileLastUpdateUtcExists(string id)
        {
            return _context.AmsFileLastUpdateUtcs.Any(e => e.FileName == id);
        }*/
    }
}
