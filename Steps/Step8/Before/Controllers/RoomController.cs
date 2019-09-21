using DevCruise.Model;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DevCruise.Controllers
{
    [ApiController]
    [Route("/api/room")]
    public class RoomController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetRooms()
        {
            return Ok(Enum.GetNames(typeof(Room)));
        }
    }
}