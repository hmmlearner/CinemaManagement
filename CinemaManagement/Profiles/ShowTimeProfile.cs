using AutoMapper;
using CinemaManagement.DTO;
using CinemaManagement.Models;

namespace CinemaManagement.Profiles
{
    public class ShowTimeProfile:Profile
    {
        public ShowTimeProfile()
        {
            CreateMap<ShowTime, ShowTimeCreateDto>();
            CreateMap<ShowTimeCreateDto, ShowTime>();
            CreateMap<ShowTime, ShowTimeDto>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Movie.Title))  
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Movie.Duration))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Movie.Genre))
                .ForMember(dest => dest.TicketPrice, opt => opt.MapFrom(src => src.Movie.TicketPrice))
                .ForMember(dest => dest.TheaterName, opt => opt.MapFrom(src => src.Theater.Name))
                .ForMember(dest => dest.TotalSeats, opt => opt.MapFrom(src => src.Theater.TotalSeats));
        }
    }
}
