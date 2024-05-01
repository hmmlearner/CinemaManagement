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

            CreateMap<MovieUpdateDto, Movie>();


        }
    }
}
