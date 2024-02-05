using CinemaManagement.Models;
using CinemaManagement.Test.Utilities;
using CinemaManagement.Tests.Mocks;
using Moq;


namespace CinemaManagement.Tests
{
    public class ShowTimeRepositoryTests
    {
        /*
        [Theory]
        [InlineData(100, "2024-01-30 14:30:00", "2024-01-30", 100, 1, 0)]
        [InlineData(101, "2024-01-30 14:30:00", "2024-01-30", 101, 2, 0)]
        public async Task GetShowTimeAsync_shouldReturnCorrectShowTime(int id, DateTime startTime, DateTime date, int movieId, int theaterId, int reserveseats)
        {
            //arrange
            var mockRepository = MockIShowTimeRepository.GetMock(new MockIMovieRepository());
            var expectedShowTime = new ShowTime
            {
                Id = id,
                StartTime = startTime,
                Date = date,
                MovieId = movieId,
                TheaterId = theaterId,
                ReservedSeats = reserveseats
            };
            mockRepository.Setup(repo => repo.GetShowTimeAsync(id)).ReturnsAsync(expectedShowTime);
            //act
            var result = await mockRepository.Object.GetShowTimeAsync(id);
            //assert
            Assert.Equal(result, expectedShowTime);
        }

        [Fact]
        public async Task GetShowTimesForMovieAsync_shouldReturnAllShowTimesForMovie()
        {
            //arrange
            var mockRepository = MockIShowTimeRepository.GetMock(new MockIMovieRepository());
            //act
            var result = await mockRepository.Object.GetShowTimesByMovieAsync(100);// can this come from InlineData
            //assert
            Assert.Equal(2, result.Count());
        }

        [Theory]
        [InlineData(101, "2024-01-30 14:30:00", "2024-01-30", 101, 2, 0)]
        public async Task AddNewShowTime_ShouldAddNewShowTimeToRepository(int id, DateTime startTime, DateTime date, int movieId, int theaterId, int reserveseats)
        {
            // Arrange
            var mockRepository = MockIShowTimeRepository.GetMock(new MockIMovieRepository());
            var newShowTime = new ShowTime
            {
                Id = id,
                StartTime = startTime,
                Date = date,
                MovieId = movieId,
                TheaterId = theaterId,
                ReservedSeats = reserveseats
            };

            mockRepository.Setup(repo => repo.AddShowTime(It.IsAny<ShowTime>()))
                .Callback<ShowTime>(async newShowTime =>
                {
                    var movieShowTimes = await mockRepository.Object.GetShowTimesByMovieAsync(movieId);
                    movieShowTimes.Append(newShowTime);
                    //movieShowTimes.Add(newShowTime);
                });
            // Act
             mockRepository.Object.AddShowTime(newShowTime);
            // Assert
            var result = await mockRepository.Object.GetShowTimesByMovieAsync(movieId);
            Assert.Contains(newShowTime, result);//, new MovieEqualityComparer());
        }

        [Theory]
        [InlineData(100, "2024-01-30 14:30:00", "2024-01-30", 100, 1, 5)]
        [InlineData(101, "2024-01-30 14:30:00", "2024-01-30", 101, 2, 10)]
        public async Task UpdateMovie_ShouldUpdateMoveCorrectlyInRepository(int id, DateTime startTime, DateTime date, int movieId, int theaterId, int reserveseats)
        {
            // Arrange
            var mockRepository = MockIShowTimeRepository.GetMock(new MockIMovieRepository());
            var updatedShowTime = new ShowTime
            {
                Id = id,
                StartTime = startTime,
                Date = date,
                MovieId = movieId,
                TheaterId = theaterId,
                ReservedSeats = reserveseats
            };
            mockRepository.Setup(repo => repo.UpdateShowTime(It.IsAny<int>(), It.IsAny<int>()))
               .Callback<int,int>(async (showTimeId, reservedSeats) =>
               {
                   var existingShowTime = await mockRepository.Object.GetShowTimeAsync(showTimeId);
                   if (existingShowTime != null)
                   {
                       // Update properties of the existing movie
                       //existingShowTime.StartTime = updatedShowTime.StartTime;
                       //existingShowTime.Date = updatedShowTime.Date;
                       //existingShowTime.TicketPrice = updatedMovie.TicketPrice;
                       existingShowTime.ReservedSeats = reservedSeats;
                   }
               });
            // Act
            mockRepository.Object.UpdateShowTime(id, reserveseats);
            // Assert
            var result = await mockRepository.Object.GetShowTimeAsync(id);
            Assert.NotNull(result);
            Assert.Equal(result, updatedShowTime);// , new MovieEqualityComparer());




        }
        */
    }

}