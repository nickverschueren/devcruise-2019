using System;
using AutoMapper;
using Euricom.DevCruise.Model;
using Euricom.DevCruise.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Euricom.DevCruise.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion( "1.0" )]
    [ApiController]
    [Authorize(Scopes.ReadAccess)]
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