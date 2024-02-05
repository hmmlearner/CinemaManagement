using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using CinemaManagement.Test.Fixtures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Test
{
    public class ShowTimeRepositoryTests2 : IClassFixture<ShowTimeRepositoryFixture>
    {
        private readonly ShowTimeRepositoryFixture _fixture;

        public ShowTimeRepositoryTests2(ShowTimeRepositoryFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public void AddShowTime_ShouldAddShowTimeToRepository()
        {
            // Arrange
            var showTimeToAdd = new ShowTime { Id = 1, StartTime = DateTime.Now, Date = DateTime.Now.Date, MovieId = 1, TheaterId = 1 };

            // Act
            _fixture.MockRepository.Object.AddShowTime(showTimeToAdd);

            // Assert
            _fixture.MockRepository.Verify(repo => repo.AddShowTime(showTimeToAdd), Times.Once);

        }

        [Fact]
        public async Task UpdateShowTime_ShouldReturnTrue_WhenUpdateSuccessful()
        {
            // Arrange
            var showTimeId = 1;
            var seats = 10;

            // Use the showtimes loaded in the constructor
            var expectedShowTime = _fixture.ShowTimes.FirstOrDefault(st => st.Id == showTimeId);
            _fixture.MockRepository.Setup(repo => repo.UpdateShowTime(showTimeId, seats)).ReturnsAsync(true);

            // Act
            var result = await _fixture.MockRepository.Object.UpdateShowTime(showTimeId, seats);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void DeleteShowTime_ShouldDeleteShowTimeFromRepository()
        {
            // Arrange
            var showTimeToDelete = new ShowTime { Id = 1, StartTime = DateTime.Now, Date = DateTime.Now.Date, MovieId = 1, TheaterId = 1 };

            // Act
            _fixture.MockRepository.Object.DeleteShowTime(showTimeToDelete);

            // Assert
            _fixture.MockRepository.Verify(repo => repo.DeleteShowTime(showTimeToDelete), Times.Once);
        }

        [Fact]
        public async Task GetShowTimeAsync_ShouldReturnShowTime_WhenShowTimeExists()
        {
            // Arrange
            var showTimeId = 1;

            // Use the showtimes loaded in the constructor
            var expectedShowTime = _fixture.ShowTimes.FirstOrDefault(st => st.Id == showTimeId);
            _fixture.MockRepository.Setup(repo => repo.GetShowTimeAsync(showTimeId)).ReturnsAsync(expectedShowTime);

            // Act
            var result = await _fixture.MockRepository.Object.GetShowTimeAsync(showTimeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedShowTime, result);
        }

        [Fact]
        public async Task GetShowTimesByMovieAsync_ShouldReturnListOfShowTimes()
        {
            // Arrange
            var movieId = 100;

            var expectedShowTimes = _fixture.ShowTimes.Where(st => st.MovieId == movieId);

            _fixture.MockRepository.Setup(repo => repo.GetShowTimesByMovieAsync(movieId)).ReturnsAsync(expectedShowTimes);

            // Act
            var result = await _fixture.MockRepository.Object.GetShowTimesByMovieAsync(movieId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedShowTimes, result);
        }

        // Add similar tests for other methods...

        [Fact]
        public async Task SaveChangesAsync_ShouldReturnTrue()
        {
            // Arrange
            _fixture.MockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

            // Act
            var result = await _fixture.MockRepository.Object.SaveChangesAsync();

            // Assert
            Assert.True(result);
        }
    }
}
