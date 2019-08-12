using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utility.Filters
{
    /// <summary>
    /// 自定义的资源Filter
    /// </summary>
    public class CustomResourceFIlterAttribute : Attribute, IResourceFilter
    {
        private static readonly object _lock = new object();
        private static readonly Dictionary<string, object> _Cache = new Dictionary<string, object>();
        private static string _cacheKey;

        /// <summary>
        /// 在控制器实例化之前
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            //从缓存中取请求结果
            _cacheKey = context.HttpContext.Request.Path.ToString();
            if (_Cache.ContainsKey(_cacheKey))
            {
                var result = _Cache[_cacheKey] as ViewResult;
                if (result != null)
                {
                    context.Result = result;
                }
            }
        }

        /// <summary>
        /// 请求处理完之后
        /// </summary>
        /// <param name="context"></param>
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            //缓存请求结果
            if (!string.IsNullOrWhiteSpace(_cacheKey) &&!_Cache.ContainsKey(_cacheKey))
            {
                lock (_lock)
                {
                    if (!_Cache.ContainsKey(_cacheKey))
                    {
                        var result = context.Result as ViewResult;
                        if (result != null)
                        {
                            _Cache.Add(_cacheKey, result);
                        }
                    }
                }
            }
        }
    }
}
