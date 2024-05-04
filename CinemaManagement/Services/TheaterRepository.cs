using CinemaManagement.Data;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;

namespace CinemaManagement.Services
{

    /// <summary>
    /// Represents a repository for managing theaters.
    /// </summary>
    public class TheaterRepository : ITheaterRepository
    {
        private readonly CinemaContext _cinemaContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheaterRepository"/> class.
        /// </summary>
        /// <param name="cinemaContext">The cinema context.</param>
        public TheaterRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        /// <summary>
        /// Adds a theater to the repository.
        /// </summary>
        /// <param name="theater">The theater to add.</param>
        public void AddTheater(Theater theater)
        {
            if (theater == null)
            {
                throw new ArgumentNullException(nameof(theater));
            }

            _cinemaContext.Add(theater);
        }

        /// <summary>
        /// Deletes a theater from the repository.
        /// </summary>
        /// <param name="theater">The theater to delete.</param>
        public void DeleteTheater(Theater theater)
        {
            if (theater == null)
            {
                throw new ArgumentNullException(nameof(theater));
            }

            _cinemaContext.Remove(theater);
        }

        /// <summary>
        /// Gets all theaters from the repository asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of theaters.</returns>
        public async Task<IEnumerable<Theater>> GetTheatersAsync()
        {
            return await _cinemaContext.Theaters.ToListAsync();
        }

        /// <summary>
        /// Gets a theater by its ID from the repository asynchronously.
        /// </summary>
        /// <param name="theaterId">The ID of the theater.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the theater, or null if not found.</returns>
        public async Task<Theater?> GetTheaterById(int theaterId)
        {
            return await _cinemaContext.Theaters.FirstOrDefaultAsync(t => t.Id == theaterId);
        }

        /// <summary>
        /// Gets a theater by its name from the repository asynchronously.
        /// </summary>
        /// <param name="theaterName">The name of the theater.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the theater, or null if not found.</returns>
        public async Task<Theater?> GetTheaterByName(string theaterName)
        {
            return await _cinemaContext.Theaters.FirstOrDefaultAsync(t => t.Name == theaterName);
        }

        /// <summary>
        /// Saves changes made to the repository asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the changes were saved successfully.</returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _cinemaContext.SaveChangesAsync() > 0);
        }

        /// <summary>
        /// Updates a theater in the repository.
        /// </summary>
        /// <param name="theater">The theater to update.</param>
        public void UpdateTheater(Theater theater)
        {
            throw new NotImplementedException();
        }
    }
}
