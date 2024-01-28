using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTO
{
    public class SeatDTO
    {
        public int SeatNumber { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
