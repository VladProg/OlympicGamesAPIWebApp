using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlympicGamesAPIWebApp.Models;

namespace OlympicGamesAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AthletesController : ControllerBase
    {
        private readonly OlympicGamesAPIContext _context;

        public AthletesController(OlympicGamesAPIContext context)
        {
            _context = context;
        }

        // GET: api/Athletes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAthletes()
        {
            return await _context.Athletes.Select(x=>x.Short()).ToListAsync();
        }

        // GET: api/Athletes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetAthlete(int id)
        {
            var athlete = await _context.Athletes
                .Include(a => a.Participations).ThenInclude(p => p.Country)
                .Include(a => a.Participations).ThenInclude(p => p.Event).ThenInclude(e=>e.Sport)
                .Include(a => a.Participations).ThenInclude(p => p.Game).ThenInclude(g=>g.Country)
                .FirstOrDefaultAsync(a=>a.Id==id);

            if (athlete == null)
            {
                return NotFound();
            }

            return athlete.Long();
        }

        // PUT: api/Athletes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAthlete(int id, Athlete athlete)
        {
            if (id != athlete.Id)
            {
                return BadRequest();
            }

            _context.Entry(athlete).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AthleteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAthlete", new { id = athlete.Id }, (await GetAthlete(athlete.Id)).Value);
        }

        // POST: api/Athletes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<object>> PostAthlete(Athlete athlete)
        {
            _context.Athletes.Add(athlete);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAthlete", new { id = athlete.Id }, (await GetAthlete(athlete.Id)).Value);
        }

        // DELETE: api/Athletes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> DeleteAthlete(int id)
        {
            var athlete = await _context.Athletes.FindAsync(id);
            if (athlete == null)
            {
                return NotFound();
            }

            _context.Athletes.Remove(athlete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AthleteExists(int id)
        {
            return _context.Athletes.Any(e => e.Id == id);
        }
    }
}
