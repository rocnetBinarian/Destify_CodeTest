using Destify_CodeTest.Models.Entities;
using Destify_CodeTest.Models.Services;
using Destify_CodeTest.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Destify_CodeTest.Controllers
{
    [Authorize(Policy = "CUD")]
    [ApiController]
    [Route("Movies")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }


        /// <summary>
        /// Creates a movie using the provided entity data.
        /// </summary>
        /// <param name="movie">The movie to be created.</param>
        /// <returns>Created if successful, or 500 if there was an error.</returns>
        [HttpPost]
        [Route("Create")]
        public IActionResult Create(Movie movie)
        {
            try
            {
                var id = _movieService.Create(movie);
                return Created("Created movie with ID " + id, movie);
            } catch (Exception ex)
            {
                return StatusCode(500, "Failed to create movie: " + ex.ToString());
            }
        }

        /// <summary>
        /// Deletes a movie with the provided Id
        /// </summary>
        /// <param name="id">The Id of the movie to delete.</param>
        /// <returns>Ok if successful, or 404 if no movie with the provided Id could be found.</returns>
        [HttpDelete]
        [Route("Remove")]
        public IActionResult DeleteById(int id)
        {
            var deleted = _movieService.DeleteById(id);
            if (!deleted)
            {
                return NotFound($"Movie with id {id} not found");
            }
            return Ok();
        }

        /// <summary>
        /// Gets all movies.
        /// </summary>
        /// <returns>A list of all movies</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var movies = _movieService.GetAll();
            var rtn = new List<s_Movie>();
            movies.ForEach(m => {
                rtn.Add(_movieService.BuildMovieVM(m));
            });
            return Ok(rtn);
        }

        /// <summary>
        /// Gets a movie with the specified Id.
        /// </summary>
        /// <param name="id">The Id of the movie to retrieve.</param>
        /// <returns>Ok if the movie is found, 404 if not.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            var movie = _movieService.GetById(id);
            if (movie == default)
            {
                return NotFound();
            }
            var rtn = _movieService.BuildMovieVM(movie);
            return Ok(rtn);
        }

        /// <summary>
        /// Updates the provided movie with id <paramref name="movieId"/> to use the values provided by <paramref name="movie"/>
        /// </summary>
        /// <param name="movieId">The Id of the movie to update.</param>
        /// <param name="movie">The updated values to be used.</param>
        /// <returns>
        /// BadRequest if <paramref name="movieId"/> and <paramref name="movie"/>'s Id do not match.
        /// 404 if no movie with id <paramref name="movieId"/> is found.  OK otherwise.
        /// </returns>
        [HttpPatch]
        [Route("Update/{movieId:int}")]
        public IActionResult Update(int movieId, Movie movie) {
            if (movie.Id != default(int) && movieId != movie.Id) {
                return BadRequest("MovieId in request path must match MovieId in request body");
            } 
            var mov = _movieService.Update(movieId, movie);
            if (mov == null) {
                return NotFound("Could not find movie with id " + movieId);
            }
            var rtn = _movieService.BuildMovieVM(mov);
            return Ok(rtn);
        }

        /// <summary>
        /// Replaces the provided movie having id <paramref name="movieId"/> with values provided by <paramref name="movie"/>
        /// </summary>
        /// <param name="movieId">The Id of the movie to replace.</param>
        /// <param name="movie">the new values to be used.</param>
        /// <returns>
        /// BadRequest if <paramref name="movieId"/> and <paramref name="movie"/>'s Id do not match.
        /// 404 if no movie with id <paramref name="movieId"/> is found.  OK if the replacement was successful.
        /// 500 if there was an unexpected error.
        /// </returns>
        [HttpPut]
        [Route("Replace/{movieId:int}")]
        public IActionResult Replace(int movieId, Movie movie) {
            if (movieId != movie.Id) {
                return BadRequest("MovieId in request path must match MovieId in request body");
            }
            var rtn = _movieService.Replace(movieId, movie);
            if (rtn == null) {
                return Ok();
            }
            if (rtn is KeyNotFoundException) {
                return NotFound(rtn.Message);
            }
            return StatusCode(500, rtn.Message);
        }

        /// <summary>
        /// Case-sensitive search for a movie's name.
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns>A list of all movies whose names contain <paramref name="query"/></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("Search/{query}")]
        public IActionResult Search(string query) {
            var movies = _movieService.Search(query);
            var rtn = new List<s_Movie>();
            movies.ForEach(m => {
                rtn.Add(_movieService.BuildMovieVM(m));
            });
            return Ok(rtn);
        }
    }
}
