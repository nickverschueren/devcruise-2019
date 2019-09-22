using DevCruise.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevCruise.Controllers
{
    [ApiController]
    [Route("/api/session")]
    public class SessionController : ControllerBase
    {
        private DevCruiseDbContext _dbContext;
        public SessionController(DevCruiseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Session[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSessions()
        {
            var sessions = await _dbContext.Sessions.ToListAsync();
            return Ok(sessions);
        }

        [HttpGet("{code}")]
        [ProducesResponseType(typeof(Session), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSession(string code)
        {
            var session = await _dbContext.Sessions.SingleOrDefaultAsync(s => s.Code == code);
            if(session == null)
                return NotFound();
            return Ok(session);
        }
    }
}