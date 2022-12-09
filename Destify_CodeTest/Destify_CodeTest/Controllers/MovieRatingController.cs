using Destify_CodeTest.Models.Entities;
using Destify_CodeTest.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace Destify_CodeTest.Controllers
{
    [ApiController]
    [Route("MovieRatings")]
    public class MovieRatingController : ControllerBase
    {
        private readonly IMovieRatingService _ratingService;
        public MovieRatingController(IMovieRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create(MovieRating rating)
        {
            try
            {
                var id = _ratingService.Create(rating);
                return Created("Created rating with ID " + id, rating);
            } catch (Exception ex)
            {
                return StatusCode(500, "Failed to create rating: " + ex.ToString());
            }
        }

        [HttpDelete]
        [Route("Remove")]
        public IActionResult DeleteById(int id)
        {
            var deleted = _ratingService.DeleteById(id);
            if (!deleted)
            {
                return NotFound($"Movie rating with id {id} not found");
            }
            return Ok();
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var ratings = _ratingService.GetAll();
            return Ok(ratings);
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            var rating = _ratingService.GetById(id);
            if (rating == default)
            {
                return NotFound();
            }
            return Ok(rating);
        }
    }
}
