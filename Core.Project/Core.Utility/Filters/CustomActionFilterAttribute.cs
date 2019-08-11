using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utility.Filters
{
    public class CustomActionFilterAttribute : Attribute, IActionFilter
    {
        protected ILogger<CustomActionFilterAttribute> _ilogger = null;

        //用ServiceFilter可以构造函数注入Log4net
        public CustomActionFilterAttribute(ILogger<CustomActionFilterAttribute> ilogger)
        {
            _ilogger = ilogger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("OnActionExecuting");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("OnActionExecuted");
        }
    }
}
