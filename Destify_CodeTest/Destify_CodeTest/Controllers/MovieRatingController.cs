using Destify_CodeTest.Models.Entities;
using Destify_CodeTest.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Destify_CodeTest.Controllers
{
    [Authorize(Policy = "CUD")]
    [ApiController]
    [Route("MovieRatings")]
    public class MovieRatingController : ControllerBase
    {
        private readonly IMovieRatingService _ratingService;
        public MovieRatingController(IMovieRatingService ratingService)
        {
            _ratingService = ratingService;
        }


        /// <summary>
        /// Creates a movie rating using the provided entity data.
        /// </summary>
        /// <param name="rating">The rating to be created.</param>
        /// <returns>Created if successful, or 500 if there was an error.</returns>
        [HttpPost]
        [Route("Create")]
        public IActionResult Create(MovieRating rating)
        {
            try
            {
                var id = _ratingService.Create(rating);
                return Created("/Get?id=" + id, rating);
            } catch (Exception ex)
            {
                return StatusCode(500, "Failed to create rating: " + ex.ToString());
            }
        }

        /// <summary>
        /// Deletes a rating with the provided Id
        /// </summary>
        /// <param name="id">The Id of the rating to delete.</param>
        /// <returns>Ok if successful, or 404 if no rating with the provided Id could be found.</returns>
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

        /// <summary>
        /// Gets all movie ratings for all movies.  Probably not particularly useful.
        /// </summary>
        /// <returns>A list of all movie ratings</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var ratings = _ratingService.GetAll();
            return Ok(ratings);
        }

        /// <summary>
        /// Gets a movie rating with the specified Id.
        /// </summary>
        /// <param name="id">The Id of the rating to retrieve.</param>
        /// <returns>Ok if the rating is found, 404 if not.</returns>
        [AllowAnonymous]
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

        /// <summary>
        /// Updates the provided rating with id <paramref name="ratingId"/> to use the values provided by <paramref name="rating"/>
        /// </summary>
        /// <param name="ratingId">The Id of the rating to update.</param>
        /// <param name="rating">The updated values to be used.</param>
        /// <returns>
        /// BadRequest if <paramref name="ratingId"/> and <paramref name="rating"/>'s Id do not match.
        /// 404 if no rating with id <paramref name="ratingId"/> is found.  OK otherwise.
        /// </returns>
        [HttpPatch]
        [Route("Update/{ratingId:int}")]
        public IActionResult Update(int ratingId, MovieRating rating) {
            if (rating.Id != default(int) && ratingId != rating.Id) {
                return BadRequest("RatingId in request path must match RatingId in request body");
            }
            var rtn = _ratingService.Update(ratingId, rating);
            if (rtn == null)
            {
                return NotFound("Could not find rating with id " + ratingId);
            }
            return Ok(rtn);
        }

        /// <summary>
        /// Replaces the provided rating having id <paramref name="ratingId"/> with values provided by <paramref name="rating"/>
        /// </summary>
        /// <param name="ratingId">The Id of the rating to replace.</param>
        /// <param name="rating">the new values to be used.</param>
        /// <returns>
        /// BadRequest if <paramref name="ratingId"/> and <paramref name="rating"/>'s Id do not match.
        /// 404 if no rating with id <paramref name="ratingId"/> is found.  OK if the replacement was successful.
        /// 500 if there was an unexpected error.
        /// </returns>
        [HttpPut]
        [Route("Replace/{ratingId:int}")]
        public IActionResult Replace(int ratingId, MovieRating rating) {
            if (ratingId != rating.Id) {
                return BadRequest("RatingId in request path must match RatingId in request body");
            }
            var rtn = _ratingService.Replace(ratingId, rating);
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
