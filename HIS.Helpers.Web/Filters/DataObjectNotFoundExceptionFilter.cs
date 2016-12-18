using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Helpers.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HIS.Helpers.Web.Filters
{
    public class DataObjectNotFoundExceptionFilter:ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (!(context.Exception is DataObjectNotFoundException)) return;
            context.ExceptionHandled = true;
            context.Result = new NotFoundObjectResult(context.Exception.Message);
        }
    }
}
