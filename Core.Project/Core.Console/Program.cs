using Newtonsoft.Json;
using System;

namespace Core.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");
            var user = new
            {
                name = "Richard",
                age = 20
            };
            System.Console.WriteLine(JsonConvert.SerializeObject(user));
            System.Console.ReadLine();
        }
    }
}
