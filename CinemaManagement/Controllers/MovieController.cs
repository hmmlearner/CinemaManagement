using AutoMapper;
using CinemaManagement.Interfaces;
using CinemaManagement.Models;
using CinemaManagement.Services;
using Microsoft.AspNetCore.Mvc;
using CinemaManagement.DTO;
using CinemaManagement.Filters;

namespace CinemaManagement.Controllers
{
    /// <summary>
    /// Movie controller for handling movie related operations
    /// </summary>
    [ServiceFilter(typeof(ModelStateValidationFilter))]
    [Route("api/movies")]
    public class MovieController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance used for logging.</param>
        /// <param name="movieRepository">The repository for accessing movie data.</param>
        /// <param name="mapper">The mapper instance used for object mapping.</param>
        public MovieController(ILogger<Movie> logger, IMovieRepository movieRepository, IMapper mapper)
        {
            _logger = logger;
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a new movie to the data store
        /// </summary>
        /// <param name="movieModel">movie details</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] MovieCreateDto movieModel)
        {
            if (movieModel == null)
            {
                return BadRequest("Invalid request body");
            }
            var movie = _mapper.Map<Movie>(movieModel);//--> movieModel will be of type MovieDTO so need to map it to Movie
            var movieEixsts = await _movieRepository.GetMovieByName(movie.Title);
            if (movieEixsts != null)
            {
                // return BadRequest("Movie already exists"); // should really be a 409 conflict error
                return Conflict(new { message = "Movie already exists" });
            }

            try
            {
                _movieRepository.AddMovie(movie);

                _logger.LogInformation("Added new movie.");

                // save the changes
                await _movieRepository.SaveChangesAsync();

                // Fetch the movie from the data store so the director is included
                await _movieRepository.GetMovieAsync(movie.Id);

                return CreatedAtRoute(nameof(GetMovie),
                    new { movieId = movie.Id },
                    _mapper.Map<Movie>(movie));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the movie. " + ex.Message);

            }


        }

        /// <summary>
        /// Get all movies from the data store
        /// </summary>
        /// <returns>Http 200 OK with list of movies</returns>
        [HttpGet(Name = "getMovies")]
        public async Task<IActionResult> GetMovies()
        {
            var movieEntities = await _movieRepository.GetMoviesAsync();
            return Ok(_mapper.Map<IEnumerable<MovieDto>>(movieEntities));
        }


        /// <summary>
        /// Get a movie by ID
        /// </summary>
        /// <param name="movieId">movieId of type int</param>
        /// <returns>Http 200 OK with movie details</returns>
        [HttpGet("{movieId}", Name = "getMovie")]
        public async Task<IActionResult> GetMovie(int movieId)
        {
            var movie = await _movieRepository.GetMovieAsync(movieId);
            if (movie == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MovieDto>(movie));
        }


        /// <summary>
        /// Update the movie
        /// </summary>
        /// <param name="movieId">movieId of type int</param>
        /// <param name="movieModel">Updated details of movieModel</param>
        /// <returns>Http 200 Ok with updated movie details</returns>        
        [HttpPut("update/{movieId}")]
        public async Task<IActionResult> UpdateMovie(int movieId, [FromBody] MovieUpdateDto movieModel)
        {
            // Find the movie with the specified ID
            var existingMovie = await _movieRepository.GetMovieAsync(movieId);

            if (existingMovie == null)
               {
                   return NotFound($"Movie with ID {movieId} not found.");
               }
            try
            {
                _mapper.Map(movieModel, existingMovie);

                _movieRepository.UpdateMovie(existingMovie);
                await _movieRepository.SaveChangesAsync();

                // For demonstration purposes, let's just return the updated movie
                return Ok(_mapper.Map<MovieDto>(existingMovie));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the movie. " + ex.Message);

            }
        } 

        //Delete Movie using HTTPDelete API for DeleteMovie(int movieId)
        /// <summary>
        /// Delete a movie from the data store
        /// </summary>
        /// <param name="movieId">movieId of type int</param>
        /// <returns>Http 204 NoContent</returns>
        [HttpDelete("delete/{movieId}")]
        public async Task<IActionResult> DeleteMovie(int movieId)
        {
            var movieEntity = await _movieRepository.GetMovieAsync(movieId);
            if (movieEntity == null)
            {
                return NotFound();
            }

            _movieRepository.DeleteMovie(movieEntity);
            await _movieRepository.SaveChangesAsync();

            return NoContent();
        }
    }


}
