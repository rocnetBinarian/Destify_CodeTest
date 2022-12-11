using Destify_CodeTest.Models.Entities;
using Destify_CodeTest.Models.Services;
using Destify_CodeTest.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Destify_CodeTest.Controllers
{
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
    }
}
