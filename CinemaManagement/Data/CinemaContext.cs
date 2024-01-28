using CinemaManagement.Models;

namespace CinemaManagement.Data
{
    public class CinemaContext : DbContext
    {
        public CinemaContext() : base() { }

        public CinemaContext(DbContextOptions<CinemaContext> options) : base(options)
        {

        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<ShowTime> Showtimes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Seat> Seats { get; set; }

    }
}
