using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core.Web.Models;
using Microsoft.Extensions.Logging;
using Core.Interface;
using Core.Utility.Filters;

namespace Core.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ILoggerFactory factory, ILogger<BaseController> ilogger, IServiceTest serviceTest, IEnumerable<IServiceTest> serviceTests) : base(factory, ilogger, serviceTest)
        {
            //这边可以直接注入迭代器IEnumerable<IServiceTest>，获取IServiceTest的所有实现的集合
            foreach (var service in serviceTests)
            {
                service.Show("aa", "bb");
            }
        }

        [ServiceFilter(typeof(CustomActionFilterAttribute))]
        public IActionResult Index()
        {
            int i = Convert.ToInt16("sss");
            _ilogger.LogDebug("Home Index Log");
            _serviceTest.Show("stra", "strb");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
