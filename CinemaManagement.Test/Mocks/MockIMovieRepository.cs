using Moq;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;



namespace CinemaManagement.Tests.Mocks
{
    public class MockIMovieRepository: IDisposable
    {
        public static Mock<IMovieRepository> GetMock()
        {
            var mock = new Mock<IMovieRepository>();
            var movies = new List<Movie>()
            {
                new Movie()
                {
                    Id = 100,
                    Title = "Wish",
                    Description = "Asha, a sharp-witted idealist, makes a wish so powerful that it is answered by a cosmic force—a little ball of boundless energy called Star.",
                    Duration = 95,
                    Genre = "Fantasy",
                    ReleaseDate = Convert.ToDateTime("2023-12-28"),
                    TicketPrice = 23.95
                },
                new Movie()
                {
                    Id = 101,
                    Title = "Wonka",
                    Description = "Based on the extraordinary character at the center of Charlie and the Chocolate Factory, Roald Dahl’s most iconic children’s book and one of the best-selling children’s books of all time.",
                    Duration = 116,
                    Genre = "Family",
                    ReleaseDate = Convert.ToDateTime("2023-12-18"),
                    TicketPrice = 25.95
                },
        };

            mock.Setup(m => m.GetMoviesAsync()).ReturnsAsync(movies);
            
            mock.Setup(m => m.GetMovieAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) => movies.FirstOrDefault(o => o.Id == id));

            mock.Setup(m => m.GetMovieByName(It.IsAny<string>()))
               .ReturnsAsync((string name) => movies.FirstOrDefault(o => o.Title == name));

            mock.Setup(m => m.AddMovie(It.IsAny<Movie>()))
                .Callback(() => { return; });

            mock.Setup(m => m.UpdateMovie(It.IsAny<Movie>()))
               .Callback(() => { return; });

            mock.Setup(m => m.DeleteMovie(It.IsAny<Movie>()))
               .Callback(() => { return; });
            return mock;

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}


/*
 
public class MovieRepositoryFixture
    {
        public Mock<IMovieRepository> MockRepository { get; }
        public IMovieRepository Repository => MockRepository.Object;

        public MovieRepositoryFixture()
        {
            MockRepository = new Mock<IMovieRepository>();
        }
    }



public class MovieRepositoryTestsFixture : IClassFixture<MovieRepositoryFixture>
    {
        private readonly MovieRepositoryFixture _fixture;

        public MovieRepositoryTestsFixture(MovieRepositoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetMovieAsync_ShouldReturnMovie_WhenMovieExists()
        {
            // Arrange
            var movieId = 1;
            var expectedMovie = new Movie { Id = movieId, Title = "Test Movie" };
            _fixture.MockRepository.Setup(repo => repo.GetMovieAsync(movieId)).ReturnsAsync(expectedMovie);

            // Act
            var result = await _fixture.Repository.GetMovieAsync(movieId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedMovie, result);
        }
}



*/