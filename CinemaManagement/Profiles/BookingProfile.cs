﻿using AutoMapper;
using CinemaManagement.DTO;
using CinemaManagement.Models;

namespace CinemaManagement.Profiles
{
    public class BookingProfile:Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingCreateDTO>();
            CreateMap<BookingCreateDTO, Booking>();
            CreateMap<Booking, BookingConfirmationDTO>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Showtime.Movie.Title))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Showtime.Movie.Duration))
                .ForMember(dest => dest.TheaterName, opt => opt.MapFrom(src => src.Showtime.Theater.Name))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Showtime.StartTime))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
           /* CreateMap<BookingCreateDTO, Booking>();
                .ForMember(dest => dest.s, opt => opt.MapFrom(src => src.Movie.Title))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Movie.Duration))
                .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Movie.Genre))
                .ForMember(dest => dest.TicketPrice, opt => opt.MapFrom(src => src.Movie.TicketPrice))
                .ForMember(dest => dest.TheaterName, opt => opt.MapFrom(src => src.Theater.Name))
                .ForMember(dest => dest.TotalSeats, opt => opt.MapFrom(src => src.Theater.TotalSeats));*/
        
            CreateMap<Booking, BookingDTO>();
        }
    }
}
