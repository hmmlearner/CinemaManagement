using CinemaManagement.Data;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{

    /// <summary>
    /// Repository class for managing show times.
    /// </summary>
    public class ShowTimeRepository : IShowTimeRepository
    {
        const int CLEANING_TIME = 30;
        private readonly CinemaContext _cinemaContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowTimeRepository"/> class.
        /// </summary>
        /// <param name="cinemaContext">The cinema context.</param>
        public ShowTimeRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        /// <summary>
        /// Adds a new show time.
        /// </summary>
        /// <param name="showTime">The show time to add.</param>
        public void AddShowTime(ShowTime showTime)
        {
            if (showTime == null)
            {
                throw new ArgumentNullException(nameof(showTime));
            }

            _cinemaContext.Add(showTime);
        }

        /// <summary>
        /// Deletes a show time.
        /// </summary>
        /// <param name="showTimeToDelete">The show time to delete.</param>
        public void DeleteShowTime(ShowTime showTimeToDelete)
        {
            if (showTimeToDelete == null)
            {
                throw new ArgumentNullException(nameof(showTimeToDelete));
            }

            _cinemaContext.Remove(showTimeToDelete); // should not be a casaade delete
        }

        /// <summary>
        /// Gets all show times.
        /// </summary>
        /// <returns>An asynchronous operation that returns a collection of show times.</returns>
        public Task<IEnumerable<ShowTime>> GetAllShowTimes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a show time by its ID.
        /// </summary>
        /// <param name="showTimeId">The ID of the show time.</param>
        /// <returns>An asynchronous operation that returns the show time.</returns>
        public async Task<ShowTime> GetShowTimeAsync(int showTimeId)
        {
            return await _cinemaContext.Showtimes
                .Include(s => s.Movie)
                .Include(t => t.Theater)
                .FirstOrDefaultAsync(s => s.Id == showTimeId);
        }

        /// <summary>
        /// Gets show times by movie name.
        /// </summary>
        /// <param name="movieName">The name of the movie.</param>
        /// <returns>An asynchronous operation that returns a collection of show times.</returns>
        public async Task<IEnumerable<ShowTime>> GetShowTimesByMovieAsync(string movieName)
        {
            var movie = await _cinemaContext.Movies.FirstOrDefaultAsync(m => m.Title == movieName);

            return await _cinemaContext.Showtimes
                .Include(s => s.Movie)
                .Include(t => t.Theater)
                .Where(s => s.MovieId == movie.Id)
                .ToListAsync();
        }

        /// <summary>
        /// Gets show times by movie ID.
        /// </summary>
        /// <param name="movieId">The ID of the movie.</param>
        /// <returns>An asynchronous operation that returns a collection of show times.</returns>
        public async Task<IEnumerable<ShowTime>> GetShowTimesByMovieAsync(int movieId)
        {
            var movie = await _cinemaContext.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

            return await _cinemaContext.Showtimes
                .Include(s => s.Movie)
                .Include(t => t.Theater)
                .Where(s => s.MovieId == movie.Id)
                .ToListAsync();
        }

        /// <summary>
        /// Gets show times by start time, date, theater, and duration.
        /// </summary>
        /// <param name="showTimeStartTime">The start time of the show time.</param>
        /// <param name="showTimeDate">The date of the show time.</param>
        /// <param name="theaterID">The ID of the theater.</param>
        /// <param name="duration">The duration of the show time.</param>
        /// <returns>An asynchronous operation that returns a collection of show times.</returns>
        public async Task<IEnumerable<ShowTime>> GetShowTimeByStartTimeAndDateAndTheater(DateTime showTimeStartTime, DateTime showTimeDate, int theaterID, int duration)
        {

            return await _cinemaContext.Showtimes
                .Where(s => s.TheaterId == theaterID
                && s.Date == showTimeDate
                && s.StartTime > showTimeStartTime
                && s.StartTime <= showTimeStartTime.AddMinutes(duration + CLEANING_TIME))
                .ToListAsync();
        }

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <returns>An asynchronous operation that returns a boolean indicating whether the changes were saved successfully.</returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _cinemaContext.SaveChangesAsync() > 0);
        }

        /// <summary>
        /// Updates the number of reserved seats for a show time.
        /// </summary>
        /// <param name="showTimeId">The ID of the show time.</param>
        /// <param name="seats">The number of seats to update.</param>
        /// <returns>An asynchronous operation that returns a boolean indicating whether the update was successful.</returns>
        public async Task<bool> UpdateShowTime(int showTimeId, int seats)
        {
            var showTime = await GetShowTimeAsync(showTimeId);
            if (showTime == null)
            {
                throw new ArgumentNullException(nameof(showTime));
            }
            showTime.ReservedSeats += seats;
            //need code to update seats in showtime table
            _cinemaContext.Update(showTime);


            return true;
        }
    }
}
