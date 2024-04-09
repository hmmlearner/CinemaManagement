using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTO
{
    public class SeatDto
    {
        public int SeatNo { get; set; }
        public string SeatRow { get; set; }// char?
        public string SeatRowNumber { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
