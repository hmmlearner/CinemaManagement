using CinemaManagement.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace CinemaManagement.Interfaces
{
    public interface IBookingRepository
    {


        Task<Booking?> GetBookingAsync(int bookingId);

        Task<IEnumerable<Booking>> GetBookingsForShowTimeAsync(int showTimeId);

        // IAsyncEnumerable<Movie> GetMoviesAsAsyncEnumerable();

        Task<bool> CheckIfSeatsAreAvailable(List<Seat> pickedSeats, int showTimeId);

        void AddSeatsToBooking(List<Seat> pickedSeats, int bookingId);

        void UpdateBooking(Booking bookingToUpdate); // need to update booking with customerid, BookingConfirmed, and ConfirmedDateTime
                                                     // Also need to update showtime ReservedSeats once booking is confirmed

        void AddBooking(Booking bookingToAdd);

        Task<bool> DeleteOldBookings(DateTime cutoffTime);// needs to get excuted after 10 mins of booking CreatedDateTime if BookingConfirmed is false

        Task<bool> SaveChangesAsync();

        //Task<int> CalculateTotalReservedForShowTimeAsync(int showTimeId);
        Task<IEnumerable<Seat>> GetSeatsForBookingAsync(int bookingId);
         
        Task<IEnumerable<Seat>> GetNonAvailableSeatsForShowTimeAsync(int showTimeId);
        Task<Booking> GetBookingConfirmationAsync(int bookingId);

        Task<IDbContextTransaction> BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
