using CinemaManagement.Models;

namespace CinemaManagement.Interfaces
{
    public interface ITheaterRepository
    {
        Task<IEnumerable<Theater>> GetTheatersAsync();
        Task<Theater?> GetTheaterById(int theaterId);
        Task<Theater?> GetTheaterByName(string theaterName);
        void AddTheater(Theater theater);
        void UpdateTheater(Theater theater);
        void DeleteTheater(Theater theater);
        Task<bool> SaveChangesAsync();
    }
}
