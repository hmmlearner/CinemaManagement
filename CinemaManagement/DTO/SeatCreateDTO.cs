using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTO
{
    public class SeatCreateDTO
    {
        //[Required]
        //[Range(1, 3)]
        //public int Row { get; set; }
        [Required]
        [Range(1, 10)]
        public int SeatNumber { get; set; }
    }
}
