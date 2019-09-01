using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
    public class SpeakerController : ControllerBase
    {
        private readonly DevCruiseDbContext _dbContext;
        private readonly IMapper _mapper;

        public SpeakerController(DevCruiseDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("{email}")]
        [ProducesResponseType(typeof(ViewModels.SpeakerDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSpeaker(string email)
        {
            var speaker = await _dbContext.Speakers.SingleOrDefaultAsync(s => s.Email == email);
            if (speaker == null)
                this.Problem(StatusCodes.Status404NotFound, nameof(email),
                    $"Speaker with email {email} not found");

            return Ok(_mapper.Map<ViewModels.SpeakerDetail>(speaker));
        }

        [HttpGet]
        [ProducesResponseType(typeof(ViewModels.Speaker[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSpeakers()
            => Ok(_mapper.Map<ViewModels.Speaker[]>(await _dbContext.Speakers.ToListAsync()));

        [HttpPost]
        [Authorize(Scopes.WriteAccess)]
        [ProducesResponseType(typeof(ViewModels.SpeakerDetail), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateSpeakers([FromBody] ViewModels.SpeakerDetail newSpeaker, ApiVersion apiVersion)
        {
            var duplicateEmailExists = await _dbContext.Speakers.AnyAsync(s => s.Email == newSpeaker.Email);
            if (duplicateEmailExists) return
                this.Problem(StatusCodes.Status409Conflict, nameof(newSpeaker.Email),
                $"Speaker with email {newSpeaker.Email} already exists");

            var speaker = _mapper.Map<Speaker>(newSpeaker);
            await _dbContext.AddAsync(speaker);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSpeaker), new { email = speaker.Email, version = apiVersion.ToString()  },
                _mapper.Map<ViewModels.SpeakerDetail>(newSpeaker));
        }

        [HttpPut("{email}")]
        [Authorize(Scopes.WriteAccess)]
        [ProducesResponseType(typeof(ViewModels.SpeakerDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateSpeaker(string email, [FromBody] ViewModels.SpeakerDetail updatedSpeaker)
        {
            var speaker = await _dbContext.Speakers.SingleOrDefaultAsync(s => s.Email == email);
            if (speaker == null) return
                this.Problem(StatusCodes.Status404NotFound, nameof(email),
                $"Speaker with email {email} not found");

            if (!string.Equals(updatedSpeaker.Email, email))
            {
                var duplicateEmailExists = await _dbContext.Speakers.AnyAsync(s => s.Email == updatedSpeaker.Email);
                if (duplicateEmailExists)
                    return this.Problem(StatusCodes.Status409Conflict, nameof(updatedSpeaker.Email),
                    $"Speaker with email {updatedSpeaker.Email} already exists");
            }

            _mapper.Map(updatedSpeaker, speaker);
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<ViewModels.SpeakerDetail>(speaker));
        }

        [HttpDelete("{email}")]
        [Authorize(Scopes.WriteAccess)]
        [ProducesResponseType(typeof(ViewModels.SpeakerDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteSpeakers(string email)
        {
            var speaker = await _dbContext.Speakers.SingleOrDefaultAsync(s => s.Email == email);
            if (speaker == null)
                return this.Problem(StatusCodes.Status404NotFound, nameof(email),
                $"Speaker with email {email} not found");

            var isLinkedToSlots = await _dbContext.Slots.AnyAsync(s => s.SlotSpeakers.Any(sp => sp.Speaker == speaker));
            if (isLinkedToSlots) return
                this.Problem(StatusCodes.Status409Conflict, nameof(email),
                $"Speaker with email {email} is linked to a slot");

            _dbContext.Speakers.Remove(speaker);
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<ViewModels.SpeakerDetail>(speaker));
        }
    }
}