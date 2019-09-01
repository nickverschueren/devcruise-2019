using System;
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
    [ApiVersion( "1.0" )]
    [ApiController]
    [Authorize(Scopes.ReadAccess)]
    public class SlotController : ControllerBase
    {
        private readonly DevCruiseDbContext _dbContext;
        private readonly IMapper _mapper;

        public SlotController(DevCruiseDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("{room}/{startTime}")]
        [ProducesResponseType(typeof(ViewModels.SlotDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSlot(Room room, DateTimeOffset startTime)
        {
            var startTimeUtc = startTime.UtcDateTime;
            var slot = await _dbContext.Slots
                .Include(s => s.Session)
                .Include(s => s.SlotSpeakers)
                    .ThenInclude(sp => sp.Speaker)
                .SingleOrDefaultAsync(s => s.StartTime == startTimeUtc && s.Room == room);

            if (slot == null)
                return this.Problem(StatusCodes.Status404NotFound, nameof(startTime),
                    $"A slot in room {room} and starting at {startTime:g} cannot be found");

            return Ok(_mapper.Map<ViewModels.SlotDetail>(slot));

        }

        [HttpGet]
        [ProducesResponseType(typeof(ViewModels.Slot[]), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSlots()
            => Ok(_mapper.Map<ViewModels.Slot[]>(
                await _dbContext.Slots
                .Include(s => s.Session)
                .Include(s => s.SlotSpeakers)
                    .ThenInclude(sp => sp.Speaker)
                .ToListAsync()));

        [HttpPost]
        [Authorize(Scopes.WriteAccess)]
        [ProducesResponseType(typeof(ViewModels.SlotDetail), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateSlots([FromBody] ViewModels.Slot newSlot, ApiVersion apiVersion)
        {
            var startTimeUtc = newSlot.StartTime.UtcDateTime;
            var endTimeUtc = newSlot.EndTime.UtcDateTime;

            var overlappingSlotExists = await _dbContext.Slots.AnyAsync(s =>
                s.Room == newSlot.Room && s.EndTime >= startTimeUtc && s.StartTime <= endTimeUtc);
            if (overlappingSlotExists)
                return this.Problem(StatusCodes.Status409Conflict, nameof(newSlot.StartTime),
                    $"A slot in room {newSlot.Room} starting at {newSlot.StartTime:g} and ending {newSlot.EndTime:g} would overlap other slots");

            var slot = _mapper.Map<Slot>(newSlot);
            if (!string.IsNullOrEmpty(newSlot.SessionCode))
            {
                var session = await _dbContext.Sessions.SingleOrDefaultAsync(s => s.Code == newSlot.SessionCode);
                if (session == null)
                    return this.Problem(StatusCodes.Status404NotFound, nameof(newSlot.SessionCode),
                        $"Session with code {newSlot.SessionCode} not found");

                slot.Session = session;
            }

            if (newSlot.Speakers?.Any() ?? false)
            {
                var speakers = await _dbContext.Speakers.Where(s => newSlot.Speakers.Contains(s.Email)).ToListAsync();
                if (speakers.Count != newSlot.Speakers.Length)
                    return this.Problem(StatusCodes.Status404NotFound, nameof(newSlot.Speakers),
                        $"Speaker(s) with email { string.Join(", ", newSlot.Speakers.Except(speakers.Select(s => s.Email)))} not found");

                slot.SlotSpeakers = speakers.Select(s => new SlotSpeaker { Speaker = s }).ToList();
            }

            await _dbContext.AddAsync(slot);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSlot), new { room = slot.Room, startTime = slot.StartTime, version = apiVersion.ToString() },
                _mapper.Map<ViewModels.SlotDetail>(slot));
        }

        [HttpPut("{room}/{startTime}")]
        [Authorize(Scopes.WriteAccess)]
        [ProducesResponseType(typeof(ViewModels.SlotDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateSlots(Room room, DateTimeOffset startTime, [FromBody] ViewModels.Slot updatedSlot)
        {
            var startTimeUtc = startTime.UtcDateTime;
            var updatedStartTimeUtc = updatedSlot.StartTime.UtcDateTime;
            var updatedEndTimeUtc = updatedSlot.EndTime.UtcDateTime;

            var slot = await _dbContext.Slots.SingleOrDefaultAsync(s => s.Room == room && s.StartTime == startTimeUtc);
            if (slot == null)
                return this.Problem(StatusCodes.Status404NotFound, nameof(startTime),
                    $"A slot in room {room} and starting at {startTime:g} cannot be found");

            if (!Equals(updatedSlot.Room, room) || !Equals(updatedStartTimeUtc, startTimeUtc) || !Equals(updatedEndTimeUtc, slot.EndTime))
            {
                var overlappingSlotExists = await _dbContext.Slots.AnyAsync(s => s.Id != slot.Id &&
                    s.Room == updatedSlot.Room && s.EndTime >= updatedStartTimeUtc && s.StartTime <= updatedEndTimeUtc);
                if (overlappingSlotExists)
                    return this.Problem(StatusCodes.Status409Conflict, nameof(updatedSlot.StartTime),
                        $"A slot in room {updatedSlot.Room} starting at {updatedSlot.StartTime:g} and ending {updatedSlot.EndTime:g} would overlap other slots");
            }

            _mapper.Map(updatedSlot, slot);
            //TODO session, speakers
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<ViewModels.SlotDetail>(slot));
        }

        [HttpDelete("{room}/{startTime}")]
        [Authorize(Scopes.WriteAccess)]
        [ProducesResponseType(typeof(ViewModels.SlotDetail), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteSlots(Room room, DateTimeOffset startTime)
        {
            var startTimeUtc = startTime.UtcDateTime;
            var slot = await _dbContext.Slots.SingleOrDefaultAsync(s => s.Room == room && s.StartTime == startTimeUtc);
            if (slot == null)
                return this.Problem(StatusCodes.Status404NotFound, nameof(startTime),
                    $"A slot in room {room} and starting at {startTime:g} cannot be found");

            _dbContext.Slots.Remove(slot);
            await _dbContext.SaveChangesAsync();

            return Ok(_mapper.Map<ViewModels.SlotDetail>(slot));
        }
    }
}