using CinemaManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTO
{
    public class BookingCreateDTO
    {
        [Required]
        public int ShowtimeId { get; set; } // Foreign key to Showtime
                                            //public int UserId { get; set; } // Foreign key to User
        [Required]
        [Range(1, 20)]
        public int NoOfSeats { get; set; }
        //public double TotalPrice { get; set; }
        [Required]
        public bool BookingConfirmed { get; set; } = false;
        [Required]
        public DateTime CreatedDateTime { get; set; }
        public List<SeatCreateDTO> Seats { get; set; }
        //public DateTime? ConfirmedDateTime { get; set; }

        // Navigation properties
        //public ShowTime Showtime { get; set; } // Navigation property to Showtime
    }
}
