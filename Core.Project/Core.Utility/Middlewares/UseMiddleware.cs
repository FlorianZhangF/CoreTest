using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility.Middlewares
{
    public class UseMiddleware
    {
        private readonly RequestDelegate _next;

        public UseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync($"{nameof(UseMiddleware)} test start");
            await _next.Invoke(context);
            await context.Response.WriteAsync($"{nameof(UseMiddleware)} test end");
        }
    }
}
