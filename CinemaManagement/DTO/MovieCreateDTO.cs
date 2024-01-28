using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTO
{
    public class MovieCreateDTO
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }


        [MaxLength(2000)]
        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        public int Duration { get; set; }// or int??

        [MaxLength(200)]
        public string Genre { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public double TicketPrice { get; set; }
    }
}
