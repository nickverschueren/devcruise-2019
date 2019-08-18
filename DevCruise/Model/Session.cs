using System.ComponentModel.DataAnnotations;

namespace Euricom.DevCruise.Model
{
    public class Session
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
    }
}