using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaManagement.Models
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }


        [MaxLength(2000)]
        [MinLength(10)]
        public string Description { get; set; }


        public int Duration { get; set; }// or int??
        
        [MaxLength(200)]
        public string Genre { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public double TicketPrice { get; set; }
    }
}
