using AutoMapper;
using CinemaManagement.DTO;
using CinemaManagement.Filters;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using CinemaManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Controllers
{
    /// <summary>
    /// Theater controller for handling theater related operations
    /// </summary>
    [Route("api/theaters")]
    [ServiceFilter(typeof(ModelStateValidationFilter))]
    public class TheaterController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly ITheaterRepository _theaterRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheaterController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance used for logging.</param>
        /// <param name="theaterRepository">The repository for accessing theater data.</param>
        /// <param name="mapper">The mapper instance used for object mapping.</param>
        public TheaterController(ILogger<Theater> logger, ITheaterRepository theaterRepository, IMapper mapper)
        {
            _logger = logger;
            _theaterRepository = theaterRepository;
            _mapper = mapper;
        }


        /// <summary>
        /// Adds a new theater.
        /// </summary>
        /// <param name="theaterModel">The theater model.</param>
        /// <returns>Http 201 contentCreated new theater.</returns>
        [HttpPost]
        public async Task<IActionResult> AddTheater([FromBody] TheaterCreateDto theaterModel)
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
                return StatusCode(500, "An error occurred while adding the theater. " + ex.Message);
            }
        }

        /// <summary>
        /// Retrieves a list of theaters.
        /// </summary>
        /// <returns>Http 200 Ok containing a list of TheaterDto objects.</returns>
        [HttpGet(Name = "getTheaters")]
        public async Task<ActionResult<IEnumerable<TheaterDto>>> GetTheaters()
        {
            var theaters = await _theaterRepository.GetTheatersAsync();
            return Ok(_mapper.Map<IEnumerable<TheaterDto>>(theaters));
        }

        /// <summary>
        /// Retrieves a specific theater by its ID.
        /// </summary>
        /// <param name="theaterId">The ID of the theater to retrieve.</param>
        /// <returns>Http 200 Ok containing the TheaterDto object if found, otherwise NotFound.</returns>
        [HttpGet("{theaterId}", Name = "getTheater")]
        public async Task<ActionResult<TheaterDto>> GetTheater(int theaterId)
        {
            var theater = await _theaterRepository.GetTheaterById(theaterId);
            if (theater == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TheaterDto>(theater));
        }


        /// <summary>
        /// Deletes a theater by its ID.
        /// </summary>
        /// <param name="theaterId">The ID of the theater to delete.</param>
        /// <returns>Http 204 NoContent if the theater is successfully deleted, otherwise NotFound.</returns>
        [HttpDelete("delete/{theaterId}")]
        public async Task<IActionResult> DeleteTheater(int theaterId)
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
                return StatusCode(500, "An error occurred while deleting the theater. " + ex.Message);
            }
        }
    }
}
