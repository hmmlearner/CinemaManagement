using AutoMapper;
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
    [Route("api/showtimes")]
    public class ShowTimeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IShowTimeRepository _showTimeRepository;
        private readonly IMapper _mapper;

        public ShowTimeController(ILogger<Movie> logger, IShowTimeRepository showTimeRepository, IMapper mapper)
        {
            _logger = logger;
            _showTimeRepository = showTimeRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AddShowTime([FromBody] ShowTimeCreateDto showTimeModel)
        {
            if (showTimeModel == null)
            {
                return BadRequest("Invalid request body");
            }
            var showTime = _mapper.Map<ShowTime>(showTimeModel);
            //TODO: need to extract this 
            if (await CheckForConflictingShowTimes(showTime))
            {
                return BadRequest("Theater already has a session on selected Date and Time");
            }

            try
            {
                _showTimeRepository.AddShowTime(showTime);

                _logger.LogInformation("Added new AddShowTime.");

                // save the changes
                await _showTimeRepository.SaveChangesAsync();

                // Fetch the movie from the data store so the director is included
                await _showTimeRepository.GetShowTimeAsync(showTime.Id);

                return CreatedAtRoute("GetShowTime",
                    new { showTimeId = showTime.Id },
                    _mapper.Map<ShowTime>(showTime));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the movie. "+ ex.Message);

            }

        }

        //TODO : NEED AN API TO LIST AVAILABLE SEATS/UNAVAILABLE SEATS FOR THE SHOWTIME

        private async Task<bool> CheckForConflictingShowTimes(ShowTime showTime)
        {
            var conflictingShowTimes = await _showTimeRepository.GetShowTimeByStartTimeAndDateAndTheater(showTime.StartTime, showTime.Date, showTime.TheaterId, showTime.Movie.Duration);
            return conflictingShowTimes != null;
        }



        //[Route("GetShowTime")]
        [HttpGet("getShowTime/{showTimeId}", Name = "GetShowTime")]
        public async Task<ActionResult<ShowTimeDto>> GetShowTime(int showTimeId)
        {
            var showTime = await _showTimeRepository.GetShowTimeAsync(showTimeId);
            if (showTime == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ShowTimeDto>(showTime));
        }

        //[Route("GetShowTimesByMovie")]
        [HttpGet("getShowTimesByMovie/{movieName}", Name = "GetShowTimesByMovie")]
        public async Task<ActionResult<IEnumerable<ShowTimeDto>>> GetShowTimesByMovie(string movieName)
        {
            var showTimes = await _showTimeRepository.GetShowTimesByMovieAsync(movieName);
            if (showTimes == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<ShowTimeDto>>(showTimes));

        }

        [HttpGet("getShowTimesByMovieID/{movieId}", Name = "getShowTimesByMovieID")]
        public async Task<ActionResult<IEnumerable<ShowTimeDto>>> GetShowTimesByMovieID(int movieId)
        {
            var showTimes = await _showTimeRepository.GetShowTimesByMovieAsync(movieId);
            if (showTimes == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<ShowTimeDto>>(showTimes));
        }

        // update showtime -- update or delete is suffice

        [HttpDelete("delete/{showTimeId}")]
        public async Task<IActionResult> DeleteShowTime(int showTimeId)
        {
            var showTime = await _showTimeRepository.GetShowTimeAsync(showTimeId);
            if (showTime == null)
            {
                return NotFound();
            }

            try
            {
                _showTimeRepository.DeleteShowTime(showTime);
                await _showTimeRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting ShowTime. " + ex.Message);

            }
        }
    }
}
