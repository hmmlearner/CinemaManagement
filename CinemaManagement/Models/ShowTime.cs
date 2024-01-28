using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class ShowTime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary key (you can adjust it based on your needs)
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime Date { get; set; } // New property for the date of the showtime
        public int MovieId { get; set; } // Foreign key to Movie
        public int TheaterId { get; set; } // Foreign key to Theater
        public int ReservedSeats { get; set; } // this needs to be calculated field driven from booking table that needs to be displayed on get

        // Navigation properties
        public Movie Movie { get; set; } // Navigation property to Movie
        public Theater Theater { get; set; } // Navigation property to Theater
    }
}
