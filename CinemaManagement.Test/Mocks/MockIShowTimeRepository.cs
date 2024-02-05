using Moq;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;
namespace CinemaManagement.Tests.Mocks
{
    internal class MockIShowTimeRepository
    {
        public static Mock<IShowTimeRepository> GetMock(MockIMovieRepository movieRepository1)
        {
            var mock = new Mock<IShowTimeRepository>();
            //MockIMovieRepository movieRepository = new MockIMovieRepository();
            //int movieId = movieRepository.GetMovieAsync(100).GetAwaiter().GetResult().Id;


            //var result = mockMovieRepository.movieRepository();


            var showTimes = new List<ShowTime>
            {
                new ShowTime()
                {
                    Id = 100,
                    StartTime = Convert.ToDateTime(("2024-01-30 14:30:00")),
                    Date = Convert.ToDateTime(("2024-01-30")),
                    MovieId = 100,
                    TheaterId = 1,
                    ReservedSeats = 0,
                },
                new ShowTime()
                {
                    Id = 101,
                    StartTime = Convert.ToDateTime(("2024-01-30 14:30:00")),
                    Date = Convert.ToDateTime(("2024-01-30")),
                    MovieId = 101,
                    TheaterId = 2,
                    ReservedSeats = 0
                },
                new ShowTime()
                {
                    Id = 103,
                    StartTime = Convert.ToDateTime(("2024-01-30 18:30:00")),
                    Date = Convert.ToDateTime(("2024-01-30")),
                    MovieId = 100,
                    TheaterId = 1,
                    ReservedSeats = 0
                }
            };

            //mock.Setup(m => m.GetMoviesAsync()).ReturnsAsync(movies);
            mock.Setup(m => m.GetShowTimeAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => showTimes.FirstOrDefault(o => o.Id == id));

            //mock.Setup(m => m.GetShowTimesByMovieAsync(It.IsAny<string>()))
            //    .ReturnsAsync((string movieName) => showTimes.Where(o => o.MovieId == movieRepository.GetMovieAsync(movieName).Result.Id));
            //// .ReturnsAsync((string movieName) => showTimes.FirstOrDefault(o => o.MovieId == GetMovieIdByName(movieName)));

            mock.Setup(m => m.GetShowTimesByMovieAsync(It.IsAny<int>()))
            .ReturnsAsync((int movieId) => showTimes.Where(o => o.MovieId == movieId));
            // .ReturnsAsync((string movieName) => showTimes.FirstOrDefault(o => o.MovieId == GetMovieIdByName(movieName)));

            mock.Setup(m => m.AddShowTime(It.IsAny<ShowTime>()))
                .Callback(() => { return; });

            mock.Setup(m => m.UpdateShowTime(It.IsAny<int>(), It.IsAny<int>()))
               .Callback(() => { return; });

            mock.Setup(m => m.DeleteShowTime(It.IsAny<ShowTime>()))
               .Callback(() => { return; });

            return mock;

        }







    }

}



