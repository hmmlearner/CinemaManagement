using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagement.Test.Fixtures
{
    public class BookingRepositoryFixture
    {
        public Mock<IBookingRepository> MockRepository { get; }

        public List<Booking> Bookings { get; }
        public IBookingRepository Repository => MockRepository.Object;

        public BookingRepositoryFixture()
        {
            MockRepository = new Mock<IBookingRepository>();
            Bookings = new List<Booking>
                {
                    // Add your sample showtimes here
                    new Booking { Id = 1, ShowtimeId = 1, UserId = 1, NoOfSeats = 2, TotalPrice = 20.0, BookingConfirmed = false, CreatedDateTime = DateTime.Now },
                    new Booking { Id = 2, ShowtimeId = 2, UserId = 2, NoOfSeats = 2, TotalPrice = 20.0, BookingConfirmed = true, CreatedDateTime = DateTime.Now },
                    new Booking { Id = 3, ShowtimeId = 1, UserId = 3, NoOfSeats = 2, TotalPrice = 20.0, BookingConfirmed = true, CreatedDateTime = DateTime.Now },
                    new Booking { Id = 4, ShowtimeId = 1, UserId = 1, NoOfSeats = 2, TotalPrice = 40.0, BookingConfirmed = false, CreatedDateTime = DateTime.Now.AddMinutes(-30) },
            };

            //MockRepository.Setup(repo => repo.GetShowTimesByMovieAsync(It.IsAny<int>()))
            //          .ReturnsAsync((int movieId) => ShowTimes.Where(st => st.MovieId == movieId));
            //MockRepository.Setup(repo => repo.GetShowTimeAsync(It.IsAny<int>()))
            //        .ReturnsAsync((int id) => ShowTimes.FirstOrDefault(o => o.Id == id));
            //MockRepository.Setup(m => m.GetShowTimesByMovieAsync(It.IsAny<int>()))
            //        .ReturnsAsync((int movieId) => ShowTimes.Where(o => o.MovieId == movieId));

        }
    }
}
