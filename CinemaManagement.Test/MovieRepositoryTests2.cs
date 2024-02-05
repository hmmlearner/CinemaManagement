using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using Moq;

namespace CinemaManagement.Tests
{
    public class MovieRepositoryTests2
    {
        [Fact]
        public async Task GetMovieAsync_ShouldReturnMovie_WhenMovieExists()
        {
            // Arrange
            var movieId = 1;
            var mockRepository = new Mock<IMovieRepository>();
            var expectedMovie = new Movie { Id = movieId, Title = "Test Movie" };
            mockRepository.Setup(repo => repo.GetMovieAsync(movieId)).ReturnsAsync(expectedMovie);

            // Act
            var result = await mockRepository.Object.GetMovieAsync(movieId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedMovie, result);
        }

        [Fact]
        public async Task GetMovieAsync_ShouldReturnNull_WhenMovieDoesNotExist()
        {
            // Arrange
            var movieId = 1;
            var mockRepository = new Mock<IMovieRepository>();
            mockRepository.Setup(repo => repo.GetMovieAsync(movieId)).ReturnsAsync((Movie)null);

            // Act
            var result = await mockRepository.Object.GetMovieAsync(movieId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetMovieByName_ShouldReturnMovie_WhenMovieExists()
        {
            // Arrange
            var movieName = "Test Movie";
            var mockRepository = new Mock<IMovieRepository>();
            var expectedMovie = new Movie { Id = 1, Title = movieName };
            mockRepository.Setup(repo => repo.GetMovieByName(movieName)).ReturnsAsync(expectedMovie);

            // Act
            var result = await mockRepository.Object.GetMovieByName(movieName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedMovie, result);
        }

        [Fact]
        public async Task GetMovieByName_ShouldReturnNull_WhenMovieDoesNotExist()
        {
            // Arrange
            var movieName = "Test Movie";
            var mockRepository = new Mock<IMovieRepository>();
            mockRepository.Setup(repo => repo.GetMovieByName(movieName)).ReturnsAsync((Movie)null);

            // Act
            var result = await mockRepository.Object.GetMovieByName(movieName);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetMoviesAsync_ShouldReturnListOfMovies()
        {
            // Arrange
            var movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "Wish", Description = "Asha, a sharp-witted idealist",Duration = 95, Genre = "Fantasy" },
            new Movie { Id = 2, Title = "Wonka", Description = "Extraordinary character at the center of Charlie and the Chocolate Factory",Duration = 100, Genre = "Family" },
            new Movie { Id = 3, Title = "The Iron Claw",Description = "Follows the story of the Von Erichs, a dynasty of wrestlers",Duration = 120, Genre = "Fantasy" }
        };

            var mockRepository = new Mock<IMovieRepository>();
            mockRepository.Setup(repo => repo.GetMoviesAsync()).ReturnsAsync(movies);

            // Act
            var result = await mockRepository.Object.GetMoviesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(movies, result);
        }

        [Fact]
        public void UpdateMovie_ShouldUpdateMovieInRepository()
        {
            // Arrange
            var movieToUpdate = new Movie { Id = 1, Title = "Wish" };
            var mockRepository = new Mock<IMovieRepository>();

            // Act
            mockRepository.Object.UpdateMovie(movieToUpdate);

            // Assert
            mockRepository.Verify(repo => repo.UpdateMovie(movieToUpdate), Times.Once);
        }

        [Fact]
        public void AddMovie_ShouldAddMovieToRepository()
        {
            // Arrange
            var movieToAdd = new Movie { Id = 1, Title = "Wish" };
            var mockRepository = new Mock<IMovieRepository>();

            // Act
            mockRepository.Object.AddMovie(movieToAdd);

            // Assert
            mockRepository.Verify(repo => repo.AddMovie(movieToAdd), Times.Once);
        }

        [Fact]
        public void DeleteMovie_ShouldDeleteMovieFromRepository()
        {
            // Arrange
            var movieToDelete = new Movie { Id = 1, Title = "Wish" };
            var mockRepository = new Mock<IMovieRepository>();

            // Act
            mockRepository.Object.DeleteMovie(movieToDelete);

            // Assert
            mockRepository.Verify(repo => repo.DeleteMovie(movieToDelete), Times.Once);
        }

        [Fact]
        public async Task SaveChangesAsync_ShouldReturnTrue()
        {
            // Arrange
            var mockRepository = new Mock<IMovieRepository>();
            mockRepository.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(true);

            // Act
            var result = await mockRepository.Object.SaveChangesAsync();

            // Assert
            Assert.True(result);
        }
    }
}
