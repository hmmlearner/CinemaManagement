using AutoMapper;
using CinemaManagement.DTO;
using CinemaManagement.Models;

namespace CinemaManagement.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieCreateDTO>();
            CreateMap<MovieCreateDTO, Movie>();
            CreateMap<Movie, MovieDTO>();
            //CreateMap<MovieUpdateDTO, Movie>()
            //    .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null));

            CreateMap<MovieUpdateDTO, Movie>()
                .ForAllMembers(opt => opt.Condition((src, dest, value, context) =>
                value != null &&
                (value.GetType() != typeof(double) || (double)value != default(double)) &&
                (value.GetType() != typeof(DateTime) || (DateTime)value != default(DateTime)) &&
                (value.GetType() != typeof(int) || (int)value != default(int))));


            /*CreateMap<MovieUpdateDTO, Movie>()
                 .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null
                 && // Check if the source value is not null
                (value is double doubleValue && doubleValue != default(double)) || // Check for double
                (value is DateTime dateTimeValue && dateTimeValue != default(DateTime)))); // Check for DateTime
            // .ForAllMembers(opt => opt.Condition((src, dest, value) => value != null && (int)value > 0)); */

        }
    }
}
