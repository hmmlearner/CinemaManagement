using AutoMapper;
using CinemaManagement.DTO;
using CinemaManagement.Models;

namespace CinemaManagement.Profiles
{
    public class TheaterProfile:Profile
    {
        public TheaterProfile()
        {
            CreateMap<Theater, TheaterCreateDto>();
            CreateMap<TheaterCreateDto, Theater>();
            CreateMap<Theater, TheaterDto>(); 
        }

    }
}
