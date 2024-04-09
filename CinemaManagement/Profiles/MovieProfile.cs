using AutoMapper;
using CinemaManagement.DTO;
using CinemaManagement.Models;

namespace CinemaManagement.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieCreateDto>();
            CreateMap<MovieCreateDto, Movie>();
            CreateMap<Movie, MovieDto>();


            CreateMap<MovieUpdateDto, Movie>()
                .ForAllMembers(opt => opt.Condition((src, dest, value, context) =>
                value != null &&
                (value is double || (double)value != default(double)) &&
                (value is DateTime || (DateTime)value != default(DateTime)) &&
                (value is int || (int)value != default(int))));




        }
    }
}
