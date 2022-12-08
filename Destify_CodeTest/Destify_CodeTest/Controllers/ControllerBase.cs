using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using ILogger = Serilog.ILogger;

namespace Destify_CodeTest.Controllers
{
    public class ControllerBase : Controller
    {
        protected ILogger logger;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            Type sourceType = ControllerContext.ActionDescriptor.ControllerTypeInfo.AsType();
            logger = Log.ForContext(sourceType);
            logger = logger.ForContext("ReqId", context.HttpContext.TraceIdentifier);
        }
    }
}
