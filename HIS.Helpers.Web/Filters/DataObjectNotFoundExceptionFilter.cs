using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var dataObjectException = context.Exception as DataObjectNotFoundException;
            var serverException = context.Exception as ServerException;

            if (dataObjectException != null || (serverException != null && serverException.StatusCode == HttpStatusCode.NotFound))
            {
                context.ExceptionHandled = true;
                context.Result = new NotFoundObjectResult(context.Exception.Message);
            }
        }
    }
}
