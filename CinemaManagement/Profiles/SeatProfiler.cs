using AutoMapper;
using CinemaManagement.DTO;
using CinemaManagement.Models;

namespace CinemaManagement.Profiles
{
    public class SeatProfiler:Profile
    {
        public SeatProfiler()
        {
            CreateMap<Seat, SeatCreateDto>();
            CreateMap<SeatCreateDto, Seat>();
            CreateMap<Seat, SeatDto>();
        }
    }
}
