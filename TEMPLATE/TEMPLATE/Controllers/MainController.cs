using Microsoft.AspNetCore.Mvc;

namespace TEMPLATE.Controllers
{
    [Route("")]
    public class MainController : ControllerBase
    {
        [Route("")]
        public IActionResult Foo()
        {
            logger.Information("Sanity Check.");
            return View("~/Views/MainFoo.cshtml", new Models.ViewModels.MainFooVM());
        }
    }
}
