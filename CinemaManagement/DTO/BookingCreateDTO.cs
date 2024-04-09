using CinemaManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTO
{
    public class BookingCreateDto
    {
        [Required]
        public int ShowtimeId { get; set; } // Foreign key to Showtime
                                            //public int UserId { get; set; } // Foreign key to User
        [Required]
        [Range(1, 20)]
        public int NoOfSeats { get; set; }
        [Required]
        public bool BookingConfirmed { get; set; } = false;
        [Required]
        public DateTime CreatedDateTime { get; set; }
        public List<SeatCreateDto> Seats { get; set; }

    }
}
