using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTO
{
    public class SeatCreateDto
    {

        [Required]

        public string SeatRowNumber { get; set; }
    }
}
