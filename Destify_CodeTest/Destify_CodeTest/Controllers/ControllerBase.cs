using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Destify_CodeTest.Controllers
{
    /// <summary>
    /// Simple base class for all controllers
    /// </summary>
    [Authorize(AuthenticationSchemes = "APIAuthHandler")]
    public class ControllerBase : Controller
    {
        protected ILogger logger;

        /// <summary>
        /// Prepares the logger (which may or may not be used) to include a ReqId trace.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            Type sourceType = ControllerContext.ActionDescriptor.ControllerTypeInfo.AsType();
            logger = Log.ForContext(sourceType);
            logger = logger.ForContext("ReqId", context.HttpContext.TraceIdentifier);
        }
    }
}
