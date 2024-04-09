using CinemaManagement.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTO
{
    public class BookingConfirmationDto
    {
        public string MovieTitle { get; set; }

        public int Duration { get; set; }
        public string TheaterName { get; set; }

        public DateTime StartTime { get; set; }
        public bool BookingConfirmed { get; set; } = false;
        public double TotalPrice { get; set; }

        public List<SeatDto> Seats { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
