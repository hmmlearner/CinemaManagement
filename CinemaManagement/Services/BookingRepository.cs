using CinemaManagement.Data;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace CinemaManagement.Services
{
    public class BookingRepository: IBookingRepository
    {

        private readonly CinemaContext _cinemaContext;
        private IDbContextTransaction _transaction;
        public BookingRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext ?? throw new ArgumentNullException(nameof(cinemaContext));
        }

        public async Task<IDbContextTransaction> BeginTransaction()
        {
            _transaction = await _cinemaContext.Database.BeginTransactionAsync();
            return _transaction;
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
        }

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

        public void AddSeatsToBooking(List<Seat> pickedSeats, int bookingId)
        {
            if (pickedSeats == null)
            {
                throw new ArgumentNullException(nameof(pickedSeats));
            }
            if (bookingId == 0) {
                throw new ArgumentNullException(nameof(bookingId));
            }
            foreach(var seat in pickedSeats)
            {
                seat.BookingId = bookingId;
                var (seatRow, seatNumber) = GetSeatRowAndNumber(seat.SeatRowNumber);
                seat.SeatNo = seatNumber;
                seat.SeatRow = seatRow; 
                _cinemaContext.Seats.Add(seat);
            }

        }

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


            public void UpdateBooking(Booking bookingToUpdate)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteOldBookings(DateTime cutoffTime)
        {
            var queryResult = await _cinemaContext.Bookings.Where(b => b.CreatedDateTime < cutoffTime).ToListAsync();

            foreach (var booking in queryResult)
            {
                _cinemaContext.Remove(booking);
            }
            return true;
        }

        public async Task<Booking> GetBookingAsync(int bookingId)
        {
            return await _cinemaContext.Bookings
                .Include(s => s.Showtime)
                .Include(u => u.User)   
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<Booking> GetBookingConfirmationAsync(int bookingId)
        {
            return await _cinemaContext.Bookings
                .Include(s => s.Showtime)
                .Include(u => u.User)
                .Include(s => s.Showtime.Movie)
                .Include(t => t.Showtime.Theater)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

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


        public async Task<bool> CheckIfSeatsAreAvailable(List<Seat> pickedSeats, int showTimeId)
        {
            var pickedSeatNumbers = pickedSeats.Select(s => s.SeatRowNumber).Distinct();

            var queryResult = await _cinemaContext.Seats
                .Include(s => s.Booking)
                .AnyAsync(s => s.Booking.ShowtimeId == showTimeId && pickedSeatNumbers.Contains(s.SeatRowNumber));

            return !queryResult;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _cinemaContext.SaveChangesAsync() > 0);
        }

        // need to support transactions



    }
}
