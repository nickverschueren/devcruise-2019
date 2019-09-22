using DevCruise.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        public IActionResult GetSessions()
        {
            return Ok(_dbContext.Sessions.ToList());
        }

        [HttpGet("{code}")]
        [ProducesResponseType(typeof(Session), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSession(string code)
        {
            var session = _dbContext.Sessions.SingleOrDefault(s => s.Code == code);
            if(session == null)
                return NotFound();
            return Ok(session);
        }
    }
}