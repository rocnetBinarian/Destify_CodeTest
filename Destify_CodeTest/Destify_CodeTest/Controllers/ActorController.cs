using Destify_CodeTest.Models.Entities;
using Destify_CodeTest.Models.Services;
using Destify_CodeTest.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Destify_CodeTest.Controllers
{
    /// <summary>
    /// Controller for everything actor-related
    /// </summary>
    [Authorize(Policy = "CUD")]
    [ApiController]
    [Route("Actors")]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _actorService;
        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
        }

        /// <summary>
        /// Creates an actor using the provided entity data.
        /// </summary>
        /// <param name="actor">The actor to be created.</param>
        /// <returns>Created if successful, or 500 if there was an error.</returns>
        [HttpPost]
        [Route("Create")]
        public IActionResult Create(Actor actor)
        {
            try
            {
                var id = _actorService.Create(actor);
                return Created("Created actor with ID " + id, actor);
            } catch (Exception ex)
            {
                return StatusCode(500, "Failed to create actor: " + ex.ToString());
            }
        }

        /// <summary>
        /// Deletes an actor with the provided Id
        /// </summary>
        /// <param name="id">The Id of the actor to delete.</param>
        /// <returns>Ok if successful, or 404 if no actor with the provided Id could be found.</returns>
        [HttpDelete]
        [Route("Remove")]
        public IActionResult DeleteById(int id)
        {
            var deleted = _actorService.DeleteById(id);
            if (!deleted)
            {
                return NotFound($"Actor with id {id} not found");
            }
            return Ok();
        }

        /// <summary>
        /// Gets all actors.
        /// </summary>
        /// <returns>A list of all actors</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var actors = _actorService.GetAll();
            var rtn = new List<s_Actor>();
            actors.ForEach(a => {
                rtn.Add(_actorService.BuildActorVM(a));
            });
            return Ok(rtn);
        }

        /// <summary>
        /// Gets an actor with the specified Id.
        /// </summary>
        /// <param name="id">The Id of the actor to retrieve.</param>
        /// <returns>Ok if the actor is found, 404 if not.</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            var actor = _actorService.GetById(id);
            if (actor == default)
            {
                return NotFound();
            }
            var rtn = _actorService.BuildActorVM(actor);
            return Ok(rtn);
        }

        /// <summary>
        /// Updates the provided actor with id <paramref name="actorId"/> to use the values provided by <paramref name="actor"/>
        /// </summary>
        /// <param name="actorId">The Id of the actor to update.</param>
        /// <param name="actor">The updated values to be used.</param>
        /// <returns>
        /// BadRequest if <paramref name="actorId"/> and <paramref name="actor"/>'s Id do not match.
        /// 404 if no actor with id <paramref name="actorId"/> is found.  OK otherwise.
        /// </returns>
        [HttpPatch]
        [Route("Update/{actorId:int}")]
        public IActionResult Update(int actorId, Actor actor) {
            if (actor.Id != default(int) && actorId != actor.Id) {
                return BadRequest("ActorId in request path must match ActorId in request body");
            }
            var act = _actorService.Update(actorId, actor);
            if (act == null) {
                return NotFound("Could not find actor with id " + actorId);
            }
            var rtn = _actorService.BuildActorVM(act);
            return Ok(rtn);
        }

        /// <summary>
        /// Replaces the provided actor having id <paramref name="actorId"/> with values provided by <paramref name="actor"/>
        /// </summary>
        /// <param name="actorId">The Id of the actor to replace.</param>
        /// <param name="actor">the new values to be used.</param>
        /// <returns>
        /// BadRequest if <paramref name="actorId"/> and <paramref name="actor"/>'s Id do not match.
        /// 404 if no actor with id <paramref name="actorId"/> is found.  OK if the replacement was successful.
        /// 500 if there was an unexpected error.
        /// </returns>
        [HttpPut]
        [Route("Replace/{actorId:int}")]
        public IActionResult Replace(int actorId, Actor actor) {
            if (actorId != actor.Id) {
                return BadRequest("ActorId in request path must match ActorId in request body");
            }
            var rtn = _actorService.Replace(actorId, actor);
            if (rtn == null) {
                return Ok();
            }
            if (rtn is KeyNotFoundException) {
                return NotFound(rtn.Message);
            }
            return StatusCode(500, rtn.Message);
        }

        /// <summary>
        /// Case-sensitive search for an actor's name.
        /// </summary>
        /// <param name="query">The search query</param>
        /// <returns>A list of all actors whose names contain <paramref name="query"/></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("Search/{query}")]
        public IActionResult Search(string query) {
            var actors = _actorService.Search(query);
            var rtn = new List<s_Actor>();
            actors.ForEach(a =>  {
                rtn.Add(_actorService.BuildActorVM(a));
            });
            return Ok(rtn);
        }
    }
}
