using Destify_CodeTest.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace Destify_CodeTest.Controllers
{
    [ApiController]
    [Route("Movies")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Index()
        {
            var movies = _movieService.GetAll();
            return Ok(movies);
        }
    }
}
