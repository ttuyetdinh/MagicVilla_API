using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MagicVilla_VillaAPI.Filters
{
    public class CustomExceptionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // if (context.Exception is FileNotFoundException file){
            //     context.Result = new ObjectResult("File not found is handled in the filter"){
            //         StatusCode = 503
            //     };
            //     context.ExceptionHandled = true;
            // }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
    }
}