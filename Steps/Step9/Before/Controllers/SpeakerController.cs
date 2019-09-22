using DevCruise.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        public IActionResult GetSpeakers()
        {
            return Ok(_dbContext.Speakers.ToList());
        }
        
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(Speaker), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSpeaker(string email)
        {
            var session = _dbContext.Speakers.SingleOrDefault(s => s.Email == email);
            if(session == null)
                return NotFound();
            return Ok(session);
        }
    }
}