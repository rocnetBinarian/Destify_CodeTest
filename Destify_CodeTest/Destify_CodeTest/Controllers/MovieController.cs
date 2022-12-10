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
        [Route("Update")]
        public IActionResult Update(Movie movie) {
            var rtn = _movieService.Update(movie);
            return Ok(rtn);
        }

        [HttpPut]
        [Route("Replace/{ratingId:int}")]
        public IActionResult Replace(int movieId, Movie movie) {
            var rtn = _movieService.Replace(movieId, movie);
            if (rtn == null) {
                return Ok();
            }
            if (rtn is KeyNotFoundException) {
                return NotFound(rtn.Message);
            }
            if (rtn is ArgumentException) {
                return BadRequest(rtn.Message);
            }
            return StatusCode(500, rtn.Message);
        }
    }
}
