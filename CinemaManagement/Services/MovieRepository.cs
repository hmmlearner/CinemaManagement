using CinemaManagement.Data;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    /// <summary>
    /// Movie repository for handling movie related operations
    /// </summary>
    public class MovieRepository : IMovieRepository
    {
        private readonly CinemaContext _cinemaContext;

        /// <summary>
        /// Initializes a new instance of the MovieRepository class.
        /// </summary>
        /// <param name="cinemaContext">The CinemaContext to be used for movie operations.</param>
        public MovieRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        /// <summary>
        /// Adds a new movie to the repository.
        /// </summary>
        /// <param name="movieToAdd">The movie to add.</param>
        public void AddMovie(Movie movieToAdd)
        {
            if (movieToAdd == null)
            {
                throw new ArgumentNullException(nameof(movieToAdd));
            }

            _cinemaContext.Add(movieToAdd);
        }

        
        /// <summary>
        /// Updates a movie in the repository.
        /// </summary>
        /// <param name="movieToUpdate">The movie to update.</param>
        public void UpdateMovie(Movie movieToUpdate)
        {
            if (movieToUpdate == null)
            {
                throw new ArgumentNullException(nameof(movieToUpdate));
            }
            _cinemaContext.Movies.Update(movieToUpdate);
        }


        /// <summary>
        /// Deletes a movie from the repository.
        /// </summary>
        /// <param name="movieToDelete">The movie to delete.</param>
        public void DeleteMovie(Movie movieToDelete)
        {
            if (movieToDelete == null)
            {
                throw new ArgumentNullException(nameof(movieToDelete));
            }

            _cinemaContext.Remove(movieToDelete);
        }

        /// <summary>
        /// Retrieves a movie from the repository based on the movie ID.
        /// </summary>
        /// <param name="movieId">The ID of the movie to retrieve.</param>
        /// <returns>The movie with the specified ID, or null if not found.</returns>
        public async Task<Movie?> GetMovieAsync(int movieId)
        {
            return await _cinemaContext.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
        }

        /// <summary>
        /// Retrieves all movies from the repository.
        /// </summary>
        /// <returns>An asynchronous task that represents the operation. The task result contains the list of movies.</returns>
        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            return await _cinemaContext.Movies.ToListAsync();
        }

        /// <summary>
        /// Retrieves all movies from the repository as an asynchronous enumerable. Will be used for large dataset in future.
        /// </summary>
        /// <returns>An asynchronous enumerable that represents the operation. The enumerable contains the list of movies.</returns>
        public IAsyncEnumerable<Movie> GetMoviesAsAsyncEnumerable()
        {
            return _cinemaContext.Movies.AsAsyncEnumerable();
        }

        /// <summary>
        /// Retrieves a movie from the repository based on the movie name.
        /// </summary>
        /// <param name="movieName">The name of the movie to retrieve.</param>
        /// <returns>The movie with the specified name, or null if not found.</returns>
        public async Task<Movie?> GetMovieByName(string movieName)
        {
            return await _cinemaContext.Movies.FirstOrDefaultAsync(m => m.Title == movieName);
        }


        /// <summary>
        /// Saves the changes made to the repository asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation. The task result is a boolean indicating whether the changes were successfully saved.</returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _cinemaContext.SaveChangesAsync() > 0);
        }
    }
}
