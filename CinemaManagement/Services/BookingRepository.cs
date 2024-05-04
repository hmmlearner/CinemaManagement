using CinemaManagement.Data;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CinemaManagement.Services
{

    /// <summary>
    /// Represents a repository for managing bookings.
    /// </summary>
    public class BookingRepository : IBookingRepository
    {
        private readonly CinemaContext _cinemaContext;
        private IDbContextTransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingRepository"/> class.
        /// </summary>
        /// <param name="cinemaContext">The cinema context.</param>
        public BookingRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext ?? throw new ArgumentNullException(nameof(cinemaContext));
        }

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        /// <returns>The database transaction.</returns>
        public async Task<IDbContextTransaction> BeginTransaction()
        {
            _transaction = await _cinemaContext.Database.BeginTransactionAsync();
            return _transaction;
        }

        /// <summary>
        /// Commits the current database transaction.
        /// </summary>
        public void CommitTransaction()
        {
            _transaction?.Commit();
        }

        /// <summary>
        /// Rolls back the current database transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
        }

        /// <summary>
        /// Adds a new booking.
        /// </summary>
        /// <param name="bookingToAdd">The booking to add.</param>
        public void AddBooking(Booking bookingToAdd)
        {
            if (bookingToAdd == null)
            {
                throw new ArgumentNullException(nameof(bookingToAdd));
            }

            var existingUser = _cinemaContext.Users.FirstOrDefault(u => u.Id == bookingToAdd.UserId);
            if (existingUser == null)
            {
                existingUser = new User
                {
                    Id = bookingToAdd.UserId
                };
                _cinemaContext.Users.Add(existingUser);
            }
            bookingToAdd.User = existingUser;
            _cinemaContext.Add(bookingToAdd);
        }

        /// <summary>
        /// Adds picked seats to a booking.
        /// </summary>
        /// <param name="pickedSeats">The seats to add.</param>
        /// <param name="bookingId">The booking ID.</param>
        public void AddSeatsToBooking(List<Seat> pickedSeats, int bookingId)
        {
            if (pickedSeats == null)
            {
                throw new ArgumentNullException(nameof(pickedSeats));
            }
            if (bookingId == 0)
            {
                throw new ArgumentNullException(nameof(bookingId));
            }
            foreach (var seat in pickedSeats)
            {
                seat.BookingId = bookingId;
                var (seatRow, seatNumber) = GetSeatRowAndNumber(seat.SeatRowNumber);
                seat.SeatNo = seatNumber;
                seat.SeatRow = seatRow;
                _cinemaContext.Seats.Add(seat);
            }
        }

        /// <summary>
        /// Gets the seat row and number from the seat row string.
        /// </summary>
        /// <param name="seatRow">The seat row string.</param>
        /// <returns>The seat row and number.</returns>
        private (string, int) GetSeatRowAndNumber(string seatRow)
        {
            string charSeatRow;
            int seatNumber;
            Match match = Regex.Match(seatRow, @"([a-zA-Z]+)(\d+)");
            if (match.Success)
            {
                // Extract character and number parts
                charSeatRow = match.Groups[1].Value;
                seatNumber = int.Parse(match.Groups[2].Value);
            }
            else
            {
                throw new ArgumentException("Invalid seat row format");
            }
            return (charSeatRow, seatNumber);
        }

        /// <summary>
        /// Updates a booking.
        /// </summary>
        /// <param name="bookingToUpdate">The booking to update.</param>
        public void UpdateBooking(Booking bookingToUpdate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes old bookings before the specified cutoff time.
        /// </summary>
        /// <param name="cutoffTime">The cutoff time.</param>
        /// <returns>A boolean indicating if the operation was successful.</returns>
        public async Task<bool> DeleteOldBookings(DateTime cutoffTime)
        {
            var queryResult = await _cinemaContext.Bookings.Where(b => b.CreatedDateTime < cutoffTime).ToListAsync();

            foreach (var booking in queryResult)
            {
                _cinemaContext.Remove(booking);
            }
            return true;
        }

        /// <summary>
        /// Gets a booking by ID.
        /// </summary>
        /// <param name="bookingId">The booking ID.</param>
        /// <returns>The booking.</returns>
        public async Task<Booking> GetBookingAsync(int bookingId)
        {
            return await _cinemaContext.Bookings
                .Include(s => s.Showtime)
                .Include(u => u.User)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        /// <summary>
        /// Gets a booking confirmation by ID.
        /// </summary>
        /// <param name="bookingId">The booking ID.</param>
        /// <returns>The booking confirmation.</returns>
        public async Task<Booking> GetBookingConfirmationAsync(int bookingId)
        {
            return await _cinemaContext.Bookings
                .Include(s => s.Showtime)
                .Include(u => u.User)
                .Include(s => s.Showtime.Movie)
                .Include(t => t.Showtime.Theater)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        /// <summary>
        /// Gets the seats for a booking.
        /// </summary>
        /// <param name="bookingId">The booking ID.</param>
        /// <returns>The seats for the booking.</returns>
        public async Task<IEnumerable<Seat>> GetSeatsForBookingAsync(int bookingId)
        {
            var seats = await _cinemaContext.Seats.Where(s => s.BookingId == bookingId).ToListAsync();
            return seats;
        }

        //not needed for now
        public async Task<IEnumerable<Booking>> GetBookingsForShowTimeAsync(int showTimeId)
        {
            return await _cinemaContext.Bookings
               .Include(s => s.Showtime)
               .Where(s => s.ShowtimeId == showTimeId).ToListAsync();
        }

        /// <summary>
        /// Gets the non-available seats for a showtime.
        /// </summary>
        /// <param name="showTimeId">The showtime ID.</param>
        /// <returns>The non-available seats.</returns>
        public async Task<IEnumerable<Seat>> GetNonAvailableSeatsForShowTimeAsync(int showTimeId)
        {
            var confirmedBookings = await _cinemaContext.Bookings
                .Include(s => s.Showtime)
                .Where(s => s.ShowtimeId == showTimeId && s.BookingConfirmed)
                .Select(b => b.Id)
                .ToListAsync();

            var nonAvailableSeats = await _cinemaContext.Seats
                .Include(s => s.Booking)
                .Where(s => confirmedBookings.Contains(s.BookingId))
                .ToListAsync();

            return nonAvailableSeats;
        }

        /// <summary>
        /// Checks if the seats are available for a showtime.
        /// </summary>
        /// <param name="pickedSeats">The seats to check.</param>
        /// <param name="showTimeId">The showtime ID.</param>
        /// <returns>A boolean indicating if the seats are available.</returns>
        public async Task<bool> CheckIfSeatsAreAvailable(List<Seat> pickedSeats, int showTimeId)
        {
            var pickedSeatNumbers = pickedSeats.Select(s => s.SeatRowNumber).Distinct();

            var queryResult = await _cinemaContext.Seats
                .Include(s => s.Booking)
                .AnyAsync(s => s.Booking.ShowtimeId == showTimeId && pickedSeatNumbers.Contains(s.SeatRowNumber));

            return !queryResult;
        }

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <returns>A boolean indicating if the changes were saved successfully.</returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _cinemaContext.SaveChangesAsync() > 0);
        }
    }
}
