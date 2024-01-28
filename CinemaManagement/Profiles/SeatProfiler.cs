using AutoMapper;
using CinemaManagement.DTO;
using CinemaManagement.Models;

namespace CinemaManagement.Profiles
{
    public class SeatProfiler:Profile
    {
        public SeatProfiler()
        {
            CreateMap<Seat, SeatCreateDTO>();
            CreateMap<SeatCreateDTO, Seat>();
            CreateMap<Seat, SeatDTO>();
        }
    }
}
