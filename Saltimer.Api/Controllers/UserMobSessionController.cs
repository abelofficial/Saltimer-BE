#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saltimer.Api.Data;

namespace Saltimer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMobSessionController : ControllerBase
    {
        private readonly SaltimerDBContext _context;

        public UserMobSessionController(SaltimerDBContext context)
        {
            _context = context;
        }

        // GET: api/UserMobSession
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserMobSession>>> GetUserMobSession()
        {
            return await _context.UserMobSession.ToListAsync();
        }

        // GET: api/UserMobSession/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserMobSession>> GetUserMobSession(int id)
        {
            var userMobSession = await _context.UserMobSession.FindAsync(id);

            if (userMobSession == null)
            {
                return NotFound();
            }

            return userMobSession;
        }

        // PUT: api/UserMobSession/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserMobSession(int id, UserMobSession userMobSession)
        {
            if (id != userMobSession.Id)
            {
                return BadRequest();
            }

            _context.Entry(userMobSession).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserMobSessionExists(id))
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

        // POST: api/UserMobSession
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserMobSession>> PostUserMobSession(UserMobSession userMobSession)
        {
            _context.UserMobSession.Add(userMobSession);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserMobSession", new { id = userMobSession.Id }, userMobSession);
        }

        // DELETE: api/UserMobSession/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserMobSession(int id)
        {
            var userMobSession = await _context.UserMobSession.FindAsync(id);
            if (userMobSession == null)
            {
                return NotFound();
            }

            _context.UserMobSession.Remove(userMobSession);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserMobSessionExists(int id)
        {
            return _context.UserMobSession.Any(e => e.Id == id);
        }
    }
}
