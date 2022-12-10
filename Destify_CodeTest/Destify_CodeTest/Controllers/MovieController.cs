using Destify_CodeTest.Models.Entities;
using Destify_CodeTest.Models.Services;
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

        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var movies = _movieService.GetAll();
            return Ok(movies);
        }

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
            return Ok(movie);
        }

        [HttpPatch]
        [Route("Update/{movieId:int}")]
        public IActionResult Update(int movieId, Movie movie) {
            if (movieId != movie.Id) {
                return BadRequest("MovieId in request path must match MovieId in request body");
            } 
            var rtn = _movieService.Update(movieId, movie);
            if (rtn == null) {
                return NotFound("Could not find movie with id " + movieId);
            }
            return Ok(rtn);
        }

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
    }
}
