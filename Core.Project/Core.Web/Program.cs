﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging((context, LoggingBuilder) =>
            {
                LoggingBuilder.AddFilter("System",LogLevel.Warning);//忽略系统的其他日志
                LoggingBuilder.AddFilter("Microsoft", LogLevel.Warning);//忽略系统的其他日志
                LoggingBuilder.AddLog4Net();
            })
            .UseStartup<Startup>();
    }
}
