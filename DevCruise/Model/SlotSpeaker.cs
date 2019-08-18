using System.ComponentModel.DataAnnotations;

namespace Euricom.DevCruise.Model
{
    public class SlotSpeaker
    {
        [Required]
        public int SlotId { get; set; }

        [Required]
        public int SpeakerId { get; set; }

        [Required]
        public Slot Slot { get; set; }

        [Required]
        public Speaker Speaker { get; set; }
    }
}
