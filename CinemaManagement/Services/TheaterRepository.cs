using CinemaManagement.Data;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;

namespace CinemaManagement.Services
{
    public class TheaterRepository : ITheaterRepository
    {
        private readonly CinemaContext _cinemaContext;

        public TheaterRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }
        public void AddTheater(Theater theater)
        {
            if (theater == null)
            {
                throw new ArgumentNullException(nameof(theater));
            }

            _cinemaContext.Add(theater);
        }

        public void DeleteTheater(Theater theaterToRemove)
        {
            if (theaterToRemove == null)
            {
                throw new ArgumentNullException(nameof(theaterToRemove));
            }

            _cinemaContext.Remove(theaterToRemove);
        }
        public async Task<IEnumerable<Theater>> GetTheatersAsync()
        {
            return await _cinemaContext.Theaters.ToListAsync();
        }

        public async Task<Theater?> GetTheaterById(int theaterId)
        {
            return await _cinemaContext.Theaters.FirstOrDefaultAsync(t => t.Id == theaterId);
        }

        public async Task<Theater?> GetTheaterByName(string theaterName)
        {
            return await _cinemaContext.Theaters.FirstOrDefaultAsync(t => t.Name == theaterName);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _cinemaContext.SaveChangesAsync() > 0);
        }

        public void UpdateTheater(Theater theater)
        {
            throw new NotImplementedException();
        }
    }
}
