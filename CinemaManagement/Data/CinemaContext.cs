using CinemaManagement.Models;

namespace CinemaManagement.Data
{
    public class CinemaContext : DbContext
    {
        public CinemaContext() : base() { }

        public CinemaContext(DbContextOptions<CinemaContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Seed data for 
            var movies = new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Title = "Aquaman and the Lost Kingdom - 2",
                    Description = "Having failed to defeat Aquaman the first time, Black Manta, still driven by the need to avenge his father’s death, will stop at nothing to take Aquaman down once and for all. This time Black Manta is more formidable than ever before, wielding the power of the mythic Black Trident, which unleashes an ancient and malevolent force. To defeat him, Aquaman will turn to his imprisoned brother Orm, the former King of Atlantis, to forge an unlikely alliance. Together, they must set aside their differences in order to protect their kingdom and save Aquaman’s family, and the world, from irreversible destruction. All returning to the roles they originated, Jason Momoa plays Arthur Curry/Aquaman, now balancing his duties as both the King of Atlantis and a new father; Patrick Wilson is Orm, Aquaman’s half-brother and his nemesis, who must now step into a new role as his brother’s reluctant ally; Amber Heard is Mera, Atlantis’ Queen and mother of the heir to the throne; Yahya Abdul-Mateen II is Black Manta, committed more than ever to avenge his father’s death by destroying Aquaman, his family and Atlantis; and Nicole Kidman as Atlanna, a fierce leader and mother with the heart of a warrior. Also reprising their roles are Dolph Lundgren as King Nereus and Randall Park as Dr. Stephen Shin.",
                    Genre= "Adventure",
                    TicketPrice = 50.99,
                    Duration=120,
                    ReleaseDate=Convert.ToDateTime("2023-12-18"),
                },
                new Movie
                {
                    Id = 2,
                    Title = "The Color Purple",
                    Description = "Warner Bros. Pictures invites you to experience the extraordinary sisterhood of three women who share one unbreakable bond in “The Color Purple.”",
                    Genre= "Musical",
                    TicketPrice = 40.99,
                    Duration=118,
                    ReleaseDate = Convert.ToDateTime("2023-10-18")
                },


            };

            modelBuilder.Entity<Movie>().HasData(movies);

            var theaters = new List<Theater>()
            {
                new Theater()
                {
                    Id = 1,
                    Name="Cinema-1",
                    TotalSeats = 200,
                    TheaterType="Medium",
                },
                new Theater()
                {
                    Id = 2,
                    Name="Cinema-2",
                    TotalSeats = 100,
                    TheaterType="Small",
                },
                 new Theater()
                {
                    Id = 3,
                    Name="Cinema-3",
                    TotalSeats = 100,
                    TheaterType="Small",
                },
                 new Theater()
                {
                    Id = 4,
                    Name="Cinema-4",
                    TotalSeats = 300,
                    TheaterType="Large",
                },
            };

            // Seed  to the database
            modelBuilder.Entity<Theater>().HasData(theaters);


            var showtimes = new List<ShowTime>()
            {
                new ShowTime()
                {
                    Id = 1,
                    StartTime=Convert.ToDateTime("2024-01-27 10:00:00.0000000"),
                    Date = Convert.ToDateTime("2024-01-27"),
                    MovieId=1,
                    TheaterId=1,
                    ReservedSeats= 0,
                },
                new ShowTime()
                {
                    Id = 2,
                    StartTime=Convert.ToDateTime("2024-01-27 14:00:00.0000000"),
                    Date = Convert.ToDateTime("2024-01-27"),
                    MovieId=1,
                    TheaterId=1,
                    ReservedSeats= 0,
                },
                new ShowTime()
                {
                    Id = 3,
                    StartTime=Convert.ToDateTime("2024-01-29 11:00:00.0000000"),
                    Date = Convert.ToDateTime("2024-01-29"),
                    MovieId=2,
                    TheaterId=3,
                    ReservedSeats= 0,
                },

            };

            // Seed  to the database
            modelBuilder.Entity<ShowTime>().HasData(showtimes);



            var users = new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Name="Prathima",
                    Email = "prathima@gmail.com",
                },
                new User()
                {
                    Id = 2,
                    Name="Prathima",
                    Email = "prathima@gmail.com",
                },

            };

            // Seed difficulties to the database
            modelBuilder.Entity<User>().HasData(users);
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<ShowTime> Showtimes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Seat> Seats { get; set; }


        }
}
