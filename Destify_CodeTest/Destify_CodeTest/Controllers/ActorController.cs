using Destify_CodeTest.Models.Entities;
using Destify_CodeTest.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace Destify_CodeTest.Controllers
{
    [ApiController]
    [Route("Actors")]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _actorService;
        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
        }

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

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var actors = _actorService.GetAll();
            return Ok(actors);
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult Get(int id)
        {
            var actor = _actorService.GetById(id);
            if (actor == default)
            {
                return NotFound();
            }
            return Ok(actor);
        }
    }
}
