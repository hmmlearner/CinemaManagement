using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTO
{
    public class SeatDTO
    {
        public int SeatNo { get; set; }
        public string SeatRow { get; set; }// char?
        //public int SeatNumber { get; set; }
        public string SeatRowNumber { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}
