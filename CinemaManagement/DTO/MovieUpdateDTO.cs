namespace CinemaManagement.DTO
{
    public class MovieUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }// or int??
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double TicketPrice { get; set; }
    }
}
