using Autofac.Extras.DynamicProxy;
using Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Service
{
    //[Intercept(typeof(CustomAutofacAOP))]//这边通过特性进行AOP，但是不是很适合
    public class ServiceTest : IServiceTest
    {
        public void Show(string a,string b)
        {
            Console.WriteLine($"This is ServiceTest a:{a},b:{b}");
        }
    }
}
