#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Data;

namespace Saltimer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MobTimerController : ControllerBase
    {
        private readonly SaltimerDBContext _context;

        public MobTimerController(SaltimerDBContext context)
        {
            _context = context;
        }

        // GET: api/MobTimer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MobTimer>>> GetMobTimer()
        {
            //return await _context.MobTimer.ToListAsync();
            return await _context.Set<MobTimer>()
                .Include(e => e.UserMobSessions)
                .ToListAsync();

        }

        // GET: api/MobTimer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MobTimer>> GetMobTimer(int id)
        {
            var mobTimer = await _context.MobTimer.FindAsync(id);

            if (mobTimer == null)
            {
                return NotFound();
            }

            return mobTimer;
        }

        // PUT: api/MobTimer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMobTimer(int id, MobTimer mobTimer)
        {
            if (id != mobTimer.Id)
            {
                return BadRequest();
            }

            _context.Entry(mobTimer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MobTimerExists(id))
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

        // POST: api/MobTimer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MobTimer>> PostMobTimer(MobTimer mobTimer)
        {
            _context.MobTimer.Add(mobTimer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMobTimer", new { id = mobTimer.Id }, mobTimer);
        }

        // DELETE: api/MobTimer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMobTimer(int id)
        {
            var mobTimer = await _context.MobTimer.FindAsync(id);
            if (mobTimer == null)
            {
                return NotFound();
            }

            _context.MobTimer.Remove(mobTimer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MobTimerExists(int id)
        {
            return _context.MobTimer.Any(e => e.Id == id);
        }
    }
}
