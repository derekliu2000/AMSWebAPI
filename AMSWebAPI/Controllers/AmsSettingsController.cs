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
    public class AmsSettingsController : ControllerBase
    {
        private readonly AMS_SiteContext _context;

        public AmsSettingsController(AMS_SiteContext context)
        {
            _context = context;
        }

        /*
        // GET: api/AmsSettings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AmsSetting>>> GetAmsSettings()
        {
            return await _context.AmsSettings.ToListAsync();
        }

        // GET: api/AmsSettings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AmsSetting>> GetAmsSetting(int id)
        {
            var amsSetting = await _context.AmsSettings.FindAsync(id);

            if (amsSetting == null)
            {
                return NotFound();
            }

            return amsSetting;
        }

        // PUT: api/AmsSettings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAmsSetting(int id, AmsSetting amsSetting)
        {
            if (id != amsSetting.Id)
            {
                return BadRequest();
            }

            _context.Entry(amsSetting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmsSettingExists(id))
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
        */

        // POST: api/AmsSettings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AmsSetting>> PostAmsSetting(DateTime LastModifiedUTC, [FromBody]string strAMSSetting)
        {
            _context.SetDatabase(Request);            

            AmsSetting amsSetting = new AmsSetting() { 
                LastUpdateUtc = LastModifiedUTC, 
                Settings = System.Convert.FromBase64String(strAMSSetting) 
            };
            _context.AmsSettings.Add(amsSetting);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAmsSetting", new { id = amsSetting.Id }, amsSetting);
        }

        /*
        // DELETE: api/AmsSettings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmsSetting(int id)
        {
            var amsSetting = await _context.AmsSettings.FindAsync(id);
            if (amsSetting == null)
            {
                return NotFound();
            }

            _context.AmsSettings.Remove(amsSetting);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AmsSettingExists(int id)
        {
            return _context.AmsSettings.Any(e => e.Id == id);
        }
        */
    }
}
