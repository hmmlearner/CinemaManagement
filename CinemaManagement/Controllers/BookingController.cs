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

    /// <summary>
    /// Controller for managing bookings.
    /// </summary>
    [ApiController]
    [ServiceFilter(typeof(ModelStateValidationFilter))]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowTimeRepository _showTimeRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookingController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="bookingRepository">The booking repository.</param>
        /// <param name="showTimeRepository">The showtime repository.</param>
        /// <param name="mapper">The mapper.</param>
        public BookingController(ILogger<Booking> logger, IBookingRepository bookingRepository, IShowTimeRepository showTimeRepository, IMapper mapper)
        {
            _logger = logger;
            _bookingRepository = bookingRepository;
            _showTimeRepository = showTimeRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Adds a new booking after checking if the booking is valid.
        /// </summary>
        /// <param name="bookingModel">The booking model.</param>
        /// <returns>Http 201 ContentCreated - new booking.</returns>
        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] BookingCreateDto bookingModel)
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

        /// <summary>
        /// Checks the validity of a booking by checking if the showtime is valid and the seats are available.
        /// </summary>
        /// <param name="booking">The booking.</param>
        /// <param name="seats">The seats.</param>
        /// <returns>A tuple indicating if the booking is valid and the validated booking.</returns>
        private async Task<(bool IsValid, Booking ValidatedBooking)> CheckBookingValidity(Booking booking, List<Seat> seats)
        {
            var showTime = await _showTimeRepository.GetShowTimeAsync(booking.ShowtimeId);

            //check if the showtime is valid and the theater is not full
            if (showTime == null || showTime.ReservedSeats >= showTime.Theater.TotalSeats)
            {
                return (false, null);
            }
            // check if the seats are available
            var seatsAvailable = await _bookingRepository.CheckIfSeatsAreAvailable(seats, booking.ShowtimeId);
            if (!seatsAvailable)
            {
                return (false, null);
            }
            booking.TotalPrice = seats.Count * showTime.Movie.TicketPrice;
            // Return true and the updated booking if the booking is valid
            return (true, booking);
        }

        /// <summary>
        /// Gets a booking by ID.
        /// </summary>
        /// <param name="bookingId">The booking ID.</param>
        /// <returns>Http 200 Ok - booking.</returns>
        [HttpGet("getBooking/{bookingId}", Name = "GetBooking")]
        public async Task<ActionResult<BookingDto>> GetBooking(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingAsync(bookingId);
            var bookedSeats = await _bookingRepository.GetSeatsForBookingAsync(bookingId);
            if (booking == null || bookedSeats == null)
            {
                return NotFound();
            }

            var seats = _mapper.Map<List<SeatDto>>(bookedSeats);
            var bookingDTO = _mapper.Map<BookingDto>(booking);
            bookingDTO.Seats = seats;
            return Ok(bookingDTO);
        }

        /// <summary>
        /// Confirms a booking by checking if the booking was created within 15 minutes.
        /// </summary>
        /// <param name="bookingId">The booking ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="email">The email.</param>
        /// <returns>Http 200 OK - The result of the confirmation.</returns>
        [HttpPost("confirmBooking/{bookingId}")]
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
            //TODO:Need to check if the email is valid use regex and send confirmation email to the user.

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

                    //TODO: Also need to update IsAvailable to false in the seats table


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
        }

        /// <summary>
        /// Gets the confirmation details of a booking.
        /// </summary>
        /// <param name="bookingId">The booking ID.</param>
        /// <returns>Http 200 Ok with booking confirmation details.</returns>
        [HttpGet("BookingConfirmation/{bookingId}")]
        public async Task<ActionResult<BookingConfirmationDto>> BookingConfirmation(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingConfirmationAsync(bookingId);
            if (booking == null)
            {
                return NotFound();
            }
            var bookingDto = _mapper.Map<BookingConfirmationDto>(booking);
            var bookedSeats = await _bookingRepository.GetSeatsForBookingAsync(bookingId);
            var seatsDto = _mapper.Map<List<SeatDto>>(bookedSeats);
            bookingDto.Seats = seatsDto;
            return Ok(bookingDto);

        }

        /// <summary>
        /// Gets the unavailable seats for a showtime.
        /// </summary>
        /// <param name="showTimeId">The showtime ID.</param>
        /// <returns>Http 200 OK unavailable seats dataset.</returns>
        [HttpGet("GetUnavailableSeatsForShowTime/{showTimeId}")]
        public async Task<ActionResult<IEnumerable<SeatDto>>> GetUnavailableSeatsForShowTime(int showTimeId)
        {
            var unavaliableSeats = await _bookingRepository.GetNonAvailableSeatsForShowTimeAsync(showTimeId);
            var seatsDTO = _mapper.Map<List<SeatDto>>(unavaliableSeats);
            return Ok(seatsDTO);
        }

        /// <summary>
        /// Deletes expired bookings.
        /// </summary>
        /// <returns>http 200 Ok The result of the deletion.</returns>
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
