using AutoMapper;
using CinemaManagement.DTO;
using CinemaManagement.Filters;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using CinemaManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Controllers
{
    [Route("api/theaters")]
    [ServiceFilter(typeof(ModelStateValidationFilter))]
    public class TheaterController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly ITheaterRepository _theaterRepository;
        private readonly IMapper _mapper;

        public TheaterController(ILogger<Theater> logger, ITheaterRepository theaterRepository, IMapper mapper)
        {
            _logger = logger;
            _theaterRepository = theaterRepository;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> AddTheater([FromBody] TheaterCreateDTO theaterModel)
        {
            if (theaterModel == null)
            {
                return BadRequest("Invalid request body");
            }
            var theater = _mapper.Map<Theater>(theaterModel);//--> movieModel will be of type MovieDTO so need to map it to Movie
            var theaterEixsts = await _theaterRepository.GetTheaterByName(theater.Name);
            if (theaterEixsts != null)
            {
                return BadRequest("Theater already exists");
            }
            try
            {
                _theaterRepository.AddTheater(theater);

                // save the changes
                await _theaterRepository.SaveChangesAsync();

                // Fetch the movie from the data store so the director is included
                await _theaterRepository.GetTheaterById(theater.Id);

                return CreatedAtRoute("GetTheater",
                    new { theaterId = theater.Id },
                    _mapper.Map<Theater>(theater));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the theater.");
            }

        }

        [HttpGet(Name = "getTheaters")]
        public async Task<ActionResult<IEnumerable<TheaterDTO>>> GetTheaters()
        {
            var theaters = await _theaterRepository.GetTheatersAsync();
            return Ok(_mapper.Map<IEnumerable<TheaterDTO>>(theaters));
        }


        [HttpGet("{theaterId}", Name = "getTheater")]
        public async Task<ActionResult<TheaterDTO>> GetTheater(int theaterId)
        {
            var theater = await _theaterRepository.GetTheaterById(theaterId);
            if (theater == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TheaterDTO>(theater));
        }

        [HttpDelete("delete/{theaterId}")]
        public async Task<IActionResult> DeleteMovie(int theaterId)
        {
            var theater = await _theaterRepository.GetTheaterById(theaterId);
            if (theater == null)
            {
                return NotFound();
            }
            try
            {
                _theaterRepository.DeleteTheater(theater);
                await _theaterRepository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the theater.");
            }
        }
    }
}
