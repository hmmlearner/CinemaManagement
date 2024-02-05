using Moq;
using CinemaManagement.Models;
using CinemaManagement.Tests.Mocks;
using CinemaManagement.Test.Utilities;


namespace CinemaManagement.Tests
{
    public class MovieRepositoryTests
    {
        /*
        [Fact]
        public async Task GetAllMoviesAsync_ShouldReturnMovies()
        {
            // Arrange
            var mockRepository = MockIMovieRepository.GetMock();
            //below seems to be right because we have already mocked the repository
            // Act
            var result = await mockRepository.Object.GetMoviesAsync();

            // Assert
            Assert.NotNull(result); 
            // assert there are 2 movies in the result
            Assert.Equal(2, result.Count());
        }


        [Theory]
        [InlineData(100, "Wish", 95, "Fantasy", 23.95, "2023-12-28")]
        [InlineData(101, "Wonka", 116, "Family", 23.95, "2023-12-18")]
        public async Task GetMovieById_ShouldReturnCorrectMovie(int id, string title, int duration, string genre, double ticketPrice, DateTime releaseDate)
        {
            // Arrange
            var mockRepository = MockIMovieRepository.GetMock();
            var expectedMovie = new Movie
            {
                Id = id,
                Title = title,
                Duration = duration,
                Genre = genre,
                TicketPrice = ticketPrice
            };
            mockRepository.Setup(repo => repo.GetMovieAsync(id)).ReturnsAsync(expectedMovie);
            // Act
            var result = await mockRepository.Object.GetMovieAsync(id);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedMovie, result);
        }

        [Theory]
        [InlineData("Wish", 100, 95, "Fantasy", 23.95, "2023-12-28")]
        [InlineData("Wonka", 101, 116, "Family", 23.95, "2023-12-18")]
        public async Task GetMovieByName_ShouldReturnCorrectMovie(string title, int id, int duration, string genre, double ticketPrice, DateTime releaseDate)
        {
            // Arrange
            var mockRepository = MockIMovieRepository.GetMock();
            var expectedMovie = new Movie
            {
                Id = id,
                Title = title,
                Duration = duration,
                Genre = genre,
                TicketPrice = ticketPrice
            };
            mockRepository.Setup(repo => repo.GetMovieByName(title)).ReturnsAsync(expectedMovie);
            // Act
            var result = await mockRepository.Object.GetMovieByName(title);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedMovie, result);
        }


        [Theory]
        [InlineData("The Iron Claw", 102, "Follows the story of the Von Erichs, a dynasty of wrestlers who made a great impact on the sport from the 1960s to the present day.",
            132, "Drama", 23.95, "2023-01-31")]
        public async Task AddNewMovie_ShouldAddNewMovieToRepository(string title, int id, string description, int duration, string genre, double ticketPrice, DateTime releaseDate)
        {
            // Arrange
            var mockRepository = MockIMovieRepository.GetMock();
            var newMovie = new Movie
            {
                Id = id,
                Title = title,
                Description = description,
                Duration = duration,
                Genre = genre,
                TicketPrice = ticketPrice
            };

            mockRepository.Setup(repo => repo.AddMovie(It.IsAny<Movie>()))
                .Callback<Movie>(async newMovie =>
                {
                    var movies = await mockRepository.Object.GetMoviesAsync();//.Result;
                    movies.Append(newMovie);
                   // movies.Add(newMovie);
                });
            // Act
             mockRepository.Object.AddMovie(newMovie);

            // Assert
            var result = await mockRepository.Object.GetMoviesAsync();
            Assert.Contains(newMovie, result, new MovieEqualityComparer());

        }


        [Theory]
        [InlineData(100, "Wish", "Asha, a sharp-witted idealist, makes a wish so powerful that it is answered by a cosmic force—a little ball of boundless energy called Star.",200, "Family", 33.95, "2023-02-01")]
        [InlineData(101, "Wonka", "Based on the extraordinary character at the center of Charlie and the Chocolate Factory, Roald Dahl’s most iconic children’s book and one of the best-selling children’s books of all time.",300, "Family", 43.95, "2023-01-31")]
        public async Task UpdateMovie_ShouldUpdateMoveCorrectlyInRepository(int id, string title, string description, int duration, string genre, double ticketPrice, DateTime releaseDate)
        {
            // Arrange
            var mockRepository = MockIMovieRepository.GetMock();
            var updatedMovie = new Movie
            {
                Id = id,
                Title = title,
                Description = description,
                Duration = duration,
                ReleaseDate=releaseDate,
                Genre = genre,
                TicketPrice = ticketPrice
            };

            mockRepository.Setup(repo => repo.UpdateMovie(It.IsAny<Movie>()))
               .Callback<Movie>(async upatedMovie =>
               {
                   var existingMovie = await mockRepository.Object.GetMovieAsync(id);
                   if (existingMovie != null)
                   {
                       // Update properties of the existing movie
                       existingMovie.Duration = updatedMovie.Duration;
                       existingMovie.Genre = updatedMovie.Genre;
                       existingMovie.TicketPrice = updatedMovie.TicketPrice;
                       existingMovie.ReleaseDate = updatedMovie.ReleaseDate;
                   }
               });
            // Act
             mockRepository.Object.UpdateMovie(updatedMovie);
            // Assert
            var result = await mockRepository.Object.GetMovieAsync(id);
            Assert.NotNull(result);
            Assert.Equal(updatedMovie, result, new MovieEqualityComparer());
        }

        [Theory]
        [InlineData("The Iron Claw", 102, "Follows the story of the Von Erichs, a dynasty of wrestlers who made a great impact on the sport from the 1960s to the present day.",
            132, "Drama", 23.95, "2023-01-31")]
        public async Task DeleteMovie_ShouldDeleteMoveFromRepository(string title, int id, string description, int duration, string genre, double ticketPrice, DateTime releaseDate)
        {
            // Arrange
            var mockRepository = MockIMovieRepository.GetMock();
            var movieToBeDeleted = new Movie
            {
                Id = id,
                Title = title,
                Description = description,
                Duration = duration,
                Genre = genre,
                TicketPrice = ticketPrice

            };
            mockRepository.Setup(repo => repo.DeleteMovie(It.IsAny<Movie>()))
               .Callback<Movie>(async movieToBeDeleted =>
               {
                   var movies = (await mockRepository.Object.GetMoviesAsync()).ToList();
                   var existingMovie = movies.FirstOrDefault(m => m.Id == movieToBeDeleted.Id);
                   if (existingMovie != null)
                   {
                       // need to remove existingMovie from movies
                       movies.Remove(existingMovie);
                   }
               });
            // Act
             mockRepository.Object.DeleteMovie(movieToBeDeleted);
            var result = await mockRepository.Object.GetMoviesAsync();
            // Assert
            Assert.DoesNotContain(movieToBeDeleted, result);
            
        }
        */
    }


}