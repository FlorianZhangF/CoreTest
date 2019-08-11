using Autofac;
using Autofac.Extras.DynamicProxy;
using Core.Interface;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Utility
{
    public class CustomAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ServiceTest>().As<IServiceTest>().SingleInstance()//注册
                .PropertiesAutowired().EnableInterfaceInterceptors().InterceptedBy(typeof(CustomAutofacAOP));//指定AOP
        }
    }
}
