using Core.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Service
{
    public class ServiceTest : IServiceTest
    {
        public void Show()
        {
            Console.WriteLine("This is ServiceTest");
        }
    }
}
