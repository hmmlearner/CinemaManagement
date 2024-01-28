using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTO
{
    public class ShowTimeCreateDTO
    {
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime Date { get; set; } // New property for the date of the showtime

        [Required]
        public int MovieId { get; set; } // Foreign key to Movie
        [Required]
        public int TheaterId { get; set; } // Foreign key to Theater
        //public int ReservedSeats { get; set; } // New property for the number of reserved seats

    }
}
