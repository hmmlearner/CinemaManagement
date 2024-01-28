using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{

    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary key (you can adjust it based on your needs)
        [Required]
        public int BookingId { get; set; } // Foreign key to Booking

        // Add any additional properties for the seat record
        //[Required]
        //[Range(1, 3)]
        //public int Row { get; set; }
        [Required]
        [Range(1, 20)]
        public int SeatNumber { get; set; }
        [Required]
        public bool IsAvailable { get; set; } = true;
        // Navigation property
        public Booking Booking { get; set; } // Navigation property to Booking
    }

}