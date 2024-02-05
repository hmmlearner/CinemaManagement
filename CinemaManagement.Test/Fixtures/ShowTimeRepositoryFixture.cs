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
    public class ShowTimeRepositoryFixture
    {
            public Mock<IShowTimeRepository> MockRepository { get; }

            public List<ShowTime> ShowTimes { get; }
            public IShowTimeRepository Repository => MockRepository.Object;

            public ShowTimeRepositoryFixture()
            {
                MockRepository = new Mock<IShowTimeRepository>();
                ShowTimes = new List<ShowTime>
                {
                    // Add your sample showtimes here
                    new ShowTime { Id = 1, StartTime = DateTime.Now, Date = DateTime.Now.Date, ReservedSeats = 0, MovieId = 100, TheaterId = 1, },
                    new ShowTime { Id = 2, StartTime = DateTime.Now.AddHours(2), Date = DateTime.Now.Date, ReservedSeats = 0, MovieId = 100, TheaterId = 1,}
                };

                MockRepository.Setup(repo => repo.GetShowTimesByMovieAsync(It.IsAny<int>()))
                          .ReturnsAsync((int movieId) => ShowTimes.Where(st => st.MovieId == movieId));
                MockRepository.Setup(repo => repo.GetShowTimeAsync(It.IsAny<int>()))
                        .ReturnsAsync((int id) => ShowTimes.FirstOrDefault(o => o.Id == id));
                MockRepository.Setup(m => m.GetShowTimesByMovieAsync(It.IsAny<int>()))
                        .ReturnsAsync((int movieId) => ShowTimes.Where(o => o.MovieId == movieId));

        }

    }
}
