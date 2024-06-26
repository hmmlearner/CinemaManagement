﻿using CinemaManagement.Models;

namespace CinemaManagement.DTO
{
    public class BookingDto
    {
        public int Id { get; set; } // Primary key (you can adjust it based on your needs)
        public int ShowtimeId { get; set; } // Foreign key to Showtime
        //public int UserId { get; set; } // Foreign key to User
        public int NoOfSeats { get; set; }
        public double TotalPrice { get; set; }
        public bool BookingConfirmed { get; set; }
        public DateTime CreatedDateTime { get; set; }

        // Navigation properties
        public ShowTime Showtime { get; set; } // Navigation property to Showtime

        public List<SeatDto> Seats { get; set; }
    }
}
