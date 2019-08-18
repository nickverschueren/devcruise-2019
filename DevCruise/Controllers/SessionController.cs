using System.Threading.Tasks;
using AutoMapper;
using Euricom.DevCruise.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Euricom.DevCruise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly DevCruiseDbContext _dbContext;
        private readonly IMapper _mapper;

        public SessionController(DevCruiseDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("{code}")]
        [ProducesResponseType(typeof(ViewModels.SessionDetail), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSession(string code)
        {
            var session = await _dbContext.Sessions.SingleOrDefaultAsync(s => s.Code == code);
            if (session == null) return NotFound();
            return Ok(_mapper.Map<ViewModels.SessionDetail>(session));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ViewModels.Session[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSessions()
            => Ok(_mapper.Map<ViewModels.Session[]>(await _dbContext.Sessions.ToListAsync()));

        [HttpPost]
        [ProducesResponseType(typeof(ViewModels.SessionDetail[]), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateSession([FromBody] ViewModels.SessionDetail newSession)
        {
            var duplicateCodeExists = await _dbContext.Sessions.AnyAsync(s => s.Code == newSession.Code);
            if (duplicateCodeExists) return Conflict();

            var session = _mapper.Map<Session>(newSession);
            await _dbContext.AddAsync(session);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSession), new { code = session.Code },
                _mapper.Map<ViewModels.SessionDetail>(session));
        }

        [HttpPut("{code}")]
        [ProducesResponseType(typeof(ViewModels.SessionDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateSession(string code, [FromBody] ViewModels.SessionDetail updatedSession)
        {
            var session = await _dbContext.Sessions.SingleOrDefaultAsync(s => s.Code == code);
            if (session == null) return NotFound();

            if (!string.Equals(updatedSession.Code, code))
            {
                var duplicateCodeExists = await _dbContext.Sessions.AnyAsync(s => s.Code == updatedSession.Code);
                if (duplicateCodeExists) return Conflict();
            }

            _mapper.Map(updatedSession, session);

            await _dbContext.SaveChangesAsync();
            return Ok(_mapper.Map<ViewModels.SessionDetail>(session));
        }

        [HttpDelete("{code}")]
        [ProducesResponseType(typeof(ViewModels.SessionDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteSession(string code)
        {
            var session = await _dbContext.Sessions.SingleOrDefaultAsync(s => s.Code == code);
            if (session == null) return NotFound();
            var isLinkedToSlots = await _dbContext.Slots.AnyAsync(s => s.Session == session);
            if (isLinkedToSlots) return Conflict();

            _dbContext.Sessions.Remove(session);
            await _dbContext.SaveChangesAsync();
            return Ok(_mapper.Map<ViewModels.SessionDetail>(session));
        }
    }
}