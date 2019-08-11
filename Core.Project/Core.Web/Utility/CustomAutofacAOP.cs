using Autofac;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Utility
{
    public class CustomAutofacAOP : IInterceptor
    {
        private IComponentContext _context = null;

        public CustomAutofacAOP(IComponentContext context)
        {
            _context = context;
        }

        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("AOP Begin");

            //获取方法的参数
            Console.WriteLine($"{string.Join(',', invocation.Arguments)}");

            //执行正常的代码
            invocation.Proceed();

            Console.WriteLine("AOP End");
        }
    }
}
