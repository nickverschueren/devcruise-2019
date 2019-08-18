using System;
using AutoMapper;
using Euricom.DevCruise.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Euricom.DevCruise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IMapper _mapper;

        public RoomController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status200OK)]
        public IActionResult GetRooms()
            => Ok(_mapper.Map<string[]>(Enum.GetValues(typeof(Room))));
    }
}