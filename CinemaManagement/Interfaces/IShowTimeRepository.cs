using CinemaManagement.Models;

namespace CinemaManagement.Interfaces
{
    public interface IShowTimeRepository
    {
        void AddShowTime(ShowTime showTime);
        Task<bool> UpdateShowTime(int showTimeId, int seats); //need this to update the StartTime and date
        void DeleteShowTime(ShowTime showTimeToDelete);
        Task<ShowTime> GetShowTimeAsync(int showTimeId);
        Task<IEnumerable<ShowTime>> GetAllShowTimes(); // dont need this
        Task<IEnumerable<ShowTime>> GetShowTimesByMovieAsync(string movieName);

        Task<IEnumerable<ShowTime>> GetShowTimesByMovieAsync(int movieId);
        Task<IEnumerable<ShowTime>> GetShowTimeByStartTimeAndDateAndTheater(DateTime showTimeStartTime, DateTime showTimeDate, int theaterID, int duration);
        Task<bool> SaveChangesAsync();
    }
}
