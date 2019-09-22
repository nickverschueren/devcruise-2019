using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        private readonly DevCruiseDbContext _dbContext;
        private readonly IMapper _mapper;
        public SpeakerController(DevCruiseDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ViewModels.Speaker[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSpeakers()
        {
            var speakers = await _dbContext.Speakers.ProjectTo<ViewModels.Speaker>(_mapper.ConfigurationProvider).ToListAsync();
            return Ok(speakers);
        }
        
        [HttpGet("{email}")]
        [ProducesResponseType(typeof(ViewModels.SpeakerDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSpeaker(string email)
        {
            var speaker = await _dbContext.Speakers.SingleOrDefaultAsync(s => s.Email == email);
            if(speaker == null)
                return NotFound();
            return Ok(_mapper.Map<ViewModels.SpeakerDetail>(speaker));
        }
    }
}