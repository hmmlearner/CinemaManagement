using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTO
{
    public class TheaterCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Required]
        public int TotalSeats { get; set; }

        [Required]
        [MaxLength(200)]
        public string TheaterType { get; set; }
    }
}
