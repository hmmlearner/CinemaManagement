using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using CinemaManagement.Test.Fixtures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CinemaManagement.Test
{
    public class BookingRepositoryTests2 : IClassFixture<BookingRepositoryFixture>
    {

            private readonly BookingRepositoryFixture _fixture;

            public BookingRepositoryTests2(BookingRepositoryFixture fixture)
            {
                _fixture = fixture;
            }


            [Fact]
            public void AddBooking_ShouldAddBookingToRepository()
            {


                // Arrange
                var bookingToAdd = new Booking { Id = 4, ShowtimeId = 1, UserId = 3, NoOfSeats = 2, TotalPrice = 20.0, BookingConfirmed = false, CreatedDateTime = DateTime.Now };

                // Act
                _fixture.MockRepository.Object.AddBooking(bookingToAdd);

                // Assert
                _fixture.MockRepository.Verify(repo => repo.AddBooking(bookingToAdd), Times.Once);

            }

        //void AddSeatsToBooking(List<Seat> pickedSeats, int bookingId);

            [Fact]
            public void AddSeatsBooking_ShouldAddSeatsToBookingToRepository()
            {
                var bookingId = 1;
                var pickedSeats = new List<Seat> { 
                        new Seat { Id = 1, BookingId = 1, SeatRow = "D", SeatNo = 7, SeatRowNumber= "D7", IsAvailable = true },
                        new Seat { Id = 2, BookingId = 1, SeatRow = "D", SeatNo = 8, SeatRowNumber= "D8", IsAvailable = true },
                };
                // Act
                _fixture.MockRepository.Object.AddSeatsToBooking(pickedSeats, bookingId);

                // Assert
                _fixture.MockRepository.Verify(repo => repo.AddSeatsToBooking(pickedSeats, bookingId), Times.Once);

            }

        [Fact]
            public async Task GetBookingAsync_ShouldReturnBooking_WhenBookingExists()
            {
                // Arrange
                var bookingId = 1;
            //var mockRepository = new Mock<IBookingRepository>();
            //var expectedBooking = new Booking { Id = bookingId, ShowtimeId = 1, UserId = 1, NoOfSeats = 2, TotalPrice = 20.0, BookingConfirmed = true, CreatedDateTime = DateTime.Now };
                var expectedBooking = _fixture.Bookings.FirstOrDefault(st => st.Id == bookingId);
                _fixture.MockRepository.Setup(repo => repo.GetBookingAsync(bookingId)).ReturnsAsync(expectedBooking);

                // Act
                var result = await _fixture.MockRepository.Object.GetBookingAsync(bookingId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expectedBooking, result);
            }

            [Fact]
            public async Task GetBookingAsync_ShouldReturnNull_WhenBookingDoesNotExist()
            {
                // Arrange
                var bookingId = 100;
            // var mockRepository = new Mock<IBookingRepository>();
                _fixture.MockRepository.Setup(repo => repo.GetBookingAsync(bookingId)).ReturnsAsync((Booking)null);

                // Act
                var result = await _fixture.MockRepository.Object.GetBookingAsync(bookingId);

                // Assert
                Assert.Null(result);
            }


            [Fact]
            public async Task GetShowTimesByMovieAsync_ShouldReturnListOfShowTimes()
            {
                // Arrange
                var showTimeId = 3;

                var expectedBookingsForShowTime = _fixture.Bookings.Where(st => st.ShowtimeId == showTimeId);

                _fixture.MockRepository.Setup(repo => repo.GetBookingsForShowTimeAsync(showTimeId)).ReturnsAsync(expectedBookingsForShowTime);

                // Act
                var result = await _fixture.MockRepository.Object.GetBookingsForShowTimeAsync(showTimeId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expectedBookingsForShowTime, result);
            }


        [Theory]
        [InlineData(4, 1, 3,2, 40.00, false, "2024-01-30 14:30:00")]
        // Id = 3, ShowtimeId = 1, UserId = 3, NoOfSeats = 2, TotalPrice = 20.0, BookingConfirmed = true, CreatedDateTime = DateTime.Now
        public async Task DeleteOldBookings_ShouldReturnTrue_WhenDeletionSuccessful(int id, int showtimeId, int userId, int noOfSeats,int totalPrice, bool bookingConfirmed,DateTime createdDateTimeStr)
        {
            //var createdDateTime = DateTime.Parse(createdDateTimeStr);
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking
                {
                        Id = id,
                        ShowtimeId = showtimeId,
                        UserId = userId,
                        NoOfSeats = noOfSeats,
                        TotalPrice = totalPrice,
                        BookingConfirmed = bookingConfirmed,
                        CreatedDateTime = createdDateTimeStr
                }
             };
            var mockRepository = new Mock<IBookingRepository>();

            var cutoffTime = DateTime.Now.AddMinutes(-15); // Assuming the cutoff time is 15 minutes ago
                                                           //var mockRepository = new Mock<IBookingRepository>();
            mockRepository.Setup(repo => repo.DeleteOldBookings(cutoffTime))
                .Callback<DateTime>( time =>
                {
                    // Filter bookings that are older than the cutoff time
                    var oldBookings = bookings.Where(b => b.CreatedDateTime < time && !b.BookingConfirmed).ToList();

                    // Perform deletion logic 
                    foreach (var booking in oldBookings)
                    {
                        bookings.Remove(booking);
                    }

                }).ReturnsAsync(true);
                // Act
                var result = await mockRepository.Object.DeleteOldBookings(cutoffTime);

                 // Assert
                Assert.True(result);
        }

        [Fact]
        public async Task DeleteOldBookings_ShouldReturnTrue_WhenDeletionSuccessful2()
        {
            // Arrange

            var cutoffTime = DateTime.Now.AddMinutes(-15); // Assuming the cutoff time is 15 minutes ago
                                                           //var mockRepository = new Mock<IBookingRepository>();
            _fixture.MockRepository.Setup(repo => repo.DeleteOldBookings(cutoffTime)).ReturnsAsync(true);
            // Act
            var result = await _fixture.MockRepository.Object.DeleteOldBookings(cutoffTime);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteOldBookings_ShouldReturnTrue_WhenDeletionSuccessful3()
        {
            // Arrange
            var cutoffTime = DateTime.Now.AddMinutes(-15);

            // Set up the mock repository to return the sample bookings
            _fixture.MockRepository.Setup(repo => repo.GetBookingsForShowTimeAsync(It.IsAny<int>()))
                                  .ReturnsAsync(_fixture.Bookings);

            // Set up the mock repository to delete bookings older than the cutoff time
            _fixture.MockRepository.Setup(repo => repo.DeleteOldBookings(cutoffTime))
                                  .Callback<DateTime>(time =>
                                  {
                                      var oldBookings = _fixture.Bookings.Where(b => b.CreatedDateTime < time && !b.BookingConfirmed).ToList();
                                      // Perform deletion logic by removing from the fixture's list
                                      foreach (var booking in oldBookings)
                                      {
                                          _fixture.Bookings.Remove(booking);
                                      }
                                  })
                                  .ReturnsAsync(true);

            // Act
            var result = await _fixture.MockRepository.Object.DeleteOldBookings(cutoffTime);

            // Assert
            Assert.True(result);

            // Optionally, you can assert that the bookings were removed from the fixture's list
            Assert.DoesNotContain(_fixture.Bookings, b => b.CreatedDateTime < cutoffTime && !b.BookingConfirmed);
        }

    }

}
