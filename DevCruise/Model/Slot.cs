using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Euricom.DevCruise.Model
{
    public class Slot
    {
        public int Id { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public Room Room { get; set; }

        public Session Session { get; set; }

        public ICollection<SlotSpeaker> SlotSpeakers { get; set; }
    }
}