using CinemaManagement.Data;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{
    public class MovieRepository : IMovieRepository
    {
        private readonly CinemaContext _cinemaContext;

        public MovieRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }
        public void AddMovie(Movie movieToAdd)
        {
            if (movieToAdd == null)
            {
                throw new ArgumentNullException(nameof(movieToAdd));
            }

            _cinemaContext.Add(movieToAdd);
        }

        public void UpdateMovie(Movie movieToUpdate)
        {
            if (movieToUpdate == null)
            {
                throw new ArgumentNullException(nameof(movieToUpdate));
            }
            _cinemaContext.Movies.Update(movieToUpdate);
        }

        public void DeleteMovie(Movie movieToDelete)
        {
              if (movieToDelete == null)
            {
                throw new ArgumentNullException(nameof(movieToDelete));
            }

            _cinemaContext.Remove(movieToDelete);
        }

        public async Task<Movie?> GetMovieAsync(int movieId)
        {
            return await _cinemaContext.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync()
        {
            return await _cinemaContext.Movies.ToListAsync();
        }

        public IAsyncEnumerable<Movie> GetMoviesAsAsyncEnumerable()
        {
            return _cinemaContext.Movies.AsAsyncEnumerable();
        }

        public async Task<Movie?> GetMovieByName(string movieName)
        {
            return await _cinemaContext.Movies.FirstOrDefaultAsync(m => m.Title == movieName);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _cinemaContext.SaveChangesAsync() > 0);
        }
    }
}
