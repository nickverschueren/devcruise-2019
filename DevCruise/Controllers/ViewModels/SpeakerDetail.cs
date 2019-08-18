using System.ComponentModel.DataAnnotations;

namespace Euricom.DevCruise.Controllers.ViewModels
{
    public class SpeakerDetail
    {
        [Required]
        [MaxLength(250)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [MaxLength(2000)]
        public string Bio { get; set; }
    }
}