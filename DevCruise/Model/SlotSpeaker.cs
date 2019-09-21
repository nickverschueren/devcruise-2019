using System.ComponentModel.DataAnnotations;

namespace Euricom.DevCruise.Model
{
    public class SlotSpeaker
    {
        [Key]
        public int SlotId { get; set; }

        [Key]
        public int SpeakerId { get; set; }

        [Required]
        public Slot Slot { get; set; }

        [Required]
        public Speaker Speaker { get; set; }
    }
}
