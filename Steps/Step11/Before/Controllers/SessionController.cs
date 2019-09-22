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
    [Route("/api/session")]
    public class SessionController : ControllerBase
    {
        private readonly DevCruiseDbContext _dbContext;
        private readonly IMapper _mapper;
        public SessionController(DevCruiseDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ViewModels.Session[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSessions()
        {
            var sessions = await _dbContext.Sessions.ProjectTo<ViewModels.Session>(_mapper.ConfigurationProvider).ToListAsync();
            return Ok(sessions);
        }

        [HttpGet("{code}")]
        [ProducesResponseType(typeof(ViewModels.SessionDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSession(string code)
        {
            var session = await _dbContext.Sessions.SingleOrDefaultAsync(s => s.Code == code);
            if(session == null)
                return NotFound();
            return Ok(_mapper.Map<ViewModels.SessionDetail>(session));
        }
    }
}