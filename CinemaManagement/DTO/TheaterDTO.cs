using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.DTO
{
    public class TheaterDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int TotalSeats { get; set; }
        public string TheaterType { get; set; }
    }
}
