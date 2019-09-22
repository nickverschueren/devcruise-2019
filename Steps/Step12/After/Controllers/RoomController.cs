using DevCruise.Model;
using DevCruise.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DevCruise.Controllers
{
    [ApiController]
    [Route("/api/room")]
    [Authorize(Scopes.ReadAccess)]
    public class RoomController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status200OK)]
        public IActionResult GetRooms()
        {
            return Ok(Enum.GetNames(typeof(Room)));
        }
    }
}