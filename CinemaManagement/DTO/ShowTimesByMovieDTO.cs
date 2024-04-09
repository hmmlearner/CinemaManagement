namespace CinemaManagement.DTO
{

    //THIS IS NOT REQUIRED FOR THE EXERCISE
    public class ShowTimesByMovieDto
    {

        public int Id { get; set; } // Primary key (you can adjust it based on your needs)
        public DateTime StartTime { get; set; }
        public DateTime Date { get; set; } // New property for the date of the showtime
        public int MovieId { get; set; } // Foreign key to Movie
        public int TheaterId { get; set; } // Foreign key to Theater
        public int ReservedSeats { get; set; } // New property for the number of reserved seats

        public string MovieTitle { get; set; }
        public string TheaterName { get; set; }

        public int Duration { get; set; }// or int??
        public string Genre { get; set; }

    }
}
