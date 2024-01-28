using AutoMapper;
using CinemaManagement.DTO;
using CinemaManagement.Models;

namespace CinemaManagement.Profiles
{
    public class TheaterProfile:Profile
    {
        public TheaterProfile()
        {
            CreateMap<Theater, TheaterCreateDTO>();
            CreateMap<TheaterCreateDTO, Theater>();
            CreateMap<Theater, TheaterDTO>(); 
        }

    }
}
