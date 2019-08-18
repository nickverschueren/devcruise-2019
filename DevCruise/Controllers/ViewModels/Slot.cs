using System;
using System.ComponentModel.DataAnnotations;
using Euricom.DevCruise.Model;

namespace Euricom.DevCruise.Controllers.ViewModels
{
    public class Slot
    {
        [Required]
        public DateTimeOffset StartTime { get; set; }

        [Required]
        public DateTimeOffset EndTime { get; set; }

        [Required]
        public Room Room { get; set; }

        public string[] Speakers { get; set; }

        public string SessionCode { get; set; }
    }
}
