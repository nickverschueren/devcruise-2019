using Microsoft.AspNetCore.Mvc;

namespace DevCruise.Controllers
{
    [ApiController]
    [Route("/api/room")]
    public class RoomController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetRooms()
        {
            return Ok(new[] { "Fes", "Rabat", "Nador" });
        }
    }
}