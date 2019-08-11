using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;

namespace Core.Utility.Filters
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {
        protected ILogger<CustomExceptionFilterAttribute> _ilogger = null;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public CustomExceptionFilterAttribute(IHostingEnvironment hostingEnvironment,
            IModelMetadataProvider modelMetadataProvider, ILogger<CustomExceptionFilterAttribute> ilogger)
        {
            _hostingEnvironment = hostingEnvironment;
            _modelMetadataProvider = modelMetadataProvider;
            _ilogger = ilogger;
        }

        public override void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)//异常没有被处理过
            {
                string controllerName = context.RouteData.Values["controller"].ToString();
                string actionName = context.RouteData.Values["action"].ToString();
                _ilogger.LogError($"{controllerName}_{actionName}:{context.Exception.Message}");
                _ilogger.LogError($"{controllerName}_{actionName}:{context.Exception.StackTrace}");
                if (IsAjaxRequest(context.HttpContext.Request))//检查请求头
                {
                    context.Result = new JsonResult(
                    new
                    {
                        Result = false,
                        PromptMsg = "系统出现异常，请联系管理员",
                        DebugMessage = context.Exception.Message
                    });
                }
                else
                {
                    //不是ajax请求则跳转到错误提示页面
                    var result = new ViewResult { ViewName = "~/Views/Shared/Error.cshtml" };
                    result.ViewData = new ViewDataDictionary(_modelMetadataProvider, context.ModelState);
                    result.ViewData.Add("Exception", context.Exception);
                    context.Result = result;
                }
                context.ExceptionHandled = true;
            }
        }

        /// <summary>
        /// 判断是否是ajax请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool IsAjaxRequest(HttpRequest request)
        {
            string header = request.Headers["X-Requested-With"];
            return "XMLHttpRequest".Equals(header);
        }

    }
}
