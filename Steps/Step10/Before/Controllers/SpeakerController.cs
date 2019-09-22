using DevCruise.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DevCruise.Controllers
{
    [ApiController]
    [Route("/api/speaker")]
    public class SpeakerController : ControllerBase
    {
        private DevCruiseDbContext _dbContext;
        public SpeakerController(DevCruiseDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Speaker[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSpeakers()
        {
            var speakers = await _dbContext.Speakers.ToListAsync();
            return Ok(speakers);
        }
        
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(Speaker), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSpeaker(string email)
        {
            var session = await _dbContext.Speakers.SingleOrDefaultAsync(s => s.Email == email);
            if(session == null)
                return NotFound();
            return Ok(session);
        }
    }
}