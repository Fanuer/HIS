using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Helpers.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HIS.Helpers.Web.Filters
{
    public class IdsNotIdenticalExceptionFilter:ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (!(context.Exception is IdsNotIdenticalException)) return;
            context.ExceptionHandled = true;
            context.ModelState.AddModelError("id", context.Exception.Message);
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    }
}
