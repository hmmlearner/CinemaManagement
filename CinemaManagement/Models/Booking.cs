using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary key (you can adjust it based on your needs)
        [Required]
        public int ShowtimeId { get; set; } // Foreign key to Showtime
        public int UserId { get; set; } // Foreign key to User


        [Required]
        [Range(1, 20)]
        public int NoOfSeats { get; set; }
        public double TotalPrice { get; set; }
        [Required]
        public bool BookingConfirmed { get; set; } = false;
        [Required]
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ConfirmedDateTime { get; set; }

        //[Required]
        //public string SeatNumbers { get; set; }

        //TODO: May be add seat selection here? or in a separate table? so that we can show the user which seats are available and which are not


        // Navigation properties
        public ShowTime Showtime { get; set; } // Navigation property to Showtime
        public User User { get; set; } // Navigation property to User


    }
}
