using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Euricom.DevCruise.Controllers.Extensions;
using Euricom.DevCruise.Model;
using Euricom.DevCruise.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Euricom.DevCruise.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(Scopes.ReadAccess)]
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSession(string code)
        {
            var session = await _dbContext.Sessions.SingleOrDefaultAsync(s => s.Code == code);
            if (session == null)
                return this.Problem(StatusCodes.Status404NotFound, nameof(code), $"Session with code {code} not found");

            return Ok(_mapper.Map<ViewModels.SessionDetail>(session));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ViewModels.Session[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSessions()
            => Ok(await _dbContext.Sessions.ProjectTo<ViewModels.Session>(_mapper.ConfigurationProvider).ToListAsync());

        [HttpPost]
        [Authorize(Scopes.WriteAccess)]
        [ProducesResponseType(typeof(ViewModels.SessionDetail[]), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateSession([FromBody] ViewModels.SessionDetail newSession, ApiVersion apiVersion)
        {
            var duplicateCodeExists = await _dbContext.Sessions.AnyAsync(s => s.Code == newSession.Code);
            if (duplicateCodeExists)
                return this.Problem(StatusCodes.Status409Conflict, nameof(newSession.Code), $"Session with code {newSession.Code} already exists");

            var session = _mapper.Map<Session>(newSession);
            await _dbContext.AddAsync(session);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSession), new { code = session.Code, version = apiVersion.ToString() },
                _mapper.Map<ViewModels.SessionDetail>(session));
        }

        [HttpPut("{code}")]
        [Authorize(Scopes.WriteAccess)]
        [ProducesResponseType(typeof(ViewModels.SessionDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateSession(string code, [FromBody] ViewModels.SessionDetail updatedSession)
        {
            var session = await _dbContext.Sessions.SingleOrDefaultAsync(s => s.Code == code);
            if (session == null)
                return this.Problem(StatusCodes.Status404NotFound, nameof(code), $"Session with code {code} not found");

            if (!string.Equals(updatedSession.Code, code))
            {
                var duplicateCodeExists = await _dbContext.Sessions.AnyAsync(s => s.Code == updatedSession.Code);
                if (duplicateCodeExists)
                    return this.Problem(StatusCodes.Status409Conflict, nameof(code), $"Session with code {code} already exists");
            }

            _mapper.Map(updatedSession, session);

            await _dbContext.SaveChangesAsync();
            return Ok(_mapper.Map<ViewModels.SessionDetail>(session));
        }

        [HttpDelete("{code}")]
        [Authorize(Scopes.WriteAccess)]
        [ProducesResponseType(typeof(ViewModels.SessionDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteSession(string code)
        {
            var session = await _dbContext.Sessions.SingleOrDefaultAsync(s => s.Code == code);
            if (session == null)
                return this.Problem(StatusCodes.Status404NotFound, nameof(code), $"Session with code {code} not found");

            var isLinkedToSlots = await _dbContext.Slots.AnyAsync(s => s.Session == session);
            if (isLinkedToSlots)
                return this.Problem(StatusCodes.Status409Conflict, nameof(code), $"This session is linked to slots");

            _dbContext.Sessions.Remove(session);
            await _dbContext.SaveChangesAsync();
            return Ok(_mapper.Map<ViewModels.SessionDetail>(session));
        }
    }
}