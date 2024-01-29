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
        [Range(1, 10)]
        public int SeatNo { get; set; }

        [StringLength(5)]
        public string SeatRow { get; set; }// char?

        [Required]
        //[Range(1, 20)]
        [StringLength(10)]
        public string SeatRowNumber { get; set; }
        [Required]
        public bool IsAvailable { get; set; } = true;
        // Navigation property
        public Booking Booking { get; set; } // Navigation property to Booking
    }

}