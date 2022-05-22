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
    public class ParticipationsController : ControllerBase
    {
        private readonly OlympicGamesAPIContext _context;

        public ParticipationsController(OlympicGamesAPIContext context)
        {
            _context = context;
        }

        // GET: api/Participations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetParticipations()
        {
            return await _context.Participations.Select(p => new
            {
                p.Id,
                p.Medal,
                p.AthleteId,
                p.CountryId,
                p.EventId,
                p.GameId
            }).ToListAsync();
        }

        // GET: api/Participations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetParticipation(int id)
        {
            var participation = await _context.Participations
                .Include(p=>p.Athlete)
                .Include(p=>p.Country)
                .Include(p=>p.Event).ThenInclude(p=>p.Sport)
                .Include(p=>p.Game).ThenInclude(g=>g.Country)
                .FirstOrDefaultAsync(p=>p.Id==id);

            if (participation == null)
            {
                return NotFound();
            }

            return participation.Long();
        }

        // PUT: api/Participations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<object>> PutParticipation(int id, Participation participation)
        {
            if (id != participation.Id)
            {
                return BadRequest();
            }

            _context.Entry(participation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetParticipation", new { id = participation.Id }, (await GetParticipation(participation.Id)).Value);
        }

        // POST: api/Participations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<object>> PostParticipation(Participation participation)
        {
            _context.Participations.Add(participation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetParticipation", new { id = participation.Id }, (await GetParticipation(participation.Id)).Value);
        }

        // DELETE: api/Participations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipation(int id)
        {
            var participation = await _context.Participations.FindAsync(id);
            if (participation == null)
            {
                return NotFound();
            }

            _context.Participations.Remove(participation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParticipationExists(int id)
        {
            return _context.Participations.Any(e => e.Id == id);
        }
    }
}
