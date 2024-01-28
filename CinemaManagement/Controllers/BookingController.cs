using AutoMapper;
using AutoMapper.Configuration.Conventions;
using CinemaManagement.Data;
using CinemaManagement.DTO;
using CinemaManagement.Filters;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using CinemaManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(ModelStateValidationFilter))]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowTimeRepository _showTimeRepository;
        private readonly IMapper _mapper;

        public BookingController(ILogger<Booking> logger, IBookingRepository bookingRepository, IShowTimeRepository showTimeRepository, IMapper mapper)
        {
            _logger = logger;
            _bookingRepository = bookingRepository;
            _showTimeRepository = showTimeRepository;
            _mapper = mapper;
        }



        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] BookingCreateDTO bookingModel)
        {
            if (bookingModel == null || bookingModel.Seats == null)
            {
                return BadRequest("Invalid request body");
            }
            var booking = _mapper.Map<Booking>(bookingModel);

            var seats = _mapper.Map<List<Seat>>(bookingModel.Seats);

            //need to check if the seat is already booked. To make it simpler 1. Assumption that the seating is randomly assigned rather than user selecting the seat
            //in this case we need to check if the theater is full or not. If full then we need to return bad request
            var (isValid, validatedBooking) = await CheckBookingValidity(booking, seats);
            booking = validatedBooking;
            if (!isValid)
            {
                return BadRequest("Invalid booking");
            }

            using (var transaction = _bookingRepository.BeginTransaction())
            {
                try
                {
                    _bookingRepository.AddBooking(booking);
                    // save the changes
                    await _bookingRepository.SaveChangesAsync();

                    _bookingRepository.AddSeatsToBooking(seats, booking.Id);

                    await _bookingRepository.SaveChangesAsync();
                    // Fetch the movie from the data store so the director is included
                    await _bookingRepository.GetBookingAsync(booking.Id);
                    _logger.LogInformation("Added new AddShowTime.");

                    _bookingRepository.CommitTransaction();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error Creating booking.");
                    _bookingRepository.RollbackTransaction();
                    return BadRequest("Error while booking. Please try again");
                }
            }
            // Fetch the movie from the data store so the director is included
            await _bookingRepository.GetBookingAsync(booking.Id);
            _logger.LogInformation("Added new AddShowTime.");

            return CreatedAtRoute("GetBooking",
                new { bookingId = booking.Id },
                _mapper.Map<Booking>(booking));

        }

        private async Task<(bool IsValid, Booking ValidatedBooking)> CheckBookingValidity(Booking booking, List<Seat> seats)
        {
            var showTime = await _showTimeRepository.GetShowTimeAsync(booking.ShowtimeId);
            
            //check if the showtime is valid and the theater is not full
            if (showTime == null || showTime.ReservedSeats >= showTime.Theater.TotalSeats)
            {
                return (false, null); //BadRequest("Show is fully booked. Pick another Show");
            }
            // check if the seats are available
            var seatsAvailable = await _bookingRepository.CheckIfSeatsAreAvailable(seats, booking.ShowtimeId);
            if (!seatsAvailable)
            {
                return (false, null);//BadRequest("Seats are not available. Please pick another seat");
            }
            booking.TotalPrice = seats.Count * showTime.Movie.TicketPrice;
            // Return true and the updated booking if the booking is valid
            return (true, booking);
        }


        [HttpGet("getBooking/{bookingId}", Name = "GetBooking")]
        public async Task<ActionResult<BookingDTO>> GetBooking(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingAsync(bookingId);
            var bookedSeats = await _bookingRepository.GetSeatsForBookingAsync(bookingId);
            if (booking == null || bookedSeats == null)
            {
                return NotFound();
            }

            var seats = _mapper.Map<List<SeatDTO>>(bookedSeats);
            var bookingDTO = _mapper.Map<BookingDTO>(booking);
            bookingDTO.Seats = seats;
            return Ok(bookingDTO);
        }

        //[HttpGet("GetBooking/{bookingId}", Name = "GetBooking")]
        //public async Task<ActionResult<BookingDTO>> GetBookingInfo(int bookingId)
        //{
        //    var booking = await _bookingRepository.GetBookingInfoAsync(bookingId);
        //    if (booking == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(_mapper.Map<BookingDTO>(booking));
        //}

        [HttpPost("confirmBooking/{bookingId}")]

        //need to return BookingConfirmationDTO with booking details, including the movie, showtime, seats reserved, and total price
        public async Task<IActionResult> ConfirmBooking(int bookingId, string username, string email)
        {
            var booking = await _bookingRepository.GetBookingAsync(bookingId);
            if (booking == null)
            {
                return NotFound();
            }
            // need to check if the dateCreated is within 15 mins of current time. if not then return bad request
            if (DateTime.UtcNow > booking.CreatedDateTime.AddMinutes(15))
            {
                return BadRequest("Booking has expired. Please book again");
            }
            //TODO: neeed to email format using regex?

            using (var transaction = _bookingRepository.BeginTransaction())
            {
                try
                {
                    booking.BookingConfirmed = true;
                    booking.ConfirmedDateTime = DateTime.UtcNow;
                    booking.User.Email = email;
                    booking.User.Name = username;
                    await _bookingRepository.SaveChangesAsync();

                    // need to update the seats as reserved in the showtime table
                    var seats = booking.NoOfSeats;
                    await _showTimeRepository.UpdateShowTime(booking.ShowtimeId, seats);
                    await _showTimeRepository.SaveChangesAsync();
                    
                    _bookingRepository.CommitTransaction();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error confirming booking.");
                    _bookingRepository.RollbackTransaction();
                    return BadRequest("Error while booking. Please try again");
                }
            }
            return Ok("Booking confirmed successfully");
            // i think we need to return AcceptedAtAction statuscode 202 but then a new resource is not really created?? so it fair to return 202?  
            // return AcceptedAtAction("BookingConfirmation", new { id = bookingId }, null);
        }

        [HttpGet("BookingConfirmation/{bookingId}")]
        public async Task<ActionResult<BookingConfirmationDTO>> BookingConfirmation(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingConfirmationAsync(bookingId);
            if (booking == null)
            {
                return NotFound();
            }   
            var bookingDTO = _mapper.Map<BookingConfirmationDTO>(booking);
            var bookedSeats = await _bookingRepository.GetSeatsForBookingAsync(bookingId);
            var seatsDTO = _mapper.Map<List<SeatDTO>>(bookedSeats);
            bookingDTO.Seats = seatsDTO;
            return Ok(bookingDTO);

        }

        [HttpDelete("deleteOldBookings")]
        public async Task<IActionResult> DeleteOldBookings()
        {
            try
            {
                DateTime fifteenMinutesAgo = DateTime.UtcNow.AddMinutes(-15);

                // Retrieve and delete bookings older than 15 minutes
                await _bookingRepository.DeleteOldBookings(fifteenMinutesAgo);


                // Commit changes to the database
                await _bookingRepository.SaveChangesAsync();

                return Ok("Old bookings deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in deleting old bookings.");
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
    
}
