using System.ComponentModel.DataAnnotations;

namespace Euricom.DevCruise.Model
{
    public class Speaker
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
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