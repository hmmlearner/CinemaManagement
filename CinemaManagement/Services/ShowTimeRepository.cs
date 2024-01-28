using CinemaManagement.Data;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.Services
{

    public class ShowTimeRepository : IShowTimeRepository
    {
        const int CLEANING_TIME = 30;
        private readonly CinemaContext _cinemaContext;
        public ShowTimeRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }
        public void AddShowTime(ShowTime showTime)
        {
            if (showTime == null)
            {
                throw new ArgumentNullException(nameof(showTime));
            }

            _cinemaContext.Add(showTime);
        }

        public void DeleteShowTime(ShowTime showTimeToDelete)
        {
            if (showTimeToDelete == null)
            {
                throw new ArgumentNullException(nameof(showTimeToDelete));
            }

            _cinemaContext.Remove(showTimeToDelete); // should not be a casaade delete
        }

        public Task<IEnumerable<ShowTime>> GetAllShowTimes()
        {
            throw new NotImplementedException();
        }

        public async Task<ShowTime> GetShowTimeAsync(int showTimeId)
        {
            return await _cinemaContext.Showtimes
                .Include(s => s.Movie)
                .Include(t => t.Theater)
                .FirstOrDefaultAsync(s => s.Id == showTimeId);
        }

        public async Task<IEnumerable<ShowTime>> GetShowTimesByMovieAsync(string movieName)
        {
            var movie = await _cinemaContext.Movies.FirstOrDefaultAsync(m => m.Title == movieName);

            return await _cinemaContext.Showtimes
                .Include(s => s.Movie)
                .Include(t => t.Theater)
                .Where(s => s.MovieId == movie.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<ShowTime>> GetShowTimesByMovieAsync(int movieId)
        {
            var movie = await _cinemaContext.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

            return await _cinemaContext.Showtimes
                .Include(s => s.Movie)
                .Include(t => t.Theater)
                .Where(s => s.MovieId == movie.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<ShowTime>> GetShowTimeByStartTimeAndDateAndTheater(DateTime showTimeStartTime, DateTime showTimeDate, int theaterID, int duration)
        {

            return await _cinemaContext.Showtimes
                .Where(s => s.TheaterId == theaterID 
                && s.Date == showTimeDate
                && s.StartTime > showTimeStartTime
                && s.StartTime <= showTimeStartTime.AddMinutes(duration+ CLEANING_TIME))
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _cinemaContext.SaveChangesAsync() > 0);
        }

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
