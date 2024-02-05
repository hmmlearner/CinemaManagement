using CinemaManagement.Models;

namespace CinemaManagement.Interfaces
{
    public interface IMovieRepository
    {
        Task<Movie?> GetMovieAsync(int movieId);

        Task<Movie?> GetMovieByName(string movieName);
        Task<IEnumerable<Movie>> GetMoviesAsync();

       // IAsyncEnumerable<Movie> GetMoviesAsAsyncEnumerable();

        void UpdateMovie(Movie movieToUpdate);

        void AddMovie(Movie movieToAdd);

        void DeleteMovie(Movie movieToDelete);

        Task<bool> SaveChangesAsync();

    }
}
