using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BiodiversityCloudApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationsController : ControllerBase
    {
        public readonly ApplicationDbContext _context;
        public ObservationsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // POST: api/Observations
        [HttpPost]
        public async Task<ActionResult<Observation>> CreateObservation(Observation observation)
        {
            _context.Observations.Add(observation);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetObservation", new { id = observation.Id }, observation);
        }

        // GET: api/Observations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Observation>>> GetObservations()
        {
            return await _context.Observations.ToListAsync();
        }

        // GET: api/Observations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Observation>> GetObservation(Guid id)
        {
            var observation = await _context.Observations.FindAsync(id);
            if (observation == null)
            {
                return NotFound();
            }
            return observation;
        }

        // PUT: api/Observations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateObservation(Guid id, Observation observation)
        {
            if (id != observation.Id)
            {
                return BadRequest();
            }

            _context.Entry(observation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Observations.Any(e => e.Id == id))
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

        // DELETE: api/Observations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObservation(Guid id)
        {
            var observation = await _context.Observations.FindAsync(id);
            if (observation == null)
            {
                return NotFound();
            }
            _context.Observations.Remove(observation);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    }
}