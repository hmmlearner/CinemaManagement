using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Models
{
    public class Theater
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // Primary key (you can adjust it based on your needs)
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
