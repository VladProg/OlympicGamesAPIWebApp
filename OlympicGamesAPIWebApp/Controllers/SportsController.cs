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
    public class SportsController : ControllerBase
    {
        private readonly OlympicGamesAPIContext _context;

        public SportsController(OlympicGamesAPIContext context)
        {
            _context = context;
        }

        // GET: api/Sports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetSports()
        {
            return await _context.Sports.Select(x => x.Short()).ToListAsync();
        }

        // GET: api/Sports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetSport(int id)
        {
            var sport = await _context.Sports.Include(s=>s.Events).FirstOrDefaultAsync(s=>s.Id==id);

            if (sport == null)
            {
                return NotFound();
            }

            return sport.Long();
        }

        // PUT: api/Sports/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> PutSport(int id, Sport sport)
        {
            if (id != sport.Id)
            {
                return BadRequest();
            }

            _context.Entry(sport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SportExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSport", new { id = sport.Id }, (await GetSport(sport.Id)).Value);
        }

        // POST: api/Sports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<object>> PostSport(Sport sport)
        {
            _context.Sports.Add(sport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSport", new { id = sport.Id }, (await GetSport(sport.Id)).Value);
        }

        // DELETE: api/Sports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSport(int id)
        {
            var sport = await _context.Sports.FindAsync(id);
            if (sport == null)
            {
                return NotFound();
            }

            _context.Sports.Remove(sport);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SportExists(int id)
        {
            return _context.Sports.Any(e => e.Id == id);
        }
    }
}
